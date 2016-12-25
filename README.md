# AzureSearchDemo
A Windows console app demonstrating the ability of Azure Search.

Azure Search is a fully managed search-as-a-service in Microsoft Azure. It offers scalable full-text search for the program. In this program, we use the event data from .NET Developers Community Singapore to demonstrate how Azure Search works.

Currently, this demo application covers the following features in Azure Search.
 - Create Azure Search index;
 - Data upload;
 - Query.
 
## Simple Query: Keywords
Search for events which will give participants some door prizes.
 
## Simple Query: NOT Operator
Search for events that cover topics related to HTML5 without mentioning Azure.
 
## Simple Query: Search Phrase
Using quotes in the search term, Azure Search will only match documents that contains that whole phrase together and in that order. For example, we want to query events covering Visual Studio.
 
## Lucene Query: Fuzzy Search
A fuzzy search based on [Damerau-Levenshtein Distance](https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance) finds matches in terms that have a similar construction. To do a fuzzy search, use the tilde "~" symbol at the end of a single word with an optional parameter, a value between 0 and 2, that specifies the edit distance.
 
So we do not need to worry if users mistype "Xamarin" as "Zamarin" in the query if they query with Fuzzy Search.
 
## Lucene Query: Term Boosting
Term boosting refers to ranking an event higher in the search results if it contains the requested boosted term, relative to other events that do not contain the term. For example, we can specify that we want to show those MVC related events having the term "Riza" first.

## Lucene Query: Proximity Search
Proximity searches are used to find terms that are near each other in a document.
