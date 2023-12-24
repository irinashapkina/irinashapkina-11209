using System.Net.Sockets;

namespace Dot.Game
{
    public partial class Form1 : Form
    {
        private ColoredTcpClient coloredTcpClient;
        private readonly List<ColoredPoint> coloredPoints = new List<ColoredPoint>();
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private TextBox txtUserName;
        private readonly ServerObject server;
        private readonly string id = Guid.NewGuid().ToString();
        
        //Обработчик кнопки "Connect"
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string host = "127.0.0.1";
                int port = 8888;

                string userName = txtUserName.Text;
                
                // Инициализация клиента и подключение к серверу
                client = new TcpClient();
                client.ConnectAsync(host, port);

                var stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);
                
                //прием точек от сервера     
                Task.Run(() => ReceivePointsAsync());

                this.MouseClick += Form1_MouseClick;
                this.Paint += Form1_Paint;

                // Инициализация объекта ColoredTcpClient для отправки сообщений
                ColoredTcpClient coloredTcpClient = new ColoredTcpClient(client, userName); 
                coloredTcpClient.SendMessageAsync(userName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ReceivePointsAsync()
        {
            while (true)
            {
                try
                {
                    string pointMessage = await reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(pointMessage) && pointMessage.StartsWith("ColoredPoint:"))
                    {
                        // Обработка полученной информации о точке
                        var pointInfo = pointMessage.Split(':');
                        int x = int.Parse(pointInfo[1]);
                        int y = int.Parse(pointInfo[2]);
                        ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), pointInfo[3]);
                        string pointUserName = pointInfo[4];
                        AddColoredPoint(x, y, color, pointUserName);
                        server.BroadcastPoint(new ColoredPoint(x, y, color, pointUserName), id);
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        //добавление точки
        private void AddColoredPoint(int x, int y, ConsoleColor color, string userName)
        {
            coloredPoints.Add(new ColoredPoint(x, y, color, userName));
            Invalidate(); // Force the form to redraw
            Update();
        }

        public void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var point in coloredPoints)
            {
                DrawColoredPoint(e.Graphics, point);
            }
        }

        private void DrawColoredPoint(Graphics g, ColoredPoint point)
        {
            Brush brush = new SolidBrush(GetDrawingColor(point.Color));
            g.FillEllipse(brush, point.X, point.Y, 20, 20);
            g.DrawString(point.UserName, Font, brush, point.X, point.Y + 20);
        }

        private Color GetDrawingColor(ConsoleColor consoleColor)
        {
            switch (consoleColor)
            {
                case ConsoleColor.Black:
                    return Color.Black;
                case ConsoleColor.Blue:
                    return Color.Blue;
                case ConsoleColor.Cyan:
                    return Color.Cyan;
                case ConsoleColor.DarkBlue:
                    return Color.DarkBlue;
                case ConsoleColor.DarkCyan:
                    return Color.DarkCyan;
                case ConsoleColor.DarkGray:
                    return Color.DarkGray;
                case ConsoleColor.DarkGreen:
                    return Color.DarkGreen;
                case ConsoleColor.DarkMagenta:
                    return Color.DarkMagenta;
                case ConsoleColor.DarkRed:
                    return Color.DarkRed;
                case ConsoleColor.DarkYellow:
                    return Color.DarkOrange;
                case ConsoleColor.Gray:
                    return Color.Gray;
                case ConsoleColor.Green:
                    return Color.Green;
                case ConsoleColor.Magenta:
                    return Color.Magenta;
                case ConsoleColor.Red:
                    return Color.Red;
                case ConsoleColor.White:
                    return Color.White;
                case ConsoleColor.Yellow:
                    return Color.Yellow;
                default:
                    return Color.Pink;
            }
        }
        
        //отправка цветной точки на сервер
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            string userName = txtUserName.Text;

            ConsoleColor userColor = (ConsoleColor)Enum.GetValues(typeof(ConsoleColor)).GetValue(new Random().Next(16));
            ColoredPoint coloredPoint = new ColoredPoint(x, y, userColor, userName);
            AddColoredPoint(x, y, userColor, userName);

            ColoredTcpClient coloredTcpClient = new ColoredTcpClient(client, userName);
            coloredTcpClient.SendColoredPointAsync(coloredPoint);
        }
    }

    class ColoredTcpClient
    {
        private readonly TcpClient client;
        private readonly StreamReader reader;
        private readonly StreamWriter writer;

        public ColoredTcpClient(TcpClient tcpClient, string userName)
        {
            client = tcpClient;
            var stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            SendMessageAsync(userName); 
        }

        public async Task SendMessageAsync(string message)
        {
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();
        }

        public async Task SendColoredPointAsync(ColoredPoint coloredPoint)
        {
            string message = $"ColoredPoint:{coloredPoint.X}:{coloredPoint.Y}:{coloredPoint.Color}:{coloredPoint.UserName}";
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();
        }
    }
}
