using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D rd2d;
    private SpriteRenderer sprite;
    private SpawnPickups spawnPickups;
    public PlayerMovement playerMovement;
    public GameObject prefabSubPlayer;
    private GameObject subPlayer;

    public bool hasSecondLife = false;

    public int score = 0;
    public float delayDamage = 0.0f;

    //after how many seconds the player will receive damage after the last damage
    public float defaultDelayDamage = 0.5f;
    public float health = 100;

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

    //Called if object SetActive(true)
    private void OnEnable()
    {
        UpdateHealth();
        UpdateCountText();
        winText.text = "";
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            KillSelf();
        }

        if (delayDamage > 0)
        {
            delayDamage -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && subPlayer == null)
        {
            Shoot();
        }
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    private void FixedUpdate()
    {
        if(subPlayer == null)
        {
            playerMovement.Move(rd2d);
        }
    }

    //we need this method because if object touch on high speed OnCollisionStay 2D doesn't register the touch
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage(collision);
    }

    //method is called if ANY objects touching
    private void OnCollisionStay2D(Collision2D collision)
    {
        TakeDamage(collision);
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

    private void TakeDamage(Collision2D otherObjectCollision)
    {
        if (delayDamage <= 0)
        {
            var damageComponent = otherObjectCollision.gameObject.GetComponent<DamageComponent>();
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
        if (!hasSecondLife)
        {
            winText.text = "You die";
        }
        else
        {
            winText.text = "You die, if you wanna to use second life press R";
        }
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

    private void KillSelf()
    {
        health = 0;
        gameObject.SetActive(false);
        UpdatePlayerText();
    }

    private void Shoot()
    {
        subPlayer = Instantiate(prefabSubPlayer, transform.position, Quaternion.identity);

        int x = Random.Range(-2, 2);
        int y = Random.Range(-2, 2);

        subPlayer.transform.position += new Vector3(x, y, subPlayer.transform.position.z);
    }

    public GameObject isSubPlayerAlive()
    {
        if(subPlayer == null)
        {
            return null;
        }
        return subPlayer;
    }
}
