using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Image _energyBar = default;
    [SerializeField] private Text _creditText = default;
    public float Speed { get; set; } = 5;     // Player movement speed
    public int Credits { get; private set; } = 100;
    public float Energy { get; private set; }
    private float _startEnergy = 100;

    private void Awake()
    {
        _creditText.text = Credits.ToString();
        Energy = _startEnergy;
    }

    public void UpdateCredits(int amount, bool pay = false)
    {
        if (pay)
        {
            if (Credits - amount > 0)
                Credits -= amount;
        }
        else
            Credits++;
        _creditText.text = Credits.ToString();
    }

    public void UpdateEnergy(float amount)
    {
        Energy += amount;
        _energyBar.fillAmount = Energy / _startEnergy;
    }
}
