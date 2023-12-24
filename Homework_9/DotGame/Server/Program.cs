using System.Net;
using System.Net.Sockets;

ServerObject server = new ServerObject();// создаем сервер
await server.ListenAsync(); // запускаем сервер

public class ColoredUser
{
    public string UserName { get; set; }
    public ConsoleColor Color { get; set; }

    public ColoredUser(string userName, ConsoleColor color)
    {
        UserName = userName;
        Color = color;
    }
}
public class ColoredPoint
{
    public int X { get; set; }
    public int Y { get; set; }
    public ConsoleColor Color { get; set; }
    public string UserName { get; set; }

    public ColoredPoint(int x, int y, ConsoleColor color, string userName)
    {
        X = x;
        Y = y;
        Color = color;
        UserName = userName;
    }
}

public class ServerObject
{
    TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888); // сервер для прослушивания
    List<ClientObject> clients = new List<ClientObject>(); // все подключения
    
    // Метод для удаления подключения по идентификатору
    protected internal void RemoveConnection(string id)
    {
        // получаем по id закрытое подключение
        ClientObject? client = clients.FirstOrDefault(c => c.Id == id);
        // и удаляем его из списка подключений
        if (client != null) clients.Remove(client);
        client?.Close();
    }
    public ConsoleColor GetRandomConsoleColor()
    {
        ConsoleColor[] colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
        return colors[new Random().Next(colors.Length)];
    }
    // прослушивание входящих подключений
    protected internal async Task ListenAsync()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");
 
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
 
                ConsoleColor userColor = GetRandomConsoleColor(); // или выберите цвет каким-то другим способом
                ClientObject clientObject = new ClientObject(tcpClient, this, userColor);
                clients.Add(clientObject);
                Task.Run(clientObject.ProcessAsync);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }
 
    // трансляция сообщения подключенным клиентам
    protected internal async Task BroadcastMessageAsync(string message, string id)
    {
        foreach (var client in  clients)
        {
            if (client.Id != id) // если id клиента не равно id отправителя
            {
                await client.Writer.WriteLineAsync(message); //передача данных
                await client.Writer.FlushAsync();
            }
        }
    }
    //трансляции цветной точки подключенным клиентам
    public async Task BroadcastPoint(ColoredPoint point, string id)
    {
        foreach (var client in clients)
        {
            if (client.Id != id)
            {
                client.AddColoredPoint(point);
                await client.SendColoredPointsAsync();
            }
        }
    }

    // отключение всех клиентов
    protected internal void Disconnect()
    {
        foreach (var client in clients)
        {
            client.Close(); //отключение клиента
        }
        tcpListener.Stop(); //остановка сервера
    }

}
class ClientObject
{
    List<ColoredPoint> coloredPoints = new List<ColoredPoint>();
    protected internal string Id { get;} = Guid.NewGuid().ToString();
    protected internal StreamWriter Writer { get;}
    protected internal StreamReader Reader { get;}
    public ConsoleColor UserColor { get; private set; }

 
    TcpClient client;
    ServerObject server; // объект сервера
 
    public ClientObject(TcpClient tcpClient, ServerObject serverObject, ConsoleColor userColor)
    {
        client = tcpClient;
        server = serverObject;
        // получаем NetworkStream для взаимодействия с сервером
        var stream = client.GetStream();
        // создаем StreamReader для чтения данных
        Reader = new StreamReader(stream);
        // создаем StreamWriter для отправки данных
        Writer = new StreamWriter(stream);
        UserColor = userColor;
    }
    //добавление цветной точки
    protected internal void AddColoredPoint(ColoredPoint coloredPoint)
    {
        coloredPoints.Add(coloredPoint);
    }
    //отправка цветных точек
    protected internal async Task SendColoredPointsAsync()
    {
        foreach (var coloredPoint in coloredPoints)
        {
            string message = $"ColoredPoint: {coloredPoint.X},{coloredPoint.Y},{coloredPoint.Color},{coloredPoint.UserName}";
            await Writer.WriteLineAsync(message);
            await Writer.FlushAsync();
        }
    }
    public async Task ProcessAsync()
    {
        try
        {
            // получаем имя пользователя
            string? userName = await Reader.ReadLineAsync();
            string? message = $"{userName} вошел в чат";
            // посылаем сообщение о входе в чат всем подключенным пользователям
            await server.BroadcastMessageAsync(message, Id);
            Console.WriteLine(message);
            // в бесконечном цикле получаем сообщения от клиента
            while (true)
            {
                try
                {
                    message = await Reader.ReadLineAsync();
                    if (message == null) continue;
                    message = $"{userName}: {message}";
                    Console.WriteLine(message);
                    await server.BroadcastMessageAsync(message, Id);
                }
                catch
                {
                    message = $"{userName} покинул чат";
                    Console.WriteLine(message);
                    await server.BroadcastMessageAsync(message, Id);
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            // в случае выхода из цикла закрываем ресурсы
            server.RemoveConnection(Id);
        }
    }
    // закрытие подключения
    protected internal void Close()
    {
        Writer.Close();
        Reader.Close();
        client.Close();
    }
}