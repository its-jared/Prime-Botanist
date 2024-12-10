using TMPro;
using UnityEngine;

public class StatsMenu : MonoBehaviour
{
    public TextMeshProUGUI averagePlantHealthText;
    public TextMeshProUGUI worldAge;

    public void Active()
    {
        averagePlantHealthText.text = $"{WorldStats.averagePlantHealth} hp";
        worldAge.text = $"{Mathf.Floor(WorldStats.worldAge)} yr(s)";
    }
}
