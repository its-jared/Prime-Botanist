using System;

[Serializable]
public class GameSettings
{
    public float musicVolume;
    public float clickVolume;

    // Use default values if it's a new game.
    public GameSettings()
    {
        musicVolume = 0.25f;
        clickVolume = 0.5f;
    }
}
