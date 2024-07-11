namespace TorboxNET.Test;

public class TorrentsTest
{
    [Fact]
    public async Task UploadFile()
    {
        var client = new TorboxNETClient(Setup.ApiKey);

        const String filePath = @"big-buck-bunny.torrent";

        var file = await System.IO.File.ReadAllBytesAsync(filePath);

        var result = await client.Torrents.CreateTorrentAsync(file);
        
        Assert.Equal("dd8255ecdc7ca55fb0bbf81323d87062db1f6d1c", result.Hash);
    }
    
    [Fact]
    public async Task UploadMagnet()
    {
        var client = new TorboxNETClient(Setup.ApiKey);

        const String magnet = "magnet:?xt=urn:btih:dd8255ecdc7ca55fb0bbf81323d87062db1f6d1c&dn=Big+Buck+Bunny&tr=udp%3A%2F%2Fexplodie.org%3A6969&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Ftracker.empire-js.us%3A1337&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Ftracker.opentrackr.org%3A1337&tr=wss%3A%2F%2Ftracker.btorrent.xyz&tr=wss%3A%2F%2Ftracker.fastcast.nz&tr=wss%3A%2F%2Ftracker.openwebtorrent.com&ws=https%3A%2F%2Fwebtorrent.io%2Ftorrents%2F&xs=https%3A%2F%2Fwebtorrent.io%2Ftorrents%2Fbig-buck-bunny.torrent";

        var result = await client.Torrents.CreateTorrentAsync(magnet);

        Assert.Equal("dd8255ecdc7ca55fb0bbf81323d87062db1f6d1c", result.Hash);
    }
    
    [Fact]
    public async Task ControlTorrent()
    {
        var client = new TorboxNETClient(Setup.ApiKey);
        
        await client.Torrents.ControlTorrentAsync("76062", "delete");
    }
    
    [Fact]
    public async Task RequestDownloadLink()
    {
        var client = new TorboxNETClient(Setup.ApiKey);
        
        var response = await client.Torrents.RequestDownloadLink("76078");

        Assert.NotNull(response);
    }
    
    [Fact]
    public async Task GetTorrentList()
    {
        var client = new TorboxNETClient(Setup.ApiKey);
        
        var response = await client.Torrents.GetTorrentListAsync();

        Assert.True(response.Count > 0);
    }
}