using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class Interactor : MonoBehaviour
{
    public GameObject seed;
    public GameObject terrain;
    public GameObject camera;
    public Transform highlight;

    public LayerMask terrainLayer;
    public LayerMask waterLayer;

    public ToolType activeToolType;
    public ToggleActiveTool tat;

    private InputAction placeAction;
    private InputAction breakAction;
    private MeshController meshCon;

    void Awake()
    {
        placeAction = InputSystem.actions.FindAction("Place");
        breakAction = InputSystem.actions.FindAction("Break");

        meshCon = terrain.GetComponent<MeshController>();
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray terrainRay = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(terrainRay, out hit, Mathf.Infinity, terrainLayer))
        {
            highlight.gameObject.SetActive(true);
            highlight.position = new Vector3(Mathf.FloorToInt(hit.point.x),
                                             meshCon.GetNoise(new Vector2(hit.point.x, hit.point.z)) + 0.5f,
                                             Mathf.FloorToInt(hit.point.z));

            // Check if the left mouse button was clicked.
            // If so, check if we're planting plants or watering them.
            if (placeAction.WasCompletedThisFrame() && activeToolType == ToolType.Planter)
                Place(highlight.position);
            // Check if the right mouse button was clicked.
            else if (breakAction.WasCompletedThisFrame())
                Break(highlight.position);

            return;
        }
        else
            highlight.gameObject.SetActive(false);  
    }

    public void ChangeSeed(GameObject newSeed)
    {
        seed = newSeed;
        tat.ToggleTool(activeToolType, seed.GetComponent<Plant>());
    }

    private void Place(Vector3 pos)
    {
        GameObject instance = Instantiate(seed, terrain.transform);
        instance.transform.position = pos;
        instance.name = "Plant";
        World.instance.AddPlant(pos, instance.transform);
    }

    private void Break(Vector3 pos)
    {
        World.instance.BreakPlant(pos);
    }
}
