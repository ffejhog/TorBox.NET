using System.Collections.ObjectModel;
using TorboxNET.Models;
using TorboxNET.Models.Torrents;

namespace TorboxNET.Apis;

public class Torrents
{
    private readonly Requests _requests;
    
    internal Torrents(HttpClient httpClient, Store store)
    {
        _requests = new Requests(httpClient, store);
    }
    
    /// <summary>
    /// Creates a torrent under your account.
    /// Simply send either a magnet link, or a torrent file.
    /// Once they have been checked, they will begin downloading assuming your account has available active download slots, and they aren't too large.  
    /// </summary>
    /// <param name="file">The torrent's torrent file.</param>
    /// <param name="seed">Tells TorBox your preference for seeding this torrent. 1 is auto. 2 is seed. 3 is don't seed.</param>
    /// <param name="allowZip">Tells TorBox if you want to allow this torrent to be zipped or not. TorBox only zips if the torrent is 100 files or larger.</param>
    /// <param name="name">The name you want the torrent to be. Optional.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    public async Task<CreateTorrentResponse> CreateTorrentAsync(
        byte[]? file = null,
        int seed = 1,
        bool allowZip = true,
        String? name = null,
        CancellationToken cancellationToken = default)
    {
        if (file == null)
        {
            throw new Exception("You must supply either a file or magnet link.");
        }
        
        var data = new[]
        {
            new KeyValuePair<String, String>("seed", seed.ToString()),
            new KeyValuePair<String, String>("allow_zip", allowZip.ToString()),
            new KeyValuePair<String, String>("name", name),
        };

        var result = await _requests.PostFileRequestAsync<CreateTorrentResponse>("api/torrents/createtorrent", file, data,true, cancellationToken);

        return result;
    }
    
    /// <summary>
    /// Creates a torrent under your account.
    /// Simply send either a magnet link, or a torrent file.
    /// Once they have been checked, they will begin downloading assuming your account has available active download slots, and they aren't too large.  
    /// </summary>
    /// <param name="magnetLink">The torrent's magnet link.</param>
    /// <param name="seed">Tells TorBox your preference for seeding this torrent. 1 is auto. 2 is seed. 3 is don't seed.</param>
    /// <param name="allowZip">Tells TorBox if you want to allow this torrent to be zipped or not. TorBox only zips if the torrent is 100 files or larger.</param>
    /// <param name="name">The name you want the torrent to be. Optional.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    public async Task<CreateTorrentResponse> CreateTorrentAsync(
        String? magnetLink = null,
        int seed = 1,
        bool allowZip = true,
        String? name = null,
        CancellationToken cancellationToken = default)
    {
        if (magnetLink == null)
        {
            throw new Exception("You must supply either a file or magnet link.");
        }
        
        var data = new[]
        {
            new KeyValuePair<String, String>("magnet", magnetLink),
            new KeyValuePair<String, String>("seed", seed.ToString()),
            new KeyValuePair<String, String>("allow_zip", allowZip.ToString()),
            new KeyValuePair<String, String>("name", name),
        };

        var result = await _requests.PostRequestAsync<CreateTorrentResponse>("api/torrents/createtorrent", data,true, cancellationToken);

        return result;
    }
    
    /// <summary>
    /// Controls a torrent. By sending the torrent's ID and the type of operation you want to perform, it will send that request to the torrent client.
    /// </summary>
    /// <param name="torrentId">The torrent's id.</param>
    /// <param name="operation">"reannounce" reannounces the torrent to get new peers
    ///                         "delete" deletes the torrent from the client and your account permanently
    ///                         "pause" pauses a torrent's upload or download
    ///                         "resume" resumes a paused torrent </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    public async Task ControlTorrentAsync(
        String torrentId,
        String operation,
        CancellationToken cancellationToken = default)
    {
        var data = new[]
        {
            new KeyValuePair<String, String>("torrent_id", torrentId),
            new KeyValuePair<String, String>("operation", operation)
        };
        await _requests.PostRequestJsonAsync("api/torrents/controltorrent", data,true, cancellationToken);
    }
    
    /// <summary>
    /// Controls a Queued torrent. By sending the torrent's ID and the type of operation you want to perform, it will send that request to the torrent client.
    /// </summary>
    /// <param name="queuedId">The queued torrent's id.</param>
    /// <param name="operation">"reannounce" reannounces the torrent to get new peers
    ///                         "delete" deletes the torrent from the client and your account permanently
    ///                         "pause" pauses a torrent's upload or download
    ///                         "resume" resumes a paused torrent </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    public async Task ControlQueuedTorrentAsync(
        String queuedId,
        String operation,
        CancellationToken cancellationToken = default)
    {
        var data = new[]
        {
            new KeyValuePair<String, String>("queued_id", queuedId),
            new KeyValuePair<String, String>("operation", operation)
        };
        await _requests.PostRequestJsonAsync("api/torrents/controlqueued", data,true, cancellationToken);
    }
    
    /// <summary>
    /// Requests the download link from the server. Because downloads are metered, TorBox cannot afford to allow free access to the links directly. This endpoint opens the link for 1 hour for downloads.
    /// </summary>
    /// <param name="torrentId">The torrent's id.</param>
    /// <param name="fileId">The file Id's to download(If null all are downloaded</param>
    /// <param name="zip">If you want a zip link. Required if no file_id. Takes precedence over file_id if both are given.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A link to the file or zip of files</returns>
    public async Task<String> RequestDownloadLink(
        String torrentId,
        String? fileId = null,
        bool zip = true,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<String, String>
        {
            {
                "token", _requests.GetToken()
            },
            {
                "torrent_id", torrentId
            },
            {
                "file_id", fileId
            },
            {
                "zip", zip.ToString()
            }
        };
        var response = await _requests.GetRequestAsync<String>("api/torrents/requestdl", false, parameters, cancellationToken);
        return response;
    }
    
    /// <summary>
    /// Gets the user's torrent list. This gives you the needed information to perform other torrent actions.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    public async Task<IList<TorrentItem>> GetTorrentListAsync(CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<String, String>
        {
            {
                "bypass_cache", "true"
            }
        };
        var response = await _requests.GetRequestAsync<IList<TorrentItem>>("api/torrents/mylist", true, parameters, cancellationToken);
        return response;
    }
}