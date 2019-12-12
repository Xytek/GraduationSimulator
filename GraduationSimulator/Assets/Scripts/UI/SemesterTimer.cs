using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SemesterTimer : MonoBehaviour
{
    private bool isActive = true;
    private float _currentTime = 0;
    private float _startingTime = 300;

    [SerializeField] private Text _timerText = default;
    public GameObject SemesterOverScreen;

    void Start()
    {
        _currentTime = _startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            _currentTime -= 1 * Time.deltaTime;

            string minutes = ((int)_currentTime / 60).ToString();
            string seconds = ((int)_currentTime % 60).ToString();
            _timerText.text = minutes + ":" + seconds;
        }

        if (_currentTime <= 0)
        {
            _currentTime = 0;
            SemesterOverScreen.SetActive(true);
            Application.Quit();
        }
    }

    public void Deactivate()
    {
        isActive = false;
    }
    public void Activate()
    {
        isActive = true;
    }
}
