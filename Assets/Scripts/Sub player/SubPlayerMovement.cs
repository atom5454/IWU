using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPlayerMovement : MonoBehaviour
{
    //public Joystick joystick;
    public float speed = 5f;
    public float braking = 0.4f;

    public void Move(Rigidbody2D rd2d)
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // + joystick.Horizontal;
        float moveVertical = Input.GetAxis("Vertical"); // + joystick.Vertical;

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
