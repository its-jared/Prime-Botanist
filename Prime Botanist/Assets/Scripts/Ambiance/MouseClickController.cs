using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class MouseClickController : MonoBehaviour, ISaveSystem
{
    public AudioClip mouseClick;
    public AudioSource ads;

    public InputAction leftMouseButton;
    public InputAction rightMouseButton;

    void Awake()
    {
        ads = GetComponent<AudioSource>();

        leftMouseButton = InputSystem.actions.FindAction("Place");
        rightMouseButton = InputSystem.actions.FindAction("Break");
    }

    void Update()
    {
        // Listen for when the mouse is clicked to play
        // the mouse click sound.
        MouseClickCheck();
    }

    public void LoadSettings(GameSettings settings)
    {
        ads.volume = settings.clickVolume;
    }

    public void SaveSettings(ref GameSettings settings)
    {
        settings.clickVolume = ads.volume;
    }

    private void MouseClickCheck()
    {
        if (leftMouseButton.WasPressedThisFrame() ||
            rightMouseButton.WasPressedThisFrame())
            ads.PlayOneShot(mouseClick);
    }
}
