using Newtonsoft.Json;

namespace BattleNetServer.Configuration;

public class ServerConfiguration
{
     private const string configName = "appsettings.json";
     
    static ServerConfiguration()
    {
         GetConfigurationAsync();
    }
     public static AppSettingsConfig Config { get; private set; }

     private static void GetConfigurationAsync()
     {
         try
         {
             var json = File.OpenText(configName).ReadToEnd();
             var obj = JsonConvert.DeserializeObject<AppSettingsConfig>(json);
             CreateStaticFileDirectory(obj);
             Config = obj;
         }
         catch (Exception e)
         {
             Console.WriteLine("Файл appsettings.json не был найден.");
             throw;
         }
     }
     private static void CreateStaticFileDirectory(AppSettingsConfig config)
     { 
         try
         {
             if (!Directory.Exists(config.StaticFile))
             {
                 Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), config.StaticFile));
                 Console.WriteLine($"Папка static была успешно создана в пути: {config.StaticFile}");
             }
         }
         catch (Exception e)
         { 
             Console.WriteLine($"Произошла ошибка при создании папки по указанному пути: {config.StaticFile}");
             throw;
         }
     }
}