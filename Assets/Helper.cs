using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    [ContextMenu("FFF")]
    public void F()
    {
        var t = FindObjectsByType<AudioSource>(FindObjectsSortMode.InstanceID).Length;
        Debug.Log(t);   
    }
}
