using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Training : MonoBehaviour
{
    public Image[] images;
    int index = 0;
    public Text cloudText;
    public GameObject LeaveFromTrainingBttn;
    public Text leftToMenu;

    public void Start()
    {
        cloudText.text = TrainingLang.lng.training[0];
        leftToMenu.text = TrainingLang.lng.training[11];
    }
    public void Next()
    {
        if (index < 8)
        {
            images[index].color = new Color(55f / 255f, 55f / 255f, 55f / 255f, 1f);
            images[index + 1].color = Color.white;
            cloudText.text = TrainingLang.lng.training[index + 1];
            index++;
        }
        else if (index == 8)
        {
            cloudText.text = TrainingLang.lng.training[10];
            LeaveFromTrainingBttn.SetActive(true);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}