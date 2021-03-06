# AzureSearchDemo
A Windows console app demonstrating the ability of Azure Search.

[Azure Search](https://docs.microsoft.com/en-us/azure/search) is a fully managed search-as-a-service in Microsoft Azure. It offers scalable full-text search for the program. In this program, we use the event data from the [.NET Developers Community Singapore](https://www.meetup.com/NET-Developers-SG) to demonstrate how Azure Search works.

Currently, this demo application covers the following features in Azure Search.
 - Create Azure Search index;
 - Data upload;
 - Query.
 
## Simple Query: Keywords
Search for events which will give participants some door prizes.

![Simple Query: Keywords](AzureSearch/github-images/Azure-Search-Simple-Query.png?raw=true)
 
## Simple Query: NOT Operator
Search for events that cover topics related to HTML5 without mentioning Azure.

![Simple Query: NOT Operator](AzureSearch/github-images/Azure-Search-Not-Operator.png?raw=true)
 
## Simple Query: Phrase Search
Using quotes in the search term, Azure Search will only match documents that contains that whole phrase together and in that order. For example, we want to query events covering Visual Studio.

![Simple Query: Phrase Search](AzureSearch/github-images/Azure-Search-Phrase-Search.png?raw=true)
 
## Lucene Query: Fuzzy Search
A fuzzy search based on [Damerau-Levenshtein Distance](https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance) finds matches in terms that have a similar construction. To do a fuzzy search, use the tilde "~" symbol at the end of a single word with an optional parameter, a value between 0 and 2, that specifies the edit distance.
 
So we do not need to worry if users mistype "Xamarin" as "Zamarin" in the query if they query with Fuzzy Search.

![Lucene Query: Fuzzy Search](AzureSearch/github-images/Azure-Search-Fuzzy-Search.png?raw=true)

## Lucene Query: Proximity Search
Proximity searches are used to find terms that are near each other in a document.

![Lucene Query: Proximity Search](AzureSearch/github-images/Azure-Search-Proximity-Search.png?raw=true)
 
## Lucene Query: Term Boosting
Term boosting refers to ranking an event higher in the search results if it contains the requested boosted term, relative to other events that do not contain the term. For example, we can specify that we want to show those MVC related events having the term "Riza" first.

![Lucene Query: Proximity Search](AzureSearch/github-images/Azure-Search-Term-Boosting.png?raw=true)

## Fields to Customize
Please fill in the values in ApiKeys.config before you can run and test this application.
 - MEETUP_API_KEY
 - AZURE_SEARCH_SERVICE_NAME
 - AZURE_SEARCH_ADMIN_API_KEY
 - AZURE_SEARCH_ADMIN_QUERY_KEY
 
## References
 - [Azure Search](https://docs.microsoft.com/en-us/azure/search/)
 - [Simple Query Syntax](https://docs.microsoft.com/en-us/rest/api/searchservice/simple-query-syntax-in-azure-search) 
 - [Lucene Query](https://docs.microsoft.com/en-us/rest/api/searchservice/Lucene-query-syntax-in-Azure-Search)
 - [Meetup Events API](https://www.meetup.com/meetup_api/docs/:urlname/events/#list)
