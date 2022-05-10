using UnityEngine;

[System.Serializable] [SelectImplementationName("Log")]
public class LogMessageEffect : GameEffect
{
    public LogType LogTypeToUse;
    [SerializeReference] [SelectImplementation]
    public FirstGameVarSelector DataToDisplay;
    public override void PlayEffect()
    {
        string messageToDisplay = DataToDisplay.GetFinalGameVarWrapper().ToString();

        switch (LogTypeToUse)
        {
            case LogType.Error:
                Debug.LogError(messageToDisplay);
                break;
            case LogType.Assert:
                Debug.LogAssertion(messageToDisplay);
                break;
            case LogType.Warning:
                Debug.LogWarning(messageToDisplay);
                break;
            case LogType.Log:
                Debug.Log(messageToDisplay);
                break;
            default:
                Debug.LogError(messageToDisplay);
                break;
        }
    }
}
