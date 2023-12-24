namespace BattleNetServer.Attribuets;

public class PostAttribute : MethodAttribute
{
    public PostAttribute(string actionName) : base(actionName)
    {
    }
}