
[SelectImplementationName("Open Text Box")]
public class OpenTextBoxEffect : GameEffect
{
    public string TextToDisplayInBox = "";

    public override bool PlayEffect(GameContext context)
    {
        string[] textArray = TextToDisplayInBox.Split(';');

        foreach (string str in textArray)
        {
            UiManager.Inst.OpenTextBox(str);
        }

        return true;
    }
}
