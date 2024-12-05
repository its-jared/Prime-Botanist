using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotation;
    public float rotationSpeed;
    public Animation posAnim;
    public Animation negAnim;

    private InputAction rightAction;
    private InputAction leftAction;

    void Start()
    {
        // Set actions (inputs).
        rightAction = InputSystem.actions.FindAction("MoveCameraRight");
        leftAction = InputSystem.actions.FindAction("MoveCameraLeft");
    }

    void Update()
    {
        if (rightAction.WasPressedThisFrame())
        {
            transform.RotateAround(target.position, Vector3.up, rotation);
        }
        if (leftAction.WasPressedThisFrame())
        {
            transform.RotateAround(target.position, Vector3.up, -rotation);
        }
    }
}
