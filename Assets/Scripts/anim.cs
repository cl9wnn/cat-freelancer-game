using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class anim : MonoBehaviour
{

    public GameObject[] panels;
    public Animator[] animators;
    public AudioSource OnMouseClick;

    private bool[] isOpen;

    void Start()
    {
        isOpen = new bool[panels.Length];
    }

    public void OpenPanel(int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= panels.Length)
        {
            return;
        }

        for (int i = 0; i < panels.Length; i++)
        {
            isOpen[i] = (i == panelIndex) ? !isOpen[i] : false;
            animators[i].SetBool("open", isOpen[i]);
        }
        OnMouseClick.Play();
    }

    public void CloseAll()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            isOpen[i] = false;
            animators[i].SetBool("open", false);
        }
        OnMouseClick.Play();

    }
}