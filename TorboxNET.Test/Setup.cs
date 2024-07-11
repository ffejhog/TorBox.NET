namespace TorboxNET.Test;

public class Setup
{
    public static String ApiKey => Environment.GetEnvironmentVariable("TORBOX_APIKEY") ?? "";
}