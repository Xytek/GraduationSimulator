using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SemesterTimer : MonoBehaviour
{
    private float _currentTime = 0;
    private float _startingTime = 300;

    [SerializeField]
    Text timerText;
    public GameObject SemesterOverScreen;

    void Start()
    {
        _currentTime = _startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime -= 1 * Time.deltaTime;

        string minutes = ((int)_currentTime / 60).ToString();
        string seconds = ((int)_currentTime % 60).ToString();
        timerText.text = minutes+":"+seconds;

        if(_currentTime <= 0)
        {
            _currentTime = 0;
            SemesterOverScreen.SetActive(true);
            Application.Quit();
        }
    }
}
