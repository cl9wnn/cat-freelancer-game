using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class WebTraining : MonoBehaviour
{
    public Image[] images;
    public Image shopImage;
    public Sprite[] shopSprites;
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
        UpdateImageColor(index, new Color(55f / 255f, 55f / 255f, 55f / 255f, 1f));

        if (index <= 3 || index == 8)
        {
            SetShopImageColor(new Color(55f / 255f, 55f / 255f, 55f / 255f, 1f));
            UpdateNextStep();
        }
        else if (index >= 4 && index < 8)
        {
            SetShopImageColor(Color.white);
            shopImage.sprite = shopSprites[index - 4];
            UpdateNextStep();
        }
        else if (index == 9)
        {
            SetShopImageColor(new Color(55f / 255f, 55f / 255f, 55f / 255f, 1f));
            cloudText.text = TrainingLang.lng.training[10];
            LeaveFromTrainingBttn.SetActive(true);
        }
    }

    private void UpdateImageColor(int imageIndex, Color color)
    {
        images[imageIndex].color = color;
    }

    private void SetShopImageColor(Color color)
    {
        shopImage.color = color;
    }

    private void UpdateNextStep()
    {
        UpdateImageColor(index + 1, Color.white);
        cloudText.text = TrainingLang.lng.training[index + 1];
        index++;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("PC Level");
    }
}