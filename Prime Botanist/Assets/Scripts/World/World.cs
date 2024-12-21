using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class World : MonoBehaviour
{
    public static World instance;

    public bool plantSpreading;

    public Queue<GameObject> plantsToKill = new Queue<GameObject>();

    public Transform cameraBody;
    public Camera cameraItself;
    public Interactor interactor;

    public Dictionary<Vector2, Transform> plants = new Dictionary<Vector2, Transform>();

    private MeshController meshCon;

    void Start()
    {
        instance = this;
        meshCon = GetComponent<MeshController>();

        // Set camera pos and what not to match world size.
        cameraBody.position = new Vector3(meshCon.width - 5f, 0f, meshCon.length - 5f);
        cameraItself.orthographicSize = meshCon.width - 5f;
    }

    void Update()
    {
        // Loop and kill all plants with-in the plantsToKill queue.
        if (plantsToKill.TryDequeue(out GameObject go))
            BreakPlant(go.transform.position);
    }

    public void AddPlant(Vector2 pos, Transform t, bool click = true)
    {
        Vector2Int intpos = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
        Plant plant = t.gameObject.GetComponent<Plant>();

        // Attempting to spawn plant outside of the world.
        if (pos.x <= 0 || pos.y <= 0 ||
            pos.x >= meshCon.width || pos.y >= meshCon.length)
        {
            Destroy(t.gameObject);
            return;
        }
        // Check if the plant is on a rock. 
        if (meshCon.tiles[intpos.x, intpos.y] == (int)TileTypes.Rock)
        {
            Destroy(t.gameObject);
            return;
        }
        // Check to see if plants soil requirements are met. 
        if (plant.picky)
        {
            // Soil Health
            if (Soil.instance.GetHealthAtPoint(intpos) >= plant.minimumSoilHealth) return;
            // Soil Water
            if (Soil.instance.GetWaterAtPoint(intpos) >= plant.minimumSoilWater) return;
            // Soil Sunlight
            if (Soil.instance.GetSunlightAtPoint(intpos) >= plant.minimumSunlight) return;
        }

        if (plants.TryAdd(pos, t))
        {
            return;
        }

        Debug.Log($"Unable to add plant at: {pos}.");
        Destroy(t.gameObject);

        // "Click" the plant.
        Transform plantTransform;
        if (click && plants.TryGetValue(pos, out plantTransform)) {
            plant.gameObject.GetComponent<Plant>().Clicked();
        }
    }
    public void AddPlant(Vector3 pos, Transform t, bool click = true)
    {
        AddPlant(new Vector2(pos.x, pos.z), t, click);
    }

    public Transform GetPlant(Vector2 pos)
    {
        if (plants.TryGetValue(pos, out Transform t)) return t;
        Debug.Log($"Unable to get plant at: {pos}.");
        return null;        
    }
    public Transform GetPlant(Vector3 pos)
    {
        return GetPlant(new Vector2(pos.x, pos.z));
    }

    public Plant GetPlantClass(Vector2 pos)
    {
        if (plants.TryGetValue(pos, out Transform t)) return t.GetComponent<Plant>();
        Debug.Log($"Unable to get plant at: {pos}.");
        return null;
    }
    public Plant GetPlantClass(Vector3 pos)
    {
        return GetPlantClass(new Vector2(pos.x, pos.z));
    }

    public void BreakPlant(Vector2 pos)
    {
        if (plants.TryGetValue(pos, out Transform t))
        {
            Destroy(t.gameObject);
            plants.Remove(pos);
            return;
        }
        Debug.Log($"Unable to remove plant at: {pos}.");
    }
    public void BreakPlant(Vector3 pos)
    {
        BreakPlant(new Vector2(pos.x, pos.z));
    }
}
