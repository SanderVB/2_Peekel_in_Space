using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] int health = 1;
    [SerializeField] float destroyTimer = 1f;
    [SerializeField] int enemyValue = 1;
    [SerializeField] GameObject deathEffect;
    [SerializeField] Transform parent;
    bool isDying = false;

    // Use this for initialization
    void Start ()
    {
        if(GetComponent<BoxCollider>()==false)
            AddNonTriggerBoxCollider();
	}

    private void AddNonTriggerBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        health -= FindObjectOfType<PlayerController>().GetDamageValue();
        if (health <= 0)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        if (!isDying)
        {
            isDying = true;
            GameObject deathFX = Instantiate(deathEffect, transform.position, Quaternion.identity);
            deathFX.transform.parent = parent;
            Destroy(gameObject, destroyTimer / 2);
            Destroy(deathFX, destroyTimer);
            FindObjectOfType<GameSession>().AddToScore(enemyValue);
        }
    }
}
