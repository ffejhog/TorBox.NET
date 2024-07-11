using Newtonsoft.Json;

namespace TorboxNET.Models.Torrents;

public class CreateTorrentResponse
{
    /// <summary>
    ///     Uploaded data so far.
    /// </summary>
    [JsonProperty("torrent_id")]
    public int Torrent_Id { get; set; }
    
    /// <summary>
    ///     Uploaded data so far.
    /// </summary>
    [JsonProperty("name")]
    public String Name { get; set; }
    
    /// <summary>
    ///     Uploaded data so far.
    /// </summary>
    [JsonProperty("hash")]
    public String Hash { get; set; }
}