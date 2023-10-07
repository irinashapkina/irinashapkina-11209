using Newtonsoft.Json;

namespace BattleNetServer.Configuration;

public class AppSettingsConfig
{
    [JsonProperty("address")]
    public string? Address { get; set; }
    
    [JsonProperty("port")]
    public int Port { get; set; }
    
    [JsonProperty("static_file_path")]
    public string? StaticFile { get; set; }
}