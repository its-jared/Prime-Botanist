using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleActiveTool : MonoBehaviour
{
    public Interactor interactor;
    public TextMeshProUGUI activeToolLabel;
    public Image activePlant;

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
                break;
            case ToolType.WaterBucket:
                activeToolLabel.text = "Watering Can";
                interactor.activeToolType = ToolType.WaterBucket;
                break;
            case ToolType.Dropper:
                activeToolLabel.text = "Dropper";
                interactor.activeToolType = ToolType.Dropper;
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
