# TorboxNET

Torbox .NET wrapper library written in C#

Supports torrent related API calls at the moment. 

## Usage

Create an instance of `TorboxNETClient` for each user you want to authenticate. If you need to support multiple users you will need to create a new instance every time you switch users.

```csharp
var client = new TorboxNETClient("api key");
```


Pass in the Api Key for the user. You can find it in your user settings page

The method naming followings the API documentation as close as I could:
```csharp
var client = new TorboxNETClient(="api key");

// https://www.postman.com/wamy-dev/workspace/torbox/request/29572726-062b717f-4866-4fc0-a3e6-6e4c2520eefa
var result = await client.Torrent.CreateTorrentAsync(magnet);
```

## Authentication

Each user has its own API key, which can be found here: <https://torbox.app/settings>.

## Unit tests

The unit tests are not designed to be ran all at once, they are used to act as a test client.

Set an env variable called "TORBOX_APIKEY" for your api token

Some functions will need replacement ID's to work properly.