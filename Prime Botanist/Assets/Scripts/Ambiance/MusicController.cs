using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    public AudioClip[] songs;

    private AudioSource ads;

    void Awake()
    {
        // Get and set the audio source. 
        ads = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if a song is currently playing.
        // If a song isn't currently playing we want to start another one. 
        if (!ads.isPlaying)
            ads.PlayOneShot(songs[Random.Range(0, songs.Length)]);
    }
}
