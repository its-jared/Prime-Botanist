using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MeshController : MonoBehaviour
{
    public Material meshMaterial;
    public int width, length;
    public bool flat;
    public Color[] groundColors;

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


    void Awake()
    {
        // Set the camera's rotation target to be in the center of the world. 
        cameraRotationTarget.position = new Vector3(width / 2, 0, length / 2);

        // Randomize the seed, if the seed is 0.
        if (seed == 0) seed = Random.Range(0, 99999);

        // Unity components required for mesh rendering.
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Generate mesh geometry.
        mesh.Clear();
        generateVerticies();
        generateTriangles();
        mesh.RecalculateNormals();

        // Add mesh & material to Unity mesh rendering components.
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshRenderer.material = meshMaterial;

        // Set the size and scale of the water quad.
        // We don't want water if the world is flat.
        if (!flat) generateWater();
    }

    void FixedUpdate()
    {
        mesh.colors = colors;
    }

    // TODO: Make nonstatic.
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
                float n = 0f;
                if (!flat) n = Mathf.PerlinNoise((x + seed) * 0.15f, (z + seed) * 0.15f) * 2f;

                if (n <= waterLevel) tiles[x, z] = 1;
                else tiles[x, z] = 0;

                noise[x, z] = n;
                verts[i] = new Vector3(x, n, z);
                uvs[i] = new Vector3(x, z);
                colors[i] = new Color(Mathf.Lerp(groundColors[0].r, groundColors[1].r, n),
                                      Mathf.Lerp(groundColors[0].g, groundColors[1].g, n),
                                      Mathf.Lerp(groundColors[0].b, groundColors[1].b, n));
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
        Vector3 waterPos = new Vector3(x + 0.5f, 0f, z + 0.5f);

        water.localScale = waterScale;
        water.position = waterPos;
    }
}
