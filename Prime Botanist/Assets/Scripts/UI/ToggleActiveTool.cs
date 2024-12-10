using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleActiveTool : MonoBehaviour
{
    public Interactor interactor;
    public TextMeshProUGUI activeToolLabel;
    public Image activePlant;

    public Button planterButton;
    public Button WaterBucketButton;
    public Button DropperButton;

    void Start()
    {
        ToggleTool(interactor.activeToolType, interactor.seed.GetComponent<Plant>());
    }

    public void ToggleTool(ToolType toolType, Plant plant = null)
    {
        switch (toolType)
        {
            case ToolType.Planter:
                activeToolLabel.text = plant.plantName;
                activePlant.sprite = plant.plantIcon;
                interactor.activeToolType = ToolType.Planter;

                planterButton.interactable = false;
                WaterBucketButton.interactable = true;
                DropperButton.interactable = true;
                break;
            case ToolType.WaterBucket:
                activeToolLabel.text = "Watering Can";
                interactor.activeToolType = ToolType.WaterBucket;

                planterButton.interactable = true;
                WaterBucketButton.interactable = false;
                DropperButton.interactable = true;
                break;
            case ToolType.Dropper:
                activeToolLabel.text = "Dropper";
                interactor.activeToolType = ToolType.Dropper;

                planterButton.interactable = true;
                WaterBucketButton.interactable = true;
                DropperButton.interactable = false;
                break;
        }
    }

    // !! Only used by the toggle buttons in the hotbar.
    // This is because unity is annoying and won't allow
    // the above function inside of the editor. 
    public void ToggleTool(int type)
    {
        ToggleTool((ToolType)type, interactor.seed.GetComponent<Plant>());
    }
}
