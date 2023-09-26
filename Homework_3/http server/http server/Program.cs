using http_server;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var serv = new Server();
        serv.ServeStaticContent();
        Console.ReadKey();
    }
}