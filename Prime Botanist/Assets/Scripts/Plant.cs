using UnityEngine;

public class Plant : MonoBehaviour
{
    public string plantName;
    public GameObject plantPrefab;
    public Transform design;
    public Color meshColor = Color.green;

    [Header("Plant Health")]
    public int plantHealth = 20; // 0 - 20; where 0 is dead and 20 is healthy

    [Header("Plant Prefrences")]
    public bool canGoInWater;
    public bool canGoOnLand;

    [Header("Spread Properties")]
    public bool spread;
    public int allowedIterations;
    public int spreadDistance; 
    public float spreadSpeed;
    public float spreadTime;

    [Header("Variations")]
    public bool randomYAxisRotation;
    public bool randomHeightVariation;
    public bool randomOffset;

    private float iteration;
    private Vector3 pos;

    void Awake()
    {
        // Set up the iterations.
        iteration = spreadTime;

        // Variations
        if (randomYAxisRotation) transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
        if (randomHeightVariation) design.position = new Vector3(design.position.x, Random.Range(-0.5f, 0f), design.position.z);
        if (randomOffset) design.position = new Vector3(Random.Range(0, 0.5f), design.position.y, Random.Range(0, 0.5f));

        // Fixes odd bug where plants' design mesh sticks around the orgin when spawned through spreading.
        // This bug doesn't occur when placing the plants down regularly though. 
        // TODO: Implement a better solution.
        design.position = new Vector3(transform.position.x - design.position.x,
                                      design.position.y,
                                      transform.position.z - design.position.z);
    }

    void FixedUpdate()
    {
        // If the plant is unable to spread, or global spreading is stopped,
        // don't waste resources calculating the spread timer.
        if (!spread || !World.instance.plantSpreading) return;
        // Stop when no more interations avalible.
        if (allowedIterations <= 0) return;

        iteration -= spreadSpeed * Time.deltaTime;

        if (iteration <= 0)
        {
            iteration = spreadTime;
            allowedIterations--;

            // Spread.
            if (allowedIterations >= 0)
            {
                Vector3 direction = Vector3.zero;
                pos = transform.position;

                switch (Random.Range(0, 3))
                {
                    case 0:
                        direction = new Vector3(spreadDistance, 0, 0);
                        break;
                    case 1:
                        direction = new Vector3(-spreadDistance, 0, 0);
                        break;
                    case 2:
                        direction = new Vector3(0, 0, spreadDistance);
                        break;
                    case 3:
                        direction = new Vector3(0, 0, -spreadDistance);
                        break;
                }

                GameObject newPlant = Instantiate(plantPrefab);
                newPlant.name = $"Plant; Iter.: {allowedIterations}";
                newPlant.transform.position = pos + direction;
                World.instance.AddPlant(pos + direction, newPlant.transform, false);
            }
        }
    }

    public void Clicked()
    {
        TakeDamage(20);
    }

    public void TakeDamage(int amount)
    {
        plantHealth -= amount;

        if (plantHealth <= 0)
        {
            die();
            return;
        }
    }

    private void die()
    {
        World.instance.plantsToKill.Enqueue(this.gameObject);
    }
}
