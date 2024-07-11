using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TorboxNET.Exceptions;
using TorboxNET.Models;

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
    
    private async Task<String?> Request(String url,
        Boolean requireAuthentication, 
        RequestType requestType,
        HttpContent? data,
        IDictionary<String, String>? parameters,
        CancellationToken cancellationToken)
    {
        url = $"{Store.API_URL}{url}";

        if (parameters is {Count: > 0})
        {
            var parametersString = string.Join("&", 
                parameters
                    .Where(p => p.Value != null)  // Exclude parameters with empty values
                    .Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}")
            );
            if (!string.IsNullOrEmpty(parametersString))
            {
                url = $"{url}?{parametersString}";
            }
        }

        _httpClient.DefaultRequestHeaders.Remove("Authorization");

        if (requireAuthentication)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_store.ApiKey}");
        }

        var response = requestType switch
        {
            RequestType.Get => await _httpClient.GetAsync(url, cancellationToken),
            RequestType.Post => await _httpClient.PostAsync(url, data, cancellationToken),
            RequestType.Put => await _httpClient.PutAsync(url, data, cancellationToken),
            RequestType.Delete => await _httpClient.DeleteAsync(url, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null)
        };

        var buffer = await response.Content.ReadAsByteArrayAsync();
        var text = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            text = null;
        }
            
        return text;
    }
    
    private async Task<T> Request<T>(String url,
        Boolean requireAuthentication,
        RequestType requestType,
        HttpContent? data,
        IDictionary<String, String>? parameters,
        CancellationToken cancellationToken)
        where T : class
    {
        var requestResult = await Request(url, requireAuthentication, requestType, data, parameters, cancellationToken);

        if (requestResult == null)
        {
            return null;
        }

        try
        {
            var result = JsonConvert.DeserializeObject<Response<T>>(requestResult, JsonSerializerSettings);

            if (result == null)
            {
                throw new Exception("Response was null");
            }

            if (result.Success != true)
            {
                if (result.Detail != null)
                {
                    throw new TorboxException(result.Detail);
                }

                throw new JsonSerializationException($"Unknown error. Response was: {result}");
            }

            if (result.Data == null)
            {
                return null;
            }

            return result.Data;
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to deserialize Torbox API response to {typeof(T).Name}. Response was: {requestResult}", ex);
        }
    }
    
    private async Task<Response> RequestFullResponse(String url,
        Boolean requireAuthentication,
        RequestType requestType,
        HttpContent? data,
        IDictionary<String, String>? parameters,
        CancellationToken cancellationToken)
    {
        var requestResult = await Request(url, requireAuthentication, requestType, data, parameters, cancellationToken);

        if (requestResult == null)
        {
            return new Response();
        }
        var result = JsonConvert.DeserializeObject<Response>(requestResult, JsonSerializerSettings);

        if (result == null)
        { 
            throw new Exception("Response was null");
        }

        if (result.Success != true)
        {
            if (result.Detail != null)
            {
                throw new TorboxException(result.Detail);
            }

            throw new JsonSerializationException($"Unknown error. Response was: {result}");
        }

        return result;
    }
    
    public async Task<T> GetRequestAsync<T>(String url, Boolean requireAuthentication, IDictionary<String, String>? parameters, CancellationToken cancellationToken)
        where T : class
    {
        return await Request<T>(url, requireAuthentication, RequestType.Get, null, parameters, cancellationToken);
    }
        
    public async Task<T> PostRequestAsync<T>(String url, IEnumerable<KeyValuePair<String, String>>? data, Boolean requireAuthentication, CancellationToken cancellationToken)
        where T : class, new()
    {
        var content = data != null ? new FormUrlEncodedContent(data) : null;
        return await Request<T>(url, requireAuthentication, RequestType.Post, content, null, cancellationToken);
    }
    
    public async Task PostRequestJsonAsync(String url, IEnumerable<KeyValuePair<String, String>>? data, Boolean requireAuthentication, CancellationToken cancellationToken)
    {
        var content = data != null ? new StringContent(JsonConvert.SerializeObject(data.ToDictionary(x => x.Key, x => x.Value)),
                                                       Encoding.UTF8, 
                                              "application/json") : null;
        await RequestFullResponse(url, requireAuthentication, RequestType.Post, content, null, cancellationToken);
    }
    
    public async Task<T> PostFileRequestAsync<T>(String url, Byte[] file, IEnumerable<KeyValuePair<String, String>>? data, Boolean requireAuthentication, CancellationToken cancellationToken)
        where T : class, new()
    {
        using var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Headers.ContentType.MediaType = "multipart/form-data";

        var fileContent = new ByteArrayContent(file);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-bittorrent");
        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName ="1.torrent"
        };
        multipartFormDataContent.Add(fileContent, "file", "1.torrent");
        // Add each form field individually to the multipart content
        if (data != null)
        {
            foreach (var item in data)
            {
                // Use empty string for null values
                if (item.Value != null)
                {
                    multipartFormDataContent.Add(new StringContent(item.Value), item.Key);
                }

            }
        }
        return await Request<T>(url, requireAuthentication, RequestType.Post, multipartFormDataContent, null, cancellationToken);
    }

    internal String GetToken()
    {
        return _store.ApiKey ?? "";
    }
    
    private enum RequestType
    {
        Get,
        Post,
        Put,
        Delete
    }
}