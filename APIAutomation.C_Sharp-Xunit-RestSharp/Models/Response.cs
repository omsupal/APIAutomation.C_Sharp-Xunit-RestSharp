using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIAutomation.Models
{
    /// <summary>
    /// Valid Response Contract 
    /// This is Common in All the APIS
    /// </summary>
    public class ResponseModel
    {
        [JsonProperty("Context", Required = Required.Always)]
        public Context Context { get; set; }
        [JsonProperty("Response", Required = Required.Always)]
        public Response Response { get; set; }
    }
    /// <summary>
    /// This Should be Common for All the Response 
    /// </summary>
    public class Context
    {
        [JsonProperty("TrackToken", Required = Required.Always)]

        public string TrackToken { get; set; }
        //[JsonProperty("IpAddress", Required = Required.Always)]
        public string IpAddress { get; set; }
        public string Message { get; set; }
    }

    public class Response
    {
        [JsonProperty("SAMPLE_PROPERTY", Required = Required.Always)]
        [MinLength(1)]
        [MaxLength(100)]
        public string SAMPLE_PROPERTY_STRING { get; set; }

        [JsonProperty("SAMPLE_PROPERTY_INT", Required = Required.Always)]
        [Range(0, int.MaxValue)]
        public int SAMPLE_PROPERTY_INT { get; set; }

    }

}
