using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class Move : MonoBehaviour
{
    private Rigidbody2D rd2d;
    private SpriteRenderer sprite;
    public Joystick joystick;
    public Transform prefab;
    public Transform prefabMass;
    public Transform prefabSpeed;

    private int score;
    public int human;
    public int humanMass;
    public int humanSpeed;

    public float speed;

    public Text countText;
    public Text winText;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        score = 0;
        human = 5;
        humanMass = 0;
        humanSpeed = 0;

        SetCountText();
        winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            sprite.color = Color.black;
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
                ChangeSpeedVar2();
            }
        }
    }

    private void ChangeSpeedVar2()
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
    private void ChangeSpeedVar1()
    {
        var x = rd2d.velocity.x;
        var y = rd2d.velocity.y;
        if (x != 0)
        {
            if (x > 0)
            {
                Vector2 movement = new Vector2(-0.2f, 0);
                rd2d.AddForce(movement);
            }
            if (x < 0)
            {
                Vector2 movement = new Vector2(0.2f, 0);
                rd2d.AddForce(movement);
            }
        }
        if (y != 0)
        {
            if (y > 0)
            {
                Vector2 movement = new Vector2(0, -0.2f);
                rd2d.AddForce(movement);
            }
            if (y < 0)
            {
                Vector2 movement = new Vector2(0, 0.2f);
                rd2d.AddForce(movement);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUps"))
        {
            Destroy(other.gameObject);
            score += 1;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("PickMass"))
        {
            Destroy(other.gameObject);
            SetMass();
        }
        else if (other.gameObject.CompareTag("PickSpeed"))
        {
            Destroy(other.gameObject);
            SetSpeed();
        }
    }

    void SetCountText()
    {
        countText.text = "Score: " + score.ToString();
        //if(count >= 5)
        //{
        //    winText.text = "You win";
        //}
        human--;

        if (human == 0)
        {
            human = Random.Range(3, 5);
            for (int i = 0; i < human; i++)
            {
                int x = Random.Range(-10, 11);
                int y = Random.Range(-4, 4);
                Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
        if (score >= 8 && humanMass == 0)
        {
            SetMass();
        }
        if (score >= 5 && humanSpeed == 0)
        {
            SetSpeed();
        }
    }

    void SetMass()
    {
        if(humanMass > 0)
        {
            humanMass--;
            rd2d.mass += 0.1f;
        }

        if (score >= 8)
        {
            if (humanMass == 0)
            {
                humanMass = Random.Range(1, 1);
                for (int i = 0; i < humanMass; i++)
                {
                    int x = Random.Range(-10, 11);
                    int y = Random.Range(-4, 4);
                    Instantiate(prefabMass, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }
    void SetSpeed()
    {
        if(humanSpeed > 0)
        {
            humanSpeed--;
            speed += 0.1f;
        }
        if (score >= 5)
        {
            if (humanSpeed == 0)
            {
                humanSpeed = Random.Range(1, 1);
                for (int i = 0; i < humanSpeed; i++)
                {
                    int x = Random.Range(-10, 11);
                    int y = Random.Range(-4, 4);
                    Instantiate(prefabSpeed, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }
}
