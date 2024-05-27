using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimationSystem : MonoBehaviour
{
    public GameObject ClearPan;
    public void OpenPanel(Animator animator)
    {
        animator.SetTrigger("Open");

    }

    public void ClosePanel(Animator animator)
    {
        animator.SetTrigger("Close");
    }
}
