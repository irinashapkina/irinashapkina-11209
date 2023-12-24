namespace BattleNetServer.Attribuets;

public class GetAttribute : MethodAttribute
{
    public GetAttribute(string actionName) : base(actionName)
    {
    }
}