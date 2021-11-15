using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    private Rigidbody2D rd2d;
    private SpriteRenderer sprite;
    public Joystick joystick;
    public Transform prefab;
    public Transform prefabMass;
    public Transform prefabSpeed;

    private int score = 0;
    private int human = 5;
    private int humanMass = 0;
    private int humanSpeed = 0;

    private bool hasDamage = false;
    public float delayDamage = 0.0f;

    //after how many seconds the player will receive damage after the last damage
    public float defaultDelayDamage = 0.5f;

    public float speed;
    private float health = 100;

    public Text countText;
    public Text winText;
    public Text healthText;

    private void Awake()
    {
        rd2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateCountText();
        winText.text = "";
        healthText.text = "Health: " + health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            sprite.color = Color.black;
        }

        if(delayDamage > 0 && hasDamage == false)
        {
            delayDamage -= Time.deltaTime;
            hasDamage = false;
        }
        if (hasDamage && delayDamage == defaultDelayDamage)
        {
            UpdateHealth();
            hasDamage = false;
        }
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Vector2 stop = new Vector2(0, 0);
            rd2d.AddForce(stop);
            rd2d.SetRotation(0);
            rd2d.rotation = 0;
        }
        else
        {
            float moveHorizontal = Input.GetAxis("Horizontal") + joystick.Horizontal;
            float moveVertical = Input.GetAxis("Vertical") + joystick.Vertical;

            if (moveHorizontal != 0 || moveVertical != 0)
            {
                Vector2 movement = new Vector2(moveHorizontal, moveVertical);
                Vector2 forse = movement * speed;
                rd2d.AddForce(forse);
            }
            else
            {
                ChangeSpeed();
            }
        }
    }

    private void ChangeSpeed()
    {
        Vector2 movement = new Vector2(0, 0);
        Vector2 objectVelocity = rd2d.velocity;

        float velocityX = objectVelocity.x;
        float velocityY = objectVelocity.y;


        if (velocityX != 0)
        {
            //UnPerfomance
            //if(velocityX > -0.02f && velocityX < 0.02f)
            //{
            //    rd2d.velocity = new Vector2(0, velocityY);
            //}
            if (velocityX > 0)
            {
                movement.x = -0.4f;
            }
            if (velocityX < 0)
            {
                movement.x = 0.4f;
            }
        }
        if (velocityY != 0)
        {
            //UnPerfomance
            //if (velocityY > -0.02f && velocityY < 0.02f)
            //{
            //    rd2d.velocity = new Vector2(0, velocityX);
            //}
            if (velocityY > 0)
            {
                movement.y = -0.4f;
            }
            if (velocityY < 0)
            {
                movement.y = 0.4f;
            }
        }

        rd2d.AddForce(movement);
    }

    //method is called if ANY objects touching
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (delayDamage <= 0 && hasDamage == false)
        {
            var objectHasDamage = collision.gameObject.GetComponents(typeof(Component)).Any(x => x.name.Contains("Damage"));
            if (objectHasDamage)
            {
                health -= collision.gameObject.GetComponent<BlockDamage>().blockDamage;
                hasDamage = true;
                delayDamage = defaultDelayDamage;
            }
        }

        if (health <= 0)
        {
            Destroy(gameObject);
            UpdatePlayerText();
            UpdateHealth();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("PickUps"))
        {
            HandleHumanPickedUp(other.gameObject);
        }
        else if (other.gameObject.CompareTag("PickMass"))
        {
            Destroy(other.gameObject);
            UpdateMass();
        }
        else if (other.gameObject.CompareTag("PickSpeed"))
        {
            Destroy(other.gameObject);
            UpdateSpeed();
        }
    }

    private void UpdateCountText()
    {
        countText.text = "Score: " + score.ToString();
        if (score >= 8 && humanMass == 0)
        {
            UpdateMass();
        }
        if (score >= 5 && humanSpeed == 0)
        {
            UpdateSpeed();
        }
    }
    private void UpdatePlayerText()
    {
        winText.text = "You lose";
    }

    private void UpdateHealth()
    {
        healthText.text = "Health: " + health.ToString();
    }

    private void HandleHumanPickedUp(GameObject targetHuman)
    {
        human--;
        score += 1;
        Destroy(targetHuman);
        UpdateCountText();
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

    private void UpdateMass()
    {
        if(humanMass > 0)
        {
            humanMass--;
            rd2d.mass += 0.1f;
        }

        if (score >= 8 && humanMass == 0)
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
        }
    }

    private void UpdateSpeed()
    {
        if(humanSpeed > 0)
        {
            humanSpeed--;
            speed += 0.1f;
        }
        if (score >= 5 && humanSpeed == 0)
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
        }
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
        var massPickupBounds = GameObject.FindGameObjectsWithTag("PickSpeed")
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
