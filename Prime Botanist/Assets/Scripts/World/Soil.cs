using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshController))]
public class Soil : MonoBehaviour
{
    public static Soil instance;

    public bool debugMeshColor;

    private float[,] health;
    private float[,] water;
    private float[,] sunlight;

    private MeshController meshCon;
    private bool ready = false;

    public void Init()
    {
        instance = this;
        meshCon = GetComponent<MeshController>();

        health = new float[meshCon.width, meshCon.length];
        water = new float[meshCon.width, meshCon.length];
        sunlight = new float[meshCon.width, meshCon.length];

        ready = true;
    }

    void FixedUpdate()
    {
        // Red: Health.
        // Green: Sunlight. 
        // Blue: Water.
        if (debugMeshColor && ready) setDebugMeshColor();
    }

    // Get Values. //

    public float GetHealthAtPoint(Vector2 pos)
    {
        Vector2Int nPos = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

        if (nPos.x >= meshCon.width || nPos.x < 0 ||
            nPos.y >= meshCon.length || nPos.y < 0) return 0f;

        return health[nPos.x, nPos.y];
    }
    public float GetHealthAtPoint(Vector3 pos)
    {
        return GetHealthAtPoint(new Vector2(pos.x, pos.z));
    }

    public float GetWaterAtPoint(Vector2 pos)
    {
        Vector2Int nPos = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

        if (nPos.x >= meshCon.width || nPos.x < 0 ||
            nPos.y >= meshCon.length || nPos.y < 0) return 0f;

        return water[nPos.x, nPos.y];
    }
    public float GetWaterAtPoint(Vector3 pos)
    {
        return GetWaterAtPoint(new Vector2(pos.x, pos.z));
    }

    public float GetSunlightAtPoint(Vector2 pos)
    {
        Vector2Int nPos = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

        if (nPos.x >= meshCon.width || nPos.x < 0 ||
            nPos.y >= meshCon.length || nPos.y < 0) return 0f;

        return sunlight[nPos.x, nPos.y];
    }
    public float GetSunlightAtPoint(Vector3 pos)
    {
        return GetSunlightAtPoint(new Vector2(pos.x, pos.z));
    }

    // Set Values. //

    public void SetHealthAtPoint(Vector2 pos, float v)
    {
        Vector2Int nPos = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

        if (nPos.x >= meshCon.width || nPos.x < 0 ||
            nPos.y >= meshCon.length || nPos.y < 0) return;

        health[nPos.x, nPos.y] = v;
    }
    public void SetHealthAtPoint(Vector3 pos, float v)
    {
        SetHealthAtPoint(new Vector2(pos.x, pos.z), v);
    }

    public void SetWaterAtPoint(Vector2 pos, float v)
    {
        Vector2Int nPos = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

        if (nPos.x >= meshCon.width || nPos.x < 0 ||
            nPos.y >= meshCon.length || nPos.y < 0) return;

        water[nPos.x, nPos.y] = v;
    }
    public void SetWaterAtPoint(Vector3 pos, float v)
    {
        SetWaterAtPoint(new Vector2(pos.x, pos.z), v);
    }

    public void SetSunlightAtPoint(Vector2 pos, float v)
    {
        Vector2Int nPos = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

        if (nPos.x >= meshCon.width || nPos.x < 0 ||
            nPos.y >= meshCon.length || nPos.y < 0) return;

        sunlight[nPos.x, nPos.y] = v;
    }
    public void SetSunlightAtPoint(Vector3 pos, float v)
    {
        SetSunlightAtPoint(new Vector2(pos.x, pos.z), v);
    }

    private void setDebugMeshColor()
    {
        for (int i = 0, z = 0; z <= meshCon.width; z++)
        {
            for (int x = 0; x <= meshCon.length; x++)
            {
                Vector2 pos = new Vector2(x, z);
                float health = GetHealthAtPoint(pos);
                float water = GetWaterAtPoint(pos);
                float sunlight = GetSunlightAtPoint(pos);

                meshCon.colors[i] = new Color(health, sunlight, water);

                i++;
            }
        }
    }
}
