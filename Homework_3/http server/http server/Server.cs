using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace http_server
{
    public class Server
    {
        public async Task ServeStaticContent()
        {
            HttpListener server = new HttpListener();

            try
            {
                var appConfig = GetAppConfig();
                server.Prefixes.Add($"http://{appConfig.Address}:{appConfig.Port}/");

                Console.WriteLine("Запуск сервера");
                server.Start();
                Console.WriteLine("Сервер успешно запущен");

                while (true)
                {
                    var context = await server.GetContextAsync();
                    var url = context.Request.Url;
                    var response = context.Response;
                    // Определяем путь к запрашиваемому ресурсу
                    string requestedPath = url.AbsolutePath;

                    // Определение contentType с использованием switch
                    string contentType = GetContentType(requestedPath);

                    ServeStaticFile(response, appConfig.StaticPathFiles, requestedPath, contentType);
                    Console.WriteLine($"Запрос обработан для: {requestedPath}");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Файл настроек appsettings.json не найден");
            }
            catch (Exception ex)
            {
                Console.WriteLine("В процессе работы возникла не предвиденная ошибка");
            }
            finally
            {
                server.Stop();
                Console.WriteLine("Сервер завершил свою работу");
            }
        }

        private Appsetting GetAppConfig()
        {
            var json = File.ReadAllText("appsettings.json");
            return JsonConvert.DeserializeObject<Appsetting>(json);
        }

        private string GetContentType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".html":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".svg":
                    return "image/svg+xml";
                default:
                    return "text/html";
            }
        }

        private async Task ServeStaticFile(HttpListenerResponse response, string staticPath, string requestedPath, string contentType)
        {
            var filePath = Path.Combine(staticPath, requestedPath.TrimStart('/'));
            if (File.Exists(filePath))
            {
                response.ContentType = contentType;
                var buffer = await File.ReadAllBytesAsync(filePath);
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            else
            {
                Console.WriteLine("Файл не найден");

                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            response.Close();
        }
    }
}