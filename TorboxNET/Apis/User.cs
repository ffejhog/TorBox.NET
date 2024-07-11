namespace TorboxNET.Apis;

public class User
{
    private readonly Requests _requests;
    
    internal User(HttpClient httpClient, Store store)
    {
        _requests = new Requests(httpClient, store);
    }
    
    /// <summary>
    /// Gets a users account data and information.
    /// </summary>
    /// <param name="settings">Allows you to retrieve user settings.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
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