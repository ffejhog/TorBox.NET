namespace TorboxNET.Test;

public class UserTest
{
    [Fact]
    public async Task GetUser()
    {
        var client = new TorboxNETClient(Setup.ApiKey);
        
        var response = await client.User.GetUser(true);
        
        Assert.NotNull(response);
        Assert.False(response.IsSubscribed);
    }
}