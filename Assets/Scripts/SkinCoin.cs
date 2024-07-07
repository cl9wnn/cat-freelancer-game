using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using YG;


public class SkinCoin : MonoBehaviour
{
    public Game gmscript;
    public Image PcBttnSprite;
    public Image[] SprtBttn;
    public int bBttn = 0;
    public int Bbttn
    {
        get => bBttn;
        set
        {
            bBttn = value;
            if (bBttn == 1 && !achieve.isAchievementDone[4])
            {
                achieve.resultTexts[4].text = "";
                achieve.CompleteAchievement(4);
            }

        }
    }

    [Header("Магазин скинов")]
    public long[] skinCosts;
    public Text[] SkinTextDop;
    public Sprite[] SpritePC;
    public Sprite[] SprtBoughtBttn;
    public Button[] skinCoin;
    public Image newImg3;
    public Text offlineTimeText;
    public Text offlineTimeInfoText;
    public Text quitText;
    public Text infoText;
    public Achievements achieve;
    public GameObject infoPan;
    public Image infoBttn;
    public Image pcExample;
    public Sprite[] infoBttnSprite;
    public int indexx;
    public Image infoImg;
    public Text hintText;

    public AudioSource BuyPc;


    private void Awake()
    {
        if (YandexGame.SDKEnabled)
            Load();
    }
    private void OnEnable()
    {
        SaveManager.OnSaveEvent += Save;
        SaveManager.OnLoadEvent += Load;
    }
    private void OnDisable()
    {
        SaveManager.OnSaveEvent -= Save;
        SaveManager.OnLoadEvent -= Load;
    }

    private void Save()
    {
        YandexGame.savesData.skinCoinData = new SkinCoinData(Bbttn);
    }
    private void Load()
    {
        var data = YandexGame.savesData.skinCoinData;

        if (data == null) return;

        Bbttn = data.bBttn;
    }

    private void Start()
    {
        infoPan.SetActive(false);
        UpdateSprite();
    }

    private void Update()
    {
        InteractableBttn();
    }
    public void ChangeLanguage()
    {
        for (int i = 0; i < Bbttn; i++)
        {
            SkinTextDop[i].text = LanguageSystem.lng.skinTextDop[6];
        }
        for (int i = Bbttn; i < SkinTextDop.Length; i++)
        {
            SkinTextDop[i].text = LanguageSystem.lng.skinTextDop[i];
        }

        offlineTimeText.text = LanguageSystem.lng.revenueper[2] + (gmscript.offlineTime / 3600) + LanguageSystem.lng.time[0];
        offlineTimeInfoText.text = (gmscript.offlineTime / 3600) + LanguageSystem.lng.time[0];
        quitText.text = LanguageSystem.lng.revenueper[3];
        infoText.text = LanguageSystem.lng.revenueper[4];
        hintText.text = LanguageSystem.lng.revenueper[5];

    }

    public void UpdateSprite()
    {
        for (int i = 0; i < Bbttn; i++)
        {
            pcExample.sprite = SpritePC[i];
            PcBttnSprite.sprite = SpritePC[i];
            SprtBttn[i].sprite = SprtBoughtBttn[i];
            SkinTextDop[i].fontSize = 38;
            SkinTextDop[i].color = new Color(1, 1, 1);
            SkinTextDop[i].text = LanguageSystem.lng.skinTextDop[6];
            offlineTimeText.text = LanguageSystem.lng.revenueper[2] + (gmscript.offlineTime / 3600) + LanguageSystem.lng.time[0];
            offlineTimeInfoText.text = (gmscript.offlineTime / 3600) + LanguageSystem.lng.time[0];
        }
        newImg3.enabled = false;
    }

    public void BuyBttnsprite(int index)
    {
        if (gmscript.Score >= skinCosts[index])
        {
            Bbttn = index + 1;
            gmscript.Score -= skinCosts[index];
            gmscript.offlineTime += 3600;
        }
        BuyPc.Play();
    }
    void InteractableBttn()
    {
        for (int i = 0; i < skinCosts.Length; i++)
        {
            if (Bbttn == i && gmscript.shopItems[23 + i * 3].levelOfItem > 0)
            {
                SkinTextDop[i].text = LanguageSystem.lng.skinTextDop[7];
                SkinTextDop[i].fontSize = 36;
                SkinTextDop[i].color = new Color(1, 243 / 255f, 0);

                if (gmscript.Score >= skinCosts[i])
                {
                    skinCoin[i].interactable = true;
                    newImg3.enabled = true;
                }
                else
                {
                    skinCoin[i].interactable = false;
                    newImg3.enabled = false;
                }
            }
            else
            {
                skinCoin[i].interactable = false;
            }
        }
    }

    public void OpenInfoPan()
        {

        if (indexx == 0)
        {
            infoPan.SetActive(true);
            infoImg.enabled = false;
            infoBttn.sprite = infoBttnSprite[0];
            offlineTimeText.enabled = false;
            quitText.enabled = true;
            indexx = 1;
        }
        else if (indexx == 1)
        {
            infoPan.SetActive(false);
            infoImg.enabled = true;
            infoBttn.sprite = infoBttnSprite[1];
            offlineTimeText.enabled = true;
            quitText.enabled = false;
            indexx = 0;
        }
    }
}