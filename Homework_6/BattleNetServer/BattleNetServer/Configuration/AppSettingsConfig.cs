using Newtonsoft.Json;

namespace BattleNetServer.Configuration;

public class AppSettingsConfig
{
    [JsonProperty("address")]
    public string Address { get; set; }
    
    [JsonProperty("port")]
    public int Port { get; set; }
    
    [JsonProperty("staticFilesPath")]
    public string StaticFile { get; set; }
    
    [JsonProperty("emailAddress")]
    public string EmailAddress { get; set; }
    
    [JsonProperty("passwordAddress")]
    public string PasswordAddress { get; set; }
    
    [JsonProperty("senderName")]
    public string SenderName { get; set; }
    
    [JsonProperty("smtpServerHost")]
    public string SmtpServerHost { get; set; }
    
    [JsonProperty("smtpServerPort")]
    public int SmtpServerPort { get; set; }
}