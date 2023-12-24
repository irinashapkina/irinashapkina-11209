namespace BattleNetServer.Attribuets;

public class MethodAttribute : Attribute
{
    public MethodAttribute(string actionName)
    {
        ActionName = actionName;
    }

    public string ActionName { get; set; }
}