using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleActiveTool : MonoBehaviour
{
    public Toggle planterToggle;
    public Toggle wateringCanToggle;
    public Interactor interactor;
    public TextMeshProUGUI activeToolLabel;

    void Start()
    {
        ToggleTool(interactor.activeToolType, interactor.seed.GetComponent<Plant>());
    }

    public void ToggleTool(ToolType toolType, Plant plant = null)
    {
        switch (toolType)
        {
            case ToolType.Planter:
                planterToggle.isOn = true;
                wateringCanToggle.isOn = false;
                activeToolLabel.text = plant.plantName;
                break;
            case ToolType.WaterBucket:
                planterToggle.isOn = false;
                wateringCanToggle.isOn = true;
                activeToolLabel.text = "Watering Can";
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
