using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject gunPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 3f;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, gunPoint.transform.position, transform.rotation);

        Vector2 bulletVelocity = gunPoint.transform.right * bulletSpeed;

        if (bulletVelocity.x > 0 && player.rd2d.velocity.x > 0 || bulletVelocity.x < 0 && player.rd2d.velocity.x < 0)
        {
            bulletVelocity.x += player.rd2d.velocity.x;
        }
        if (bulletVelocity.y > 0 && player.rd2d.velocity.y > 0 || bulletVelocity.y < 0 && player.rd2d.velocity.y < 0)
        {
            bulletVelocity.y += player.rd2d.velocity.y;
        }

        bullet.GetComponent<Rigidbody2D>().AddForce(bulletVelocity, ForceMode2D.Impulse);
    }
}
