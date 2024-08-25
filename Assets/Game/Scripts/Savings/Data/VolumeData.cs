using System;

[Serializable]
public class VolumeData
{
    public float maserVolume = 1.0f;
    public bool isMuteBG;

    public VolumeData(float masterVolume, bool isMuteBG)
    {
        maserVolume = masterVolume;
        this.isMuteBG = isMuteBG;
    }
}