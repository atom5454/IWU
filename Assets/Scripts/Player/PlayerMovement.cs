using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 5f;
    public float braking = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Rigidbody2D rd2d)
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
            ChangeSpeed(rd2d);
        }
    }

    private void ChangeSpeed(Rigidbody2D rd2d)
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
                movement.x = braking * -1;
            }
            if (velocityX < 0)
            {
                movement.x = braking;
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
                movement.y = braking * -1;
            }
            if (velocityY < 0)
            {
                movement.y = braking;
            }
        }

        rd2d.AddForce(movement);
    }
}
