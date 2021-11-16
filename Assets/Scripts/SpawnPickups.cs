using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPickups : MonoBehaviour
{
    public Transform prefab;
    public Transform prefabMass;
    public Transform prefabSpeed;

    private int human = 5;
    private int humanMass = 0;
    private int humanSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDefaultPickups(GameObject targetHuman)
    {
        human--;
        if (human == 0)
        {
            var boundsToAvoid = GetBoundsToAvoid();
            human = Random.Range(3, 5);
            var i = 0;
            while (i < human)
            {
                int x = Random.Range(-10, 11);
                int y = Random.Range(-4, 4);
                var newHumanTransform = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
                var newHumanCollider = newHumanTransform.GetComponent<Collider2D>();
                if (boundsToAvoid.Any(newHumanCollider.bounds.Intersects))
                {
                    Destroy(newHumanTransform.gameObject);
                    continue; // skip incrementing the counter and try again
                }
                i++;
            }
        }
    }

    public bool SpawnGiveSpeedPickups()
    {
        bool hasTake = false;
        bool hasGenerate = false;

        if (humanSpeed > 0)
        {
            humanSpeed--;
            hasTake = true;
        }
        if (humanSpeed == 0)
        {
            humanSpeed = Random.Range(1, 1);
            var boundsToAvoid = GetBoundsToAvoid();
            var i = 0;
            while (i < humanSpeed)
            {
                int x = Random.Range(-10, 11);
                int y = Random.Range(-4, 4);
                var newHumanTransform = Instantiate(prefabSpeed, new Vector3(x, y, 0), Quaternion.identity);
                var newHumanBounds = newHumanTransform.GetComponent<Collider2D>().bounds;
                if (boundsToAvoid.Any(newHumanBounds.Intersects))
                {
                    Destroy(newHumanTransform.gameObject);
                    continue; // skip incrementing the counter and try one more time
                }
                i++;
            }
            hasGenerate = false;
        }
        return hasGenerate || hasTake;
    }

    public bool SpawnGiveMassPickups()
    {
        bool hasTake = false;
        bool hasGenerate = false;

        if (humanMass > 0)
        {
            humanMass--;
            hasTake = true;
        }
        if (humanMass == 0)
        {
            var boundsToAvoid = GetBoundsToAvoid();
            humanMass = Random.Range(1, 1);
            var i = 0;
            while (i < humanMass)
            {
                int x = Random.Range(-10, 11);
                int y = Random.Range(-4, 4);
                var newHumanTransform = Instantiate(prefabMass, new Vector3(x, y, 0), Quaternion.identity);
                var newHumanBounds = newHumanTransform.GetComponent<Collider2D>().bounds;
                if (boundsToAvoid.Any(newHumanBounds.Intersects))
                {
                    Destroy(newHumanTransform.gameObject);
                    continue; // skip incrementing the counter and try one more time
                }
                i++;
            }
            hasGenerate = false;
        }
        return hasGenerate || hasTake;
    }

    private Bounds[] GetBoundsToAvoid()
    {
        var blockContainer = GameObject.Find("Block");
        var blockBounds = blockContainer.GetComponentsInChildren<Collider2D>()
            .Select(collider => collider.bounds);
        var pickupBounds = GameObject.FindGameObjectsWithTag("PickUps")
            .Select(pickup => pickup.GetComponent<Collider2D>().bounds);
        var speedPickupBounds = GameObject.FindGameObjectsWithTag("PickSpeed")
            .Select(pickup => pickup.GetComponent<Collider2D>().bounds);
        var massPickupBounds = GameObject.FindGameObjectsWithTag("PickMass")
            .Select(pickup => pickup.GetComponent<Collider2D>().bounds);
        var playerBounds = GetComponent<Collider2D>().bounds;
        playerBounds.Expand(3.0f); // avoid spawning objects right in front of the player
        return blockBounds.Concat(pickupBounds)
            .Concat(massPickupBounds)
            .Concat(speedPickupBounds)
            .Append(playerBounds)
            .ToArray();
    }
}
