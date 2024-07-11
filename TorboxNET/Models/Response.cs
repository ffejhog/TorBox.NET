using Newtonsoft.Json;

namespace TorboxNET.Models;

internal class Response<T> : ResponseBase
{
    [JsonProperty("data")]
    public T Data { get; set; }
}

internal class Response : ResponseBase
{
    // For cases without a data property
}