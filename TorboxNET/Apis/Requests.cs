using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TorboxNET.Apis;

internal class Requests
{
    private readonly HttpClient _httpClient;
    private readonly Store _store;
    
    public static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };

    public Requests(HttpClient httpClient, Store store)
    {
        _httpClient = httpClient;
        _store = store;
    }
}