using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float speed;
    public float factor;

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(factor * speed, 0f, 0f));  
    }
}
