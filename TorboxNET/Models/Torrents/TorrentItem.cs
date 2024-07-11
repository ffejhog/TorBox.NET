using Newtonsoft.Json;

namespace TorboxNET.Models.Torrents;

public class TorrentItem
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("hash")]
    public string Hash { get; set; }

    [JsonProperty("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonProperty("magnet")]
    public string Magnet { get; set; }

    [JsonProperty("size")]
    public long Size { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }

    [JsonProperty("auth_id")]
    public string AuthId { get; set; }

    [JsonProperty("download_state")]
    public string DownloadState { get; set; }

    [JsonProperty("seeds")]
    public int Seeds { get; set; }

    [JsonProperty("peers")]
    public int Peers { get; set; }

    [JsonProperty("ratio")]
    public double Ratio { get; set; }

    [JsonProperty("progress")]
    public double Progress { get; set; }

    [JsonProperty("download_speed")]
    public long DownloadSpeed { get; set; }

    [JsonProperty("upload_speed")]
    public long UploadSpeed { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("eta")]
    public long Eta { get; set; }

    [JsonProperty("server")]
    public int Server { get; set; }

    [JsonProperty("torrent_file")]
    public bool TorrentFile { get; set; }

    [JsonProperty("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    [JsonProperty("download_present")]
    public bool DownloadPresent { get; set; }

    [JsonProperty("download_finished")]
    public bool DownloadFinished { get; set; }

    [JsonProperty("files")]
    public List<TorrentItemFile> Files { get; set; }

    [JsonProperty("inactive_check")]
    public int InactiveCheck { get; set; }

    [JsonProperty("availability")]
    public int Availability { get; set; }
}

public class TorrentItemFile
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("md5")]
    public string Md5 { get; set; }

    [JsonProperty("s3_path")]
    public string S3Path { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("size")]
    public long Size { get; set; }

    [JsonProperty("mimetype")]
    public string Mimetype { get; set; }

    [JsonProperty("short_name")]
    public string ShortName { get; set; }
}