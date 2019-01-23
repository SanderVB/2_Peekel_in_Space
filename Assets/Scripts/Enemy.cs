using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] int health = 3;
    [SerializeField] float destroyTimer = 1f;
    [SerializeField] int enemyValue = 1;
    [SerializeField] GameObject deathEffect;
    [SerializeField] Transform parent;
    bool isDying = false;


    // Use this for initialization
    void Start()
    {

    }

    private void AddNonTriggerBoxCollider()
    {
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        health -= FindObjectOfType<PlayerController>().GetDamageValue();
        if (health <= 0 && !isDying)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        isDying = true;
        GameObject deathFX = Instantiate(deathEffect, transform.position, Quaternion.identity);
        deathFX.transform.parent = parent;
        Destroy(this.gameObject, destroyTimer / 2);
        Destroy(deathFX, destroyTimer);
        FindObjectOfType<GameSession>().AddToScore(enemyValue);
        if (gameObject.tag == "Boss")
            FindObjectOfType<ResultScreenHandler>().HasWon(true);
    }
}