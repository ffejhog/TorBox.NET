using Newtonsoft.Json;

namespace TorboxNET.Models.User;

public class User
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("plan")]
    public int Plan { get; set; }

    [JsonProperty("total_downloaded")]
    public int TotalDownloaded { get; set; }

    [JsonProperty("customer")]
    public string Customer { get; set; }

    [JsonProperty("server")]
    public int Server { get; set; }

    [JsonProperty("is_subscribed")]
    public bool IsSubscribed { get; set; }

    [JsonProperty("premium_expires_at")]
    public DateTime? PremiumExpiresAt { get; set; }

    [JsonProperty("cooldown_until")]
    public DateTime? CooldownUntil { get; set; }

    [JsonProperty("auth_id")]
    public string AuthId { get; set; }

    [JsonProperty("user_referral")]
    public string UserReferral { get; set; }

    [JsonProperty("base_email")]
    public string BaseEmail { get; set; }
}