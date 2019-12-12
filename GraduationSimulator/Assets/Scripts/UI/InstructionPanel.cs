using UnityEngine.UI;
using UnityEngine;

public class InstructionPanel : Menu
{
    [SerializeField] private Text instructionText = default;

    public void UpdatePanel(EventParams param)
    {
        instructionText.text = param.text;
    }
}
