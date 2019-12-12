using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Image lockImage;
    public Button upgradeButton;
    public Image checkImage;

    public void Activate()
    {
        lockImage.enabled = false;
        upgradeButton.interactable = true;
    }

    public void Deactivate()
    {
        lockImage.enabled = true;
        upgradeButton.interactable = false;
    }

    public void Used()
    {
        Debug.Log("used");
        checkImage.enabled = true;
        upgradeButton.enabled = false;
    }
}
