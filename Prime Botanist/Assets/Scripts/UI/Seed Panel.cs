using UnityEngine;
using UnityEngine.UI;

public class SeedPanel : MonoBehaviour
{
    public Plant[] plants;
    public GameObject buttonPrefab;
    public Transform contentTransform;
    public Interactor placeSeed;
    public Image activePlant;

    void Awake()
    {
        for (int i = 0; i < plants.Length; i++)
        {
            GameObject go = Instantiate(buttonPrefab, contentTransform);
            SeedButton p = go.GetComponent<SeedButton>(); 
            Plant pl = plants[i];

            p.plant = pl;
            p.placeSeed = placeSeed;
            p.activePlant = activePlant;
            p.labelText = pl.plantName;
            p.spriteImage = pl.plantIcon;
            p.Set();
        }
    }
}
