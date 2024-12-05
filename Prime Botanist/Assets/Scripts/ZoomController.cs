using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class ZoomController : MonoBehaviour
{
    public InputAction zoomIn;
    public InputAction zoomOut;

    public float zoomFactor;

    private Camera _camera; 

    void Start()
    {
        _camera = GetComponent<Camera>();
        // Set actions.
        zoomIn = InputSystem.actions.FindAction("Zoom In");
        zoomOut = InputSystem.actions.FindAction("Zoom Out");
    }

    void Update()
    {
        if (zoomIn.WasPressedThisFrame())
            _camera.orthographicSize -= zoomFactor;
        if (zoomOut.WasPressedThisFrame())
            _camera.orthographicSize += zoomFactor;

        // Clamp the zoom.
        // Prevents the player from going too far or through the world.
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 1, 10);
    }
}
