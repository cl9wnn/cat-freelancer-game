using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using YG;
[CustomEditor(typeof(SaveManager))]
[CanEditMultipleObjects]

public class SaveManagerInspector : Editor
{

    public override void OnInspectorGUI()
    {
        GUIContent content = new GUIContent("Удалить сохранения", "Не удаляй нас хозяин!");
        if (GUILayout.Button(content, GUILayout.Width(200), GUILayout.Height(70)))
        {
            YandexGame.ResetSaveProgress();
            YandexGame.SaveProgress();
        }
    }

}