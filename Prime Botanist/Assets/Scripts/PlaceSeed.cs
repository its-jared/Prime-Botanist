using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class PlaceSeed : MonoBehaviour
{
    public GameObject seed;
    public GameObject terrain;
    public GameObject camera;
    public Transform highlight;

    public LayerMask terrainLayer;
    public LayerMask waterLayer;

    private InputAction placeAction;
    private InputAction breakAction;
    private HeightGen heightGen;

    void Awake()
    {
        placeAction = InputSystem.actions.FindAction("Place");
        breakAction = InputSystem.actions.FindAction("Break");

        heightGen = terrain.GetComponent<HeightGen>();
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
                                             heightGen.GetNoise(new Vector2(hit.point.x, hit.point.z)) + 0.5f,
                                             Mathf.FloorToInt(hit.point.z));

            if (placeAction.WasCompletedThisFrame())
                Place(highlight.position);
            if (breakAction.WasCompletedThisFrame())
                Break(highlight.position);

            return;
        }
        else
            highlight.gameObject.SetActive(false);  
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
