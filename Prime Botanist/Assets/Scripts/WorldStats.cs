using UnityEngine;

public class WorldStats
{
    public static int averagePlantHealth
    {
        get
        {
            int a = 0;
            int i = 0;

            foreach (Transform plant in World.instance.plants.Values)
            {
                a += plant.GetComponent<Plant>().plantHealth;
                i++;
            }

            if (i <= 0 || a <= 0) return 0;
            return (a / i);
        }
    }
    public static float worldAge
    {
        get
        {
            return Time.time / 100f;
        }
    }
}
