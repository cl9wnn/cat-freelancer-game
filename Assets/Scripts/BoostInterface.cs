using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostInterface : MonoBehaviour
{
    public Boost bst;
    public GameObject BstPanel;
    public float minimum = 10.0F;
    public float maximum = 20.0F;
    public GameObject boostTextImg;



    void Start()
    {

    }

    void Update()
    {
        if (bst.BoostOn == true)
        {
            BstPanel.SetActive(true);
            boostTextImg.GetComponent<Image>().enabled = true;

            boostTextImg.transform.localScale = new Vector2(Mathf.PingPong(Time.time, maximum - minimum) + minimum, Mathf.PingPong(Time.time, maximum - minimum) + minimum);
        }
        else
        {
            boostTextImg.GetComponent<Image>().enabled = false;
            BstPanel.SetActive(false);
            

        }

    }
}