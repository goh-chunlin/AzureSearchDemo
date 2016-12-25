using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureSearch
{
    [SerializePropertyNamesAsCamelCase]
    public class MeetupEvent
    {
        [Key]
        [IsFilterable]
        [JsonProperty("id")]
        public string Id { get; set; }

        [IsFilterable, IsSortable, IsSearchable]
        public string Name { get; set; }

        [IsSortable]
        [JsonProperty("created")]
        public long? CreatedAt { get; set; }

        [IsSearchable, IsFilterable]
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("link")]
        public string MeetupUrl { get; set; }

        public override string ToString()
        {
            string eventDescription = Description;

            if (eventDescription.Length > 500)
            {
                eventDescription = Description.Substring(0, 500) + "...";
            }

            return ">> Event " + Id + ": " + Name + Environment.NewLine +
                "Description: " + Regex.Replace(eventDescription, "<.*?>", String.Empty) + Environment.NewLine +
                "URL: " + MeetupUrl + Environment.NewLine + 
                Environment.NewLine;
        }
    }
}
