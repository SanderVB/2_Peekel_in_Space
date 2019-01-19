﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHandler : MonoBehaviour
{
    Vector3 explosionLocation;
    [SerializeField] float destroyTimer = 1f;
    [SerializeField] GameObject laserExplosion;
    bool hit = false;

    private void OnParticleCollision(GameObject other)
    {
        //explosionLocation = TODO;
        hit = true;
        GameObject laserFX = Instantiate(laserExplosion, explosionLocation, Quaternion.identity);
        Destroy(laserFX, destroyTimer);

    }

    private void Update()
    {
        if (hit)
            Destroy(gameObject, destroyTimer);
        else
            Destroy(gameObject, destroyTimer * 5); //in case it never hits
    }
}