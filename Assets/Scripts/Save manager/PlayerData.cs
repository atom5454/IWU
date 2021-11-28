using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int score;
    public float delayDamage;
    public float defaultDelayDamage;
    public float health;
    public float speed;
    public float mass;

    public float x;
    public float y;

    public PlayerData(Player player)
    {
        score = player.score;
        delayDamage = player.delayDamage;
        health = player.health;
        defaultDelayDamage = player.defaultDelayDamage;
        speed = player.playerMovement.speed;
        mass = player.rd2d.mass;

        x = player.transform.position.x;
        y = player.transform.position.y;
    }
}
