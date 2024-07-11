namespace TorboxNET.Apis;

public class User
{
    private readonly Requests _requests;
    
    internal User(HttpClient httpClient, Store store)
    {
        _requests = new Requests(httpClient, store);
    }
    
    public async Task<Models.User.User> GetUser(bool settings = false, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<String, String>
        {
            {
                "settings", settings.ToString()
            }
        };
        var response = await _requests.GetRequestAsync<Models.User.User>("api/user/me", true, parameters, cancellationToken);
        return response;
    }
}