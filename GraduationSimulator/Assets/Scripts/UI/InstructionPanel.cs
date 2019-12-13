using UnityEngine.UI;
using UnityEngine;

public class InstructionPanel : Menu
{
    [SerializeField] private Text instructionText = default;

    public void SetPanelText(string text)
    {
        instructionText.text = text;
    }

    public void UpdatePanel(EventParams param)
    {
        SetPanelText(param.text);
    }
}
