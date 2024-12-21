using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class WorldHappinessBar : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void FixedUpdate()
    {
        slider.value = WorldStats.averagePlantHappiness;
    }
}
