using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Image lockImage;
    public Button upgradeButton;
    public Image checkImage;

    public void Awake()
    {
        checkImage.enabled = false;
    }

    public void Activate()
    {
        checkImage.enabled = false;
        lockImage.enabled = false;
        upgradeButton.interactable = true;
    }

    public void Deactivate()
    {
        checkImage.enabled = false;
        lockImage.enabled = true;
        upgradeButton.interactable = false;
    }

    public void Used()
    {
        checkImage.enabled = true;
        lockImage.enabled = false;        
        upgradeButton.interactable = false;
    }
}
