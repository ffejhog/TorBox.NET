using TorboxNET.Apis;

namespace TorboxNET;

/// <summary>
/// TorboxNETClient is a client for connecting to the torbox.app api
/// API spec: https://www.postman.com/wamy-dev/workspace/torbox/overview
/// </summary>
public class TorboxNETClient
{
    private readonly Store _store = new();
    public Torrents Torrents;
    public User User;

    public TorboxNETClient(String apiKey, HttpClient? httpClient = null)
    {
        var client = httpClient ?? new HttpClient();
        
        _store.ApiKey = apiKey;
        Torrents = new Torrents(client, _store);
        User = new User(client, _store);

    }
}