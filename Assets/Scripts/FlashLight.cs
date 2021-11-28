using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    private float timeLife;

    private Rigidbody2D rd2d;

    private void Awake()
    {
        rd2d = GetComponent<Rigidbody2D>();

        timeLife = Random.Range(5, 15);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLife > 0)
        {
            timeLife -= Time.deltaTime;
            if (timeLife < 0)
            {
                timeLife = Mathf.Clamp(timeLife, 0, 1);
            }
        }
        if (timeLife == 0)
        {
            Destroy(gameObject);
        }
    }
}
