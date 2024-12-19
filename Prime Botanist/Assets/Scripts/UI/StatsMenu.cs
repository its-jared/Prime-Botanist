using TMPro;
using UnityEngine;

public class StatsMenu : MonoBehaviour
{
    public TextMeshProUGUI averagePlantHealthText;
    public TextMeshProUGUI averagePlantHappynessText;
    public TextMeshProUGUI averagePlantWaterText;
    public TextMeshProUGUI worldAge;

    public void Active()
    {
        averagePlantHealthText.text = $"{WorldStats.averagePlantHealth}";
        averagePlantHappynessText.text = $"{WorldStats.averagePlantHappiness}";
        averagePlantWaterText.text = $"{WorldStats.averagePlantWater}";
        worldAge.text = $"{WorldStats.worldAge}";
    }
}
