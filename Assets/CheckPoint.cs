using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CheckPoint : MonoBehaviour
{
    private CircleCollider2D saveCollider;
    private Light2D light2D;

    private PlayerData playerData;
    private Player currentPlayer;

    public float delaySave = 0.0f;
    public float defaultDelaySave = 4f;
    public bool startDelay;


    private void Awake()
    {
        saveCollider = gameObject.GetComponent<CircleCollider2D>();
        light2D = gameObject.GetComponent<Light2D>();
        saveCollider.radius = light2D.pointLightInnerRadius;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (delaySave > 0 && startDelay)
        {
            delaySave -= Time.deltaTime;
        }
        if (playerData != null)
        {
            UpdateCollliderAndColor();
        }
        if (playerData == null)
        {
            SetDefaultLightAndCollider();
        }
        if (Input.GetKeyDown(KeyCode.R) && currentPlayer != null)
        {
            if(currentPlayer.health <= 0 && !currentPlayer.isActiveAndEnabled)
            {
                //uncomment if you want to use serialization saves
                //playerData = SaveManager.Load();

                currentPlayer.delayDamage = playerData.delayDamage;
                currentPlayer.transform.position = new Vector3(playerData.x, playerData.y, 0);
                currentPlayer.score = playerData.score;
                currentPlayer.health = playerData.health;
                currentPlayer.defaultDelayDamage = playerData.defaultDelayDamage;
                currentPlayer.playerMovement.speed = playerData.speed;
                currentPlayer.rd2d.mass = playerData.mass;
                currentPlayer.hasSecondLife = false;

                currentPlayer.gameObject.SetActive(true);
                startDelay = true;

                //comment this all if player dont lose his life when is respawn
                SetDefaultLightAndCollider();
                currentPlayer = null;
                playerData = null;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (delaySave <= 0 && currentPlayer == null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerData = new PlayerData(collision.GetComponent<Player>());
                currentPlayer = collision.GetComponent<Player>();
                currentPlayer.hasSecondLife = true;
                UpdateCollliderAndColor();
                delaySave = defaultDelaySave;
                startDelay = false;

                //uncomment if you want to use serialization saves
                //SaveManager.Save(playerData);
            }
        }
    }

    //called if the player is saved
    private void UpdateCollliderAndColor()
    {
        light2D.color = Color.green;
        saveCollider.radius = light2D.pointLightInnerRadius;

        float currentIntensity = light2D.intensity;
        if(currentIntensity < 2)
        {
            light2D.intensity += Time.deltaTime;
        }

        float currentInnerRadius = light2D.pointLightInnerRadius;
        if (currentInnerRadius >= 1f)
        {
            light2D.pointLightInnerRadius -= Time.deltaTime;
        }
    }

    //called if the player used his second life
    private void SetDefaultLightAndCollider()
    {
        light2D.color = Color.yellow;
        saveCollider.radius = light2D.pointLightInnerRadius;

        float currentIntensity = light2D.intensity;
        if (currentIntensity > 1)
        {
            light2D.intensity -= Time.deltaTime;
        }

        float currentInnerRadius = light2D.pointLightInnerRadius;
        if (currentInnerRadius <= 2f)
        {
            light2D.pointLightInnerRadius += Time.deltaTime;
        }
    }
}
