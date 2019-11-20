using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    public Text txt;
    public GameObject blackboard;
    void Start()
    {        
        EventManager.StartListening("LockedElement", ActivateBlackboard);
    }
    private void OnDestroy()
    {           
        EventManager.StopListening("LockedElement", ActivateBlackboard);
    }

    void ActivateBlackboard(EventParams e)
    {                
        UpdateInfotext(e.text);
        StartCoroutine(Wait5Seconds());        
    }

    IEnumerator Wait5Seconds()
    {
        blackboard.SetActive(true);
        yield return new WaitForSeconds(5);
        blackboard.SetActive(false);
    }

    private void UpdateInfotext(string text)
    {
        txt.text = text;     
    }
}
