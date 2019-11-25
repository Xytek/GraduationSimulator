using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Image lockImage;
    private Button upgradeButton;

    public void Awake()
    {
        upgradeButton = GetComponent<Button>();
    }
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
}
