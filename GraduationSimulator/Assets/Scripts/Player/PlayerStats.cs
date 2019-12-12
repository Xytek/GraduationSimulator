using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Image _energyBar = default;    // UI object for the energy bar
    [SerializeField] private Text _creditText = default;    // Text object for the credit count
    public float Speed { get; set; } = 5f;                  // Player movement speed
    public int Credits { get; private set; } = 100;         // Player credit count
    public float Energy { get; private set; }               // Player energy
    private float _startEnergy = 100f;                      // How much energy you start with

    private void Awake()
    {
        _creditText.text = Credits.ToString();
        Energy = _startEnergy;
    }

    public void UpdateCredits(int amount, bool pay = false)
    {
        // Update credit count
        if (pay)
            Credits -= amount;
        else
            Credits++;
        // Visualize it in the HUD
        _creditText.text = Credits.ToString();
    }

    public void UpdateEnergy(float amount)
    {
        // Update energy by for example getting a coffee
        Energy += amount;
        _energyBar.fillAmount = Energy / _startEnergy;
    }
}
