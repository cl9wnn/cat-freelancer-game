using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sounds SO", fileName = "Sounds SO")]
public class SoundsSO : ScriptableObject
{
    public List<SoundData> sounds;
}