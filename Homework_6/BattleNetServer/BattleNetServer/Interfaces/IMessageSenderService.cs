namespace BattleNetServer.Services;

public interface IMessageSenderService
{
    void SendEmail(string inputEmail, string inputPassword, string subj, string filePath);
}