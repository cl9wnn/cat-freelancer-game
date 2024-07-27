using System;

[Serializable]
public class SettingsData
{
    public SettingsData(bool IsMuted)
    {
        isMuted = IsMuted;
    }
    public bool isMuted;
}