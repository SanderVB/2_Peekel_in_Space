using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] GameObject deathEffect;
    [SerializeField] Transform parent;
    [SerializeField] float destroyTimer = 1f;
    [SerializeField] int enemyValue = 1;
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
        if (!isDying)
        {
            isDying = true;
            GameObject deathFX = Instantiate(deathEffect, transform.position, Quaternion.identity);
            deathFX.transform.parent = parent;
            Destroy(gameObject, destroyTimer/2);
            Destroy(deathFX, destroyTimer);
            FindObjectOfType<GameSession>().AddToScore(enemyValue);
        }
    }
}
