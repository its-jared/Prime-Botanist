using Unity.VisualScripting;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public string plantName;
    public Sprite plantIcon;
    public GameObject plantPrefab;
    public Transform design;
    public Color meshColor = Color.green;
    // public GameObject healthBar;

    [Header("Plant Health")]
    public int plantHealth = 10; // 0 - 20; where 0 is dead and 20 is healthy

    [Header("Plant Prefrences")]
    public bool canGoInWater;
    public bool canGoOnLand;

    [Header("Spread Properties")]
    public bool spread;
    public int allowedIterations;
    public int spreadDistance; 
    public float spreadSpeed;
    public float spreadTime;
    public float randomTimeOffset;

    [Header("Variations")]
    public bool randomYAxisRotation;
    public bool randomHeightVariation;
    public bool randomOffset;

    [Header("Growth")]
    public int numberOfGrowthStages;
    public float growthTime;

    [Header("Water")]
    public int water; // 0 - 20, 20 is fully hydrated.
    public float waterRegenSpeed;
    public float waterRegenTime;

    [Space]
    public int happiness; // 0 - 20, 20 is super happy.
    public float age;

    private float iteration;
    private float randomSpreadOffset;
    private Vector3 pos;
    private bool isGrowing;
    private int growthStage;

    void Awake()
    {
        // Set up the iterations.
        iteration = spreadTime;

        // Make the plant grow.
        // Growing is postponed. 
        // isGrowing = true;

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

        randomSpreadOffset = Random.Range(-randomTimeOffset, randomTimeOffset);
    }

    void FixedUpdate()
    {
        // If the plant hasn't grown fully yet, 
        // make the plant grow until its ready. 
        if (isGrowing)
        {
            grow();
            return;
        }

        // Water's relationship with the plants health. 
        // More water gives the plant the ability to heal.
        // Less water will cause the plant to take damage.
        if (waterRegenTime >= (Time.deltaTime * waterRegenSpeed)) 
        {
            if (water >= 15) Heal(1);
            if (water <= 5) TakeDamage(1);
        }

        // Update the plants happyness,
        // this is based on their health and water. 
        happiness = (plantHealth + water) / 2;

        // If the plant is unable to spread, or global spreading is stopped,
        // don't waste resources calculating the spread timer.
        if (!spread || !World.instance.plantSpreading) return;
        // Stop when no more interations avalible.
        if (allowedIterations <= 0) return;

        iteration -= (spreadSpeed + randomSpreadOffset) * Time.deltaTime;

        if (iteration <= 0 && !(plantHealth < 20))
        {
            iteration = spreadTime;
            allowedIterations--;

            // Spread.
            if (allowedIterations >= 0 || happiness >= 10)
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
        Debug.Log($"You clicked me ({plantName}).");

        switch (World.instance.interactor.activeToolType)
        {
            case ToolType.WaterBucket:
                Water(2);
                break;
            case ToolType.Dropper:
                World.instance.interactor.ChangeSeed(gameObject);
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        plantHealth -= amount;

        if (plantHealth <= 0)
            die();
    }

    public void Heal(int amount)
    {
        plantHealth += amount;

        if (plantHealth >= 20)
            plantHealth = 20;
    }

    public void Water(int amount)
    {
        water += amount;

        if (water >= 20)
            water = 20;
    }

    private void die()
    {
        World.instance.plantsToKill.Enqueue(this.gameObject);
    }

    private void grow()
    {
        if (growthStage < numberOfGrowthStages)
        {
            growthStage++;

            Vector3 localScale = design.transform.localScale;
            design.transform.localScale = new Vector3((localScale.x + growthStage / 0.1f) * growthTime * Time.deltaTime,
                                                      (localScale.y + growthStage / 0.1f) * growthTime * Time.deltaTime,
                                                      (localScale.z + growthStage / 0.1f) * growthTime * Time.deltaTime);
        }
        else
            isGrowing = false;
    }
}
