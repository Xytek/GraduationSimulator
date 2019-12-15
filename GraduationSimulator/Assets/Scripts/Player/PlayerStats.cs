using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Image _energyBar = default;    // UI object for the energy bar
    [SerializeField] private Text _creditText = default;    // Text object for the credit count
    public float Speed { get; set; } = 3f;                  // Player movement speed
    public int Credits { get; private set; } = 0;           // Player credit count
    public float Energy { get; private set; }               // Player energy
    public float TotalTime { get; set; } = 0;               // The total time spent accross semesters
    public bool NewSem { get;  set; }                       // If eligible for a new semester
    private float _startEnergy = 100f;                      // How much energy you start with
    private int _totalCredits = 0;
    private void Awake()
    {        
        _creditText.text = Credits.ToString();
        ResetEnergy();
    }

    public void ResetEnergy()
    {
        Energy = _startEnergy;
    }

    public void UpdateCredits(int amount)
    {
        // Update credit count
        Credits += amount;  

        // increase total-credits in case credits have increased
        if(amount > 0)
        {
            _totalCredits += amount;
        }
   
        // Visualize it in the HUD
        _creditText.text = Credits.ToString();

        // If 30 credits have been collected, start new semester
        if (_totalCredits % 30 == 0 && !NewSem)
        {            
            NewSem = true;
        }
    }

    public void UpdateEnergy(float amount)
    {
        // Update energy by for example getting a coffee
        Energy += amount;
        _energyBar.fillAmount = Energy / _startEnergy;
    }
}
