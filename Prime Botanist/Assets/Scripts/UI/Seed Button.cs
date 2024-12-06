using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedButton : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Image sprite;
    public Interactor placeSeed;
    public Image activePlant;

    [Header("Info")]
    public string labelText;
    public Sprite spriteImage;
    public Plant plant;

    public void Set()
    {
        label.text = plant.plantName;
        sprite.sprite = plant.plantIcon;
    }

    public void Click()
    {
        placeSeed.ChangeSeed(plant.plantPrefab);
        activePlant.sprite = spriteImage;
    }
}
