using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnim : MonoBehaviour
{
    public Camera Animcamera;
    public GameObject prefab;
    public Game gm;
    public Boost bst;
    public Fortune fort;
    private GameObject res;

    void Start()
    {

    }

    void Update()
    {     

    }

    public void OnClick()
    {
        if (bst.BoostOn == false && fort.doo == false)
        {
            res = Instantiate(prefab, Animcamera.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward, Quaternion.identity);
            res.GetComponentInChildren<Text>().text = "+" + StringMethods.FormatMoney(gm.ScoreIncrease);
            Destroy(res, 2f);
        }
        if (bst.BoostOn == true)
        {
            res = Instantiate(prefab, Animcamera.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward, Quaternion.identity);
            res.GetComponentInChildren<Text>().text = "+" + StringMethods.FormatMoney(gm.ScoreIncrease * 3);
            res.GetComponentInChildren<Text>().color = new Color(255, 215, 0);
            Destroy(res, 2f);
        }
        if (fort.doo == true)
        {
            res = (GameObject)Instantiate(prefab, Animcamera.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward, Quaternion.identity);
            res.GetComponentInChildren<Text>().text = "+" + StringMethods.FormatMoney(gm.ScoreIncrease*3);
            res.GetComponentInChildren<Text>().color = new Color(255, 215, 0);
            Destroy(res, 2f);
        }
    }
}