using UnityEngine;
using UnityEngine.UI;

public class SemesterTimer : MonoBehaviour
{
    private bool isActive = true;
    private float _currentTime = 0f;
    private float _startingTime = 300f;
    private int _currentSemester = 1;

    [SerializeField] private Text _timerText = default;
    [SerializeField] private Text _semesterText = default;
    [SerializeField] private GameObject SemesterOverScreen;

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
            _currentTime = 0;
    }

    public float StartTime
    {
        get { return _startingTime; }
    }

    public float CurrentTime
    {
        get { return _currentTime; }
        set { _currentTime = value; }
    }

    public void NewSemester()
    {
        _currentTime = _startingTime;
        _currentSemester++;
        _semesterText.text = _currentSemester.ToString();
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
