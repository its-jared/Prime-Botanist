using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour, ISaveSystem
{
    public AudioClip[] songs;

    private AudioSource ads;

    void Awake()
    {
        // Get and set the audio source. 
        ads = GetComponent<AudioSource>();
       // ads.volume = gameSettings.gameSettings.musicVolume;
    }

    void Update()
    {
        // Check if a song is currently playing.
        // If a song isn't currently playing we want to start another one. 
        if (!ads.isPlaying)
            ads.PlayOneShot(songs[Random.Range(0, songs.Length)]);
    }

    public void LoadSettings(GameSettings settings)
    {
        ads.volume = settings.musicVolume;
    }

    public void SaveSettings(ref GameSettings settings)
    {
        settings.musicVolume = ads.volume;
    }
}
