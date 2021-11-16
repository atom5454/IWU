using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    private Rigidbody2D rd2d;
    private SpriteRenderer sprite;
    private SpawnPickups spawnPickups;
    private PlayerMovement playerMovement;

    private int score = 0;
    public float delayDamage = 0.0f;

    //after how many seconds the player will receive damage after the last damage
    public float defaultDelayDamage = 0.5f;
    private float health = 100;

    public Text countText;
    public Text winText;
    public Text healthText;

    private void Awake()
    {
        rd2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        spawnPickups = GetComponent<SpawnPickups>();
        playerMovement = GetComponent<PlayerMovement>();
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

        if(delayDamage > 0)
        {
            delayDamage -= Time.deltaTime;
        }
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    private void FixedUpdate()
    {
        playerMovement.Move(rd2d);
    }

    //method is called if ANY objects touching
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (delayDamage <= 0)
        {
            var damageComponent = collision.gameObject.GetComponent<DamageComponent>();
            if (damageComponent != null)
            {
                health -= damageComponent.blockDamage;
                delayDamage = defaultDelayDamage;
                UpdateHealth();
            }
        }

        if (health <= 0)
        {
            gameObject.SetActive(false);
            UpdatePlayerText();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUps"))
        {
            Destroy(other.gameObject);
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
        if (score == 5)
        {
            UpdateSpeed();
        }
        if (score == 8)
        {
            UpdateMass();
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
        spawnPickups.SpawnDefaultPickups(targetHuman);
        score++;
        UpdateCountText();
    }

    private void UpdateSpeed()
    {
        var isTake = spawnPickups.SpawnGiveSpeedPickups();
        if (isTake)
        {
            playerMovement.speed += 0.1f;
        }
    }

    private void UpdateMass()
    {
        var isTake = spawnPickups.SpawnGiveMassPickups();
        if (isTake)
        {
            rd2d.mass += 0.1f;
        }
    }
}