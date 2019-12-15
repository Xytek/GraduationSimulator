using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PubNubAPI;
using UnityEngine.UI;
using SimpleJSON;
using System.IO;

public class MyClass
{
    public string username;
    public string score;
    public string test;
}

public class Leaderboard : MonoBehaviour
{
    public static PubNub pubnub;

    public Text[] usernames;
    public Text[] semester1times;

    public Button SubmitButton;
    public InputField FieldUsername;    

    public GameObject UsernamePanel;
    public GameObject LeaderboardPanel;    

    void Start()
    {
        LeaderboardPanel.SetActive(false);
        UsernamePanel.SetActive(true);

        Button btn = SubmitButton.GetComponent<Button>();
        btn.onClick.AddListener(SubmitButtonClicked);

        // Use this for initialization
        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.PublishKey = "pub-c-df7f38b1-cd42-4215-8b92-1ee579de3c98";
        pnConfiguration.SubscribeKey = "sub-c-ef1c1676-1c2d-11ea-b20a-3a3f5ed666ca";

        pnConfiguration.LogVerbosity = PNLogVerbosity.BODY;
        pnConfiguration.UUID = Random.Range(0f, 999999f).ToString();

        pubnub = new PubNub(pnConfiguration);        

        MyClass myFireObject = new MyClass();
        myFireObject.test = "new user";
        string fireobject = JsonUtility.ToJson(myFireObject);
        pubnub.Fire()
            .Channel("leaderboard")
            .Message(fireobject)
            .Async((result, status) => {
                if (status.Error)
                {
                    Debug.Log(status.Error);
                    Debug.Log(status.ErrorData.Info);
                }                
            });

        pubnub.SubscribeCallback += (sender, e) => {
            SubscribeEventEventArgs mea = e as SubscribeEventEventArgs;
            if (mea.Status != null)
            {
            }
            if (mea.MessageResult != null)
            {
                Dictionary<string, object> msg = mea.MessageResult.Payload as Dictionary<string, object>;

                string[] strArr = msg["username"] as string[];
                string[] strScores = msg["score"] as string[];               

                // update the text in the UI
                int i = 0;
                foreach (string username in strArr)
                {                                        
                    usernames[i].text = username.ToString();                    
                    i++;                    
                }

                int scorevar = 0;
                foreach (string score in strScores)
                {                    
                    semester1times[scorevar].text = score.ToString();
                    scorevar++;
                }
            }
            if (mea.PresenceEventResult != null)
            {
                Debug.Log("In Example, SusbcribeCallback in presence" + mea.PresenceEventResult.Channel + mea.PresenceEventResult.Occupancy + mea.PresenceEventResult.Event);
            }
        };
        pubnub.Subscribe()
            .Channels(new List<string>() {
                "my_channel2"
            })
            .WithPresence()
            .Execute();
    }

    void SubmitButtonClicked()
    {
        GameSaving save = JsonUtility.FromJson<GameSaving>(File.ReadAllText(Application.persistentDataPath + "/saveload.json"));

        MyClass myObject = new MyClass();
        myObject.username = FieldUsername.text;
        myObject.score = save.semester2.ToString();
        string json = JsonUtility.ToJson(myObject);

        pubnub.Publish()
            .Channel("leaderboard")
            .Message(json)
            .Async((result, status) => {
                if (status.Error)                
                {
                    Debug.Log(status.Error);
                    Debug.Log(status.ErrorData.Info);
                }
            });

        // change to the leaderboard-panel
        LeaderboardPanel.SetActive(true);
        UsernamePanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
