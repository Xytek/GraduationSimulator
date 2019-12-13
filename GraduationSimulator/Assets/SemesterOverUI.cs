using UnityEngine.UI;
using UnityEngine;
public class SemesterOverUI : Menu
{
    [SerializeField] private Text instructionText = default;

    public void UpdatePanel(EventParams param)
    {
        instructionText.text = param.text;
    }
}
