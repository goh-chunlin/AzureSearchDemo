using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureSearch
{
    class Program
    {
        static string MEETUP_API_KEY = "";
        static string AZURE_SEARCH_SERVICE_NAME = "";
        static string AZURE_SEARCH_ADMIN_API_KEY = "";
        static string AZURE_SEARCH_ADMIN_QUERY_KEY = "";

        static void Main(string[] args)
        {
            MEETUP_API_KEY = ConfigurationManager.AppSettings.Get("MEETUP_API_KEY");
            AZURE_SEARCH_SERVICE_NAME = ConfigurationManager.AppSettings.Get("AZURE_SEARCH_SERVICE_NAME");
            AZURE_SEARCH_ADMIN_API_KEY = ConfigurationManager.AppSettings.Get("AZURE_SEARCH_ADMIN_API_KEY");
            AZURE_SEARCH_ADMIN_QUERY_KEY = ConfigurationManager.AppSettings.Get("AZURE_SEARCH_ADMIN_QUERY_KEY");

            StartSearch().Wait();
        }

        static async Task StartSearch()
        {
            SearchServiceClient serviceClient = CreateSearchServiceClient();

            Console.WriteLine("Deleting index...\n");
            await DeleteExistingIndexAsync(serviceClient);

            Console.WriteLine("Creating index...\n");
            await CreateIndexAsync(serviceClient);

            var indexClient = serviceClient.Indexes.GetClient("meetup-events");

            Console.WriteLine("{0}", "Uploading documents...\n");
            await UploadDocumentsAsync(indexClient);

            var indexClientForQueries = CreateSearchIndexClient();
            RunQueries(indexClientForQueries);
        }

        private static void RunQueries(ISearchIndexClient indexClient)
        {
            SearchParameters parameters;
            DocumentSearchResult<MeetupEvent> results;

            bool isContinue = true;
            while (isContinue)
            {

                Console.WriteLine("==========================================");
                Console.WriteLine("   WELCOME TO AZURE SEARCH DEMO CONSOLE   ");
                Console.WriteLine("==========================================");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Please enter search query.");
                Console.WriteLine("Enter 999 if you want to exit.");

                string searchQuery = Console.ReadLine();

                int option = 999;
                int.TryParse(searchQuery, out option);

                if (option == 999)
                {
                    isContinue = false;
                }
                else
                {
                    Console.WriteLine($"Search the entire index for the term '{searchQuery}':\n");

                    parameters =
                        new SearchParameters()
                        {
                            Select = new[] { "id", "name", "description", "link" },
                            QueryType = QueryType.Full
                        };

                    results = indexClient.Documents.Search<MeetupEvent>(searchQuery, parameters);

                    WriteDocuments(results);
                }
            }
        }

        private static void WriteDocuments(DocumentSearchResult<MeetupEvent> searchResults)
        {
            foreach (SearchResult<MeetupEvent> result in searchResults.Results)
            {
                Console.WriteLine(result.Document);
            }

            if (searchResults.Results.Count() == 0)
            {
                Console.WriteLine("No results.");
            }

            Console.WriteLine();
        }

        private static async Task DeleteExistingIndexAsync(SearchServiceClient serviceClient)
        {
            if (await serviceClient.Indexes.ExistsAsync("meetup-events"))
            {
                await serviceClient.Indexes.DeleteAsync("meetup-events");
            }
        }

        private static async Task CreateIndexAsync(SearchServiceClient serviceClient)
        {
            var definition = new Index()
            {
                Name = "meetup-events",
                Fields = FieldBuilder.BuildForType<MeetupEvent>()
            };

            await serviceClient.Indexes.CreateAsync(definition);
        }

        private static async Task UploadDocumentsAsync(ISearchIndexClient indexClient)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var meetupEvents = Enumerable.Empty<MeetupEvent>();

                    string previousResponseString = "";
                    int pageNumber = 0;
                    do
                    {
                        var meetupResponse = await client.GetAsync("https://api.meetup.com/NET-Developers-SG/events?key=" + MEETUP_API_KEY +
                            "&status=past&offset=" + pageNumber);
                        meetupResponse.EnsureSuccessStatusCode();

                        string responseString = await meetupResponse.Content.ReadAsStringAsync();

                        if (responseString == previousResponseString)
                        {
                            break;
                        }

                        var currentPageEvents = JsonConvert.DeserializeObject<IEnumerable<MeetupEvent>>(responseString);

                        meetupEvents = meetupEvents.Concat(currentPageEvents);

                        previousResponseString = responseString;
                        pageNumber++;
                    } while (true);

                    var batch = IndexBatch.Upload(meetupEvents.Take(1000));

                    try
                    {
                        indexClient.Documents.Index(batch);
                    }
                    catch (IndexBatchException e)
                    {
                        Console.WriteLine($"Failed to index some of the documents: { string.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)) }");
                    }

                    Console.WriteLine("Waiting for documents to be indexed...\n");
                    Thread.Sleep(2000);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Exception at uploading: { ex.Message }");
                }
            }            
        }

        private static SearchServiceClient CreateSearchServiceClient()
        {
            return new SearchServiceClient(AZURE_SEARCH_SERVICE_NAME, new SearchCredentials(AZURE_SEARCH_ADMIN_API_KEY));
        }

        private static SearchIndexClient CreateSearchIndexClient()
        {
            return new SearchIndexClient(AZURE_SEARCH_SERVICE_NAME, "meetup-events", new SearchCredentials(AZURE_SEARCH_ADMIN_QUERY_KEY));
        }
    }
}
