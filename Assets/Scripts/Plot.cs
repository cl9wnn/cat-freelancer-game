using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Unity.Mathematics;

public class Plot : MonoBehaviour

{
   public Game gmscript;
   public bool[] isEventDone;
    public bool isStart;
    public bool isEnd;
    public GameObject startImg;
    public GameObject endImg;
    public Text endText;
    public Text[] FurtherText;
    public Text[] eventText;
    public Text[] rewardText;
    public Text[] ScoreTextEvent;
    public string[] plusminusString;
    public Text startGameText;
    public Text languageText;
    public GameObject[] eventImg;
    public float[] scoreCoefficent;
    private string fileName;
    public int total;
    public float moneyReward;
    public AudioSource GetMoney;
    public AudioSource ThrowEvent;
    private float delay = 6;
    public GameObject fingerImg;
    public Settingss settingss;
    public int Total
    {
        get => total;
        set
        {

            total = value;

            for (int i = 0, clicks = 600; i < isEventDone.Length; i++, clicks +=2500)
            {
                if (total >= clicks && isEventDone[i] == false) PlotEvent(i);
            }
        }
    }



    public void  Awake () 
    {
      fileName = "Plots.BIN";
        if (File.Exists(Application.persistentDataPath + "/Saves/" + fileName))
        {
            SavedData data = MyLoad.LoadFileBinary<SavedData>(fileName);
            isStart = data.isStart;
            isEnd = data.isEnd; 
            for (int i = 0; i < isEventDone.Length; i++)
            {
                isEventDone[i] = data.isEventDone[i];
            }
            Total = data.total;
        }
    }

   void Start () 
    {
        StartGameEvent();
    }
   void Update ()
    {
    }
    public void ChangeLanguage()
    {
        for (int i = 0; i < FurtherText.Length; i++)
        {
            FurtherText[i].text = LanguageSystem.lng.ok[i];  //обучение - training
        }
        startGameText.text = LanguageSystem.lng.ok[8]; // начать игру 
        languageText.text = LanguageSystem.lng.ok[9]; // поменять язык

        for (int i = 0; i < eventText.Length; i++)
        {
            eventText[i].text = LanguageSystem.lng.events[i];
        }
        endText.text = LanguageSystem.lng.events[5];

    }
    void StartGameEvent()
    {
        if (isStart == false)
        {
            startImg.SetActive(true);   
        }
    }
    public void StartGame()
    {
        settingss.OpenSceneMusic.mute = true;
        settingss.audioSourceMusic.Play();
        startImg.SetActive(false);
        isStart = true;
        Vector2 spawnPosition = new Vector2(-1.8f, 2.05f);
        Instantiate(fingerImg, spawnPosition, Quaternion.identity);
    }

    public void Training()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void PlotEvent(int index)
    {
        ThrowEvent.Play();
        eventImg[index].GetComponent<Animator>().SetTrigger("open");
        eventText[index].text = LanguageSystem.lng.events[index];
        rewardText[index].text = plusminusString[index] + StringMethods.FormatMoney(CounterMoney(index));
        moneyReward = CounterMoney(index);
    }
    public void RewardEvent(int index)
    {
        eventImg[index].GetComponent<Animator>().SetTrigger("close");
        isEventDone[index] = true;
        if (index == 0 || index == 4) gmscript.Score += moneyReward;
        else gmscript.Score -= moneyReward;
        GetMoney.Play();
    }
    public float CounterMoney(int index)
    {
        if (index == 0 || index == 4) return gmscript.ScoreIncrease * scoreCoefficent[index];
        else return gmscript.Score * scoreCoefficent[index];
    }
 
    public void FinalEvent()
    {
        if (!isEnd)
        {
            endImg.SetActive(true);  //финалочка
        }
    }
    public void EndGame() // начать заново с более сложным уровнем
    {
        Application.Quit();
        string path = Application.persistentDataPath + "/Saves";
        Directory.Delete(path, true);

        for (int i = 0; i < 40; i++)
        {
            gmscript.shopItems[i].cost *= 2;
            gmscript.shopItems[i].costMultiplier *= 2;
        }
        isEnd = true;
    }

    public void ResumeGame()
    {
        endImg.SetActive(false);
        isEnd = true;

    }

#if UNITY_ANDROID && !UNITY_EDITOR
   private void OnApplicationPause (bool pause) {
      if (pause) {
         SavedData data = new SavedData (isEventDone, Total, isStart, isEnd);
         Save(data);
      }
      else Awake ();
   }
#else
    private void OnApplicationQuit () {
      SavedData data = new SavedData (isEventDone, Total, isStart, isEnd);
      Save(data);
   }
#endif

private void Save (object Obj) {
        MySave.SaveFileBinary (Obj, fileName);
    }

   [Serializable]
   public class SavedData
    {
        public SavedData(bool[] IsEventDone, int Total, bool IsStart, bool IsEnd) 
        {
            isEventDone = IsEventDone;   
            total = Total;
            isEnd = IsEnd;
            isStart = IsStart;
        }
      public bool[] isEventDone;
      public int total;
        public bool isStart;
        public bool isEnd;
   }

}