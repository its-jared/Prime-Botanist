using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class WorldHealthBar : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void FixedUpdate()
    {
        slider.value = WorldStats.averagePlantHealth;
    }
}
