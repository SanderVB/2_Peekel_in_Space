using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    Vector3 explosionLocation;
    [SerializeField] float destroyTimer = 1f;
    [SerializeField] GameObject laserExplosion;
    bool hit = false;

    private void OnParticleCollision(GameObject other)
    {
        //TODO: instantiates at 0,0,0 with terrain-collision;
        if (!hit)
        {
            hit = true;
            var laserFX = Instantiate(laserExplosion, other.transform.position, Quaternion.identity);
            Debug.Log("Col. pos.: " + other.transform.position);
            Destroy(laserFX, destroyTimer);
        }
    }

    private void Update()
    {
        if (hit)
            Destroy(gameObject, destroyTimer);
        else
            Destroy(gameObject, destroyTimer * 5); //in case it never hits
    }
}
