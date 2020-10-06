using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using TRex.Metadata;

namespace LogicAppCustomConnector.Models
{
    public class Callback
    {
        public Callback()
        {
            this.Id = Guid.NewGuid();
        }
        [Metadata("Callback ID", Visibility = VisibilityType.Internal)]
        public Guid Id { get; }

        [CallbackUrl]
        [Metadata("Callback URL", Visibility = VisibilityType.Internal)]
        public Uri Uri { set; get; }

        public HttpResponseMessage InvokeAsync<TOutput>(TOutput triggerOutput)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            return Task.Run(async () => await httpClient
                .PostAsJsonAsync(Uri, JObject.FromObject(triggerOutput)))
                .Result;
        }
    }
}