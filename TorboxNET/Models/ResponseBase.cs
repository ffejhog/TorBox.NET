using Newtonsoft.Json;

namespace TorboxNET.Models;

internal class ResponseBase
{
    [JsonProperty("success")]
    public Boolean Success { get; set; }

    [JsonProperty("detail")]
    public String? Detail { get; set; }
}