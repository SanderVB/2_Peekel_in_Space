using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHandler : MonoBehaviour
{
    Vector3 explosionLocation;
    [SerializeField] float destroyTimer = 1f;

    [SerializeField] GameObject laserExplosion;
    private void OnParticleCollision(GameObject other)
    {
        //explosionLocation = TODO;
        GameObject laserFX = Instantiate(laserExplosion, explosionLocation, Quaternion.identity);
        Destroy(laserFX, destroyTimer);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("werkt");
            gameObject.SetActive(true);
        }
    }
}
