using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Infrastructure.DTOs.Holiday
{
    // GoogleHolidayResponse myDeserializedClass = JsonConvert.DeserializeObject<GoogleHolidayResponse>(myJsonResponse); 
    public class GoogleHolidayResponse
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("etag")]
        public string Etag { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("accessRole")]
        public string AccessRole { get; set; }

        [JsonPropertyName("defaultReminders")]
        public List<object> DefaultReminders { get; set; }

        [JsonPropertyName("nextSyncToken")]
        public string NextSyncToken { get; set; }

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }
    }
    public class Creator
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("self")]
        public bool Self { get; set; }
    }
    public class Organizer
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("self")]
        public bool Self { get; set; }
    }
    public class Start
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
    public class End
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }

    public class Item
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("etag")]
        public string Etag { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("htmlLink")]
        public string HtmlLink { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("creator")]
        public Creator Creator { get; set; }

        [JsonPropertyName("organizer")]
        public Organizer Organizer { get; set; }

        [JsonPropertyName("start")]
        public Start Start { get; set; }

        [JsonPropertyName("end")]
        public End End { get; set; }

        [JsonPropertyName("transparency")]
        public string Transparency { get; set; }

        [JsonPropertyName("visibility")]
        public string Visibility { get; set; }

        [JsonPropertyName("iCalUID")]
        public string ICalUID { get; set; }

        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        [JsonPropertyName("eventType")]
        public string EventType { get; set; }
    }
}
