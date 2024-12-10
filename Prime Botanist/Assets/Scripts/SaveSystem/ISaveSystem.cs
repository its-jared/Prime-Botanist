using UnityEngine;

public interface ISaveSystem
{
    void LoadSettings(GameSettings settings);
    void SaveSettings(ref GameSettings settings);
}
