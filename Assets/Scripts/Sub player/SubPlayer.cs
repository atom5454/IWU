using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPlayer : MonoBehaviour
{
    private float timeLife;

    private Rigidbody2D rd2d;
    private SubPlayerMovement subPlayerMovement;


    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        subPlayerMovement = GetComponent<SubPlayerMovement>();

        timeLife = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLife > 0)
        {
            timeLife -= Time.deltaTime;
            if(timeLife < 0)
            {
                timeLife = Mathf.Clamp(timeLife, 0, 1);
            }
        }
        if(timeLife == 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        subPlayerMovement.Move(rd2d);
    }
}
