using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

class ColoredTcpClient
{
    private readonly TcpClient client;
    private readonly string userName;

    public ColoredTcpClient(string host, int port, string userName)
    {
        client = new TcpClient();
        this.userName = userName;

        try
        {
            client.Connect(host, port);
            Console.WriteLine($"Добро пожаловать, {userName}");
            Task.Run(() => ReceiveMessagesAsync());
            Task.Run(() => ReceivePointsAsync());
            SendMessageLoop();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            client.Close();
        }
    }

    private async Task ReceiveMessagesAsync()
    {
        using (StreamReader reader = new StreamReader(client.GetStream()))
        {
            while (true)
            {
                try
                {
                    string message = await reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(message))
                    {
                        Print(message);
                    }
                }
                catch
                {
                    break;
                }
            }
        }
    }

    private async Task ReceivePointsAsync()
    {
        using (StreamReader reader = new StreamReader(client.GetStream()))
        {
            while (true)
            {
                try
                {
                    string pointMessage = await reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(pointMessage) && pointMessage.StartsWith("ColoredPoint:"))
                    {
                        var pointInfo = pointMessage.Split(':');
                        int x = int.Parse(pointInfo[1]);
                        int y = int.Parse(pointInfo[2]);
                        ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), pointInfo[3]);
                        string pointUserName = pointInfo[4];
                        PrintPoint(x, y, color, pointUserName);
                    }
                }
                catch
                {
                    break;
                }
            }
        }
    }

    private async void SendMessageLoop()
    {
        using (StreamWriter writer = new StreamWriter(client.GetStream()))
        {
            await writer.WriteLineAsync(userName);
            await writer.FlushAsync();
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

            while (true)
            {
                string userMessage = Console.ReadLine();
                await writer.WriteLineAsync(userMessage);
                await writer.FlushAsync();
            }
        }
    }

    private void Print(string message)
    {
        if (OperatingSystem.IsWindows())
        {
            var position = Console.GetCursorPosition();
            int left = position.Left;
            int top = position.Top;
            Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
            Console.SetCursorPosition(0, top);
            Console.WriteLine(message);
            Console.SetCursorPosition(left, top + 1);
        }
        else
        {
            Console.WriteLine(message);
        }
    }

    private void PrintPoint(int x, int y, ConsoleColor color, string userName)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write("●");
        Console.ResetColor();
        Console.WriteLine($" Point by {userName}");
    }

    static void Main()
    {
        string host = "127.0.0.1";
        int port = 8888;

        Console.Write("Введите свое имя: ");
        string userName = Console.ReadLine();

        ColoredTcpClient tcpClient = new ColoredTcpClient(host, port, userName);
    }
}