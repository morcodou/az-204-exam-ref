using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublishCustomToEventGrid
{
    class NewItemCreatedEvent
    {
        [JsonProperty(PropertyName = "name")]
        public string itemName;
    }
}
