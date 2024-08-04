using System;

[Serializable]
public class VolumeData
{
    public float maserVolume = 1.0f;

    public VolumeData(float masterVolume)
    {
        maserVolume = masterVolume;
    }
}