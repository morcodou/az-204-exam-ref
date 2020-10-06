using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriberToEventGridTopic
{
   public class NewItemCreatedEventData
    {
        [JsonProperty(PropertyName = "name")]
        public string itemName;
    }
}
