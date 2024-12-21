using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MeshController : MonoBehaviour
{
    public Material meshMaterial;
    public int width, length, height;
    public int worldType;
    public WorldType[] worldTypes;

    public int seed;

    [Header("Water")]
    public Transform water;
    public float waterLevel;

    public Transform cameraRotationTarget;

    public Color[] colors;
    public byte[,] tiles;

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;

    private float[,] noise;
    private byte[,,] voxels;

    private WorldType type;
    private Soil soil;

    private int rockCount;

    void Awake()
    {
        // Set the world type
        type = worldTypes[worldType];

        // Set the camera's rotation target to be in the center of the world. 
        cameraRotationTarget.position = new Vector3(width / 2, 0, length / 2);

        // Randomize the seed, if the seed is 0.
        if (seed == 0) seed = Random.Range(0, 99999);

        // Unity components required for mesh rendering.
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Get the soil component.
        soil = GetComponent<Soil>();
        soil.Init();

        // Generate mesh geometry.
        mesh.Clear();
        generateVerticies();
        generateTriangles();
        mesh.RecalculateNormals();

        if (type.spawnObelisk) generateObelisk();

        // Add mesh & material to Unity mesh rendering components.
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshRenderer.material = meshMaterial;

        // Set the size and scale of the water quad.
        // We don't want water if the world is flat.
        if (!type.flat) generateWater();
    }

    void FixedUpdate()
    {
        mesh.colors = colors;
    }

    public float GetNoise(Vector2 pos)
    {
        return noise[(int)pos.x, (int)pos.y];
    }

    private void generateVerticies()
    {
        Vector3[] verts = new Vector3[(width + 1) * (length + 1)];
        Vector2[] uvs = new Vector2[(width + 1) * (length + 1)];
        colors = new Color[(width + 1) * (length + 1)];
        noise = new float[width + 1, length + 1];
        tiles = new byte[width + 1, length + 1];

        for (int i = 0, z = 0; z <= width; z++) 
        {
            for (int x = 0; x <= length; x++)
            {
                Vector2 pos = new Vector2(x, z);
                soil.SetHealthAtPoint(pos, type.defaultSoilHealth);
                soil.SetWaterAtPoint(pos, type.defaultSoilWater);
                soil.SetSunlightAtPoint(pos, type.defaultSoilSunlight);

                float n = 1f;
                if (!type.flat) n = Mathf.PerlinNoise((x + seed) * type.noiseZoom, (z + seed) * type.noiseZoom) * type.noiseScale;

                if (n <= waterLevel)
                {
                    tiles[x, z] = (int)TileTypes.Water;
                    Soil.instance.SetWaterAtPoint(new Vector2(x, z), 1f);
                }
                else if (type.generateRocks && type.rockThreshold >= Random.Range(0f, 1f)) placeRock(new Vector3(x, n, z));
                else tiles[x, z] = (int)TileTypes.Land;

                noise[x, z] = n;
                verts[i] = new Vector3(x, n, z);
                uvs[i] = new Vector3(x, z);
                colors[i] = type.groundGradient.Evaluate(n);
                i++;
            }
        }

        mesh.vertices = verts;
        mesh.uv = uvs;
    }

    private void generateTriangles()
    {
        int[] tris = new int[width * length * 6];
        int v = 0; // Vert count
        int t = 0; // Tris count

        for (int z = 0; z < width; z++)
        {
            for (int x = 0; x < length; x++)
            {
                // Triangle 1
                tris[t] = v;
                tris[t + 1] = v + width + 1;
                tris[t + 2] = v + 1;
                // Triangle 2
                tris[t + 3] = v + 1;
                tris[t + 4] = v + width + 1;
                tris[t + 5] = v + width + 2;

                v++;
                t += 6;
            }
            v++;
        }

        mesh.triangles = tris;
    }

    private void generateWater()
    {
        Vector3 waterScale = new Vector3(width, length, waterLevel + 0.5f);

        float x = width / 2;
        float z = width / 2;
        Vector3 waterPos = new Vector3(x, 0f, z);

        water.localScale = waterScale;
        water.position = waterPos;
    }

    private void placeRock(Vector3 pos)
    {
        rockCount++;
        if (rockCount > type.allowedNumberOfRocks) return;

        tiles[Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.z)] = (int)TileTypes.Rock;
        Soil.instance.SetSunlightAtPoint(pos, 0f);
        Instantiate(type.rockPrefab, pos, Quaternion.identity, transform);
    }

    private void generateObelisk()
    {
        float avgNoiseVal = noise[width / 2, height / 2];
        Vector3 position = new Vector3(width / 2f, avgNoiseVal, length / 2f);

        tiles[Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y)] = (int)TileTypes.Rock;
        Soil.instance.SetHealthAtRange(position, 1, 1);
        Soil.instance.SetSunlightAtRange(position, 1, 2);
        Instantiate(type.obeliskPrefab, position, Quaternion.identity, transform);
    }
}
