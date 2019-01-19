using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    [Tooltip("In m/s")] [SerializeField] float moveSpeed = 10f;
    [Header("Range of movement on screen")]
    [SerializeField] float xRange;
    [SerializeField] float yRange;

    [Header("Screen-position based")]
    [Tooltip("Change these if Z-position is changed")] [SerializeField] float positionPitchFactor = -3.5f;
    [Tooltip("Change these if Z-position is changed")] [SerializeField] float positionYawFactor = 5f;

    [Header("Control-throw based")]
    [SerializeField] float controlPitchFactor = -20f;
    [SerializeField] float controlYawFactor = 20f;
    [SerializeField] float controlRollFactor = -30f;

    [Header("Effects")]
    [SerializeField] GameObject deathFX;

    [Header("Weapons")]
    [SerializeField] Transform[] weapons;
    [SerializeField] ParticleSystem weaponEffect;
    [SerializeField] float firingCooldown = 2f;

    float xThrow, yThrow, cooldownTimer;
    bool controlEnabled = true;
    bool firing = false;
    bool onCooldown = false;

    Coroutine firingCoroutine;

    private void Start()
    {
        cooldownTimer = firingCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        //xMovement();
        //yMovement();
        if (controlEnabled)
        {
            Movement();
            RotationFluid();
            Fire();
        }
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1") && !onCooldown)
        {
            onCooldown = true;
            firingCoroutine = StartCoroutine(FireContiniously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }

        if(onCooldown)
        {
            CooldownTimer();
        }
    }

    private void CooldownTimer()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            onCooldown = false;
            cooldownTimer = firingCooldown;
        }
    }

    IEnumerator FireContiniously() //written for 2 weapons, needs rewrite if more/less
    {
        bool hasFired = false;
        while (true)
        {
            if (!hasFired)
            {
                ParticleSystem laser = Instantiate(weaponEffect, weapons[0].position, weapons[0].rotation);
                laser.Emit(1);
                hasFired = true;
                yield return new WaitForSeconds(firingCooldown);
            }
            else
            {
                ParticleSystem laser = Instantiate(weaponEffect, weapons[1].position, weapons[1].rotation);
                laser.Emit(1);
                hasFired = false;
                yield return new WaitForSeconds(firingCooldown);
            }
        }
    }


    /*  private void Fire()
      {
          float cooldownTimer;
           (firing)
          {
              Debug.Log("Pew pew");
              //add weapon firing
              if (Input.GetButtonUp("Fire1"))
              {
                  firing = false;
                  Debug.Log("stop");
              }
          }
      }*/

    /*private void xMovement()
    {
        float xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        float xOffsetThisFrame = speed * xThrow * Time.deltaTime;
        float rawNewXPos = transform.localPosition.x + xOffsetThisFrame;
        transform.localPosition = new Vector3(Mathf.Clamp(rawNewXPos, -xRange, xRange), transform.localPosition.y, transform.localPosition.z);
    }

    private void yMovement()
    {
        float yThrow = CrossPlatformInputManager.GetAxis("Vertical");
        float yOffsetThisFrame = speed * yThrow * Time.deltaTime;
        float rawNewyPos = transform.localPosition.y + yOffsetThisFrame;
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Clamp(rawNewyPos, -yRange, yRange), transform.localPosition.z);
    }*/ //seperate methods for handling horizontal & vertical movement

    private void Movement()
    {
        //set horizantal movement values
        xThrow                  = CrossPlatformInputManager.GetAxis("Horizontal");
        float xOffsetThisFrame  = moveSpeed * xThrow * Time.deltaTime;
        float rawXPos           = transform.localPosition.x + xOffsetThisFrame;
        float xClamped          = Mathf.Clamp(rawXPos, -xRange, xRange);

        //set vertical movement values
        yThrow                  = -CrossPlatformInputManager.GetAxis("Vertical"); //minus because flight controls
        float yOffsetThisFrame  = moveSpeed * yThrow * Time.deltaTime;
        float rawYPos           = transform.localPosition.y + yOffsetThisFrame;
        float yClamped          = Mathf.Clamp(rawYPos, -yRange, yRange);

        //combine both h & z values into new ship position
        transform.localPosition = new Vector3(xClamped, yClamped, transform.localPosition.z);
    }

    private void Rotation() 
    {
        float pitch             = transform.localPosition.y * positionPitchFactor + (yThrow * controlPitchFactor);
        float yaw               = transform.localPosition.x * positionYawFactor + (xThrow * controlYawFactor);
        float roll              = xThrow * controlRollFactor;
        transform.localRotation =  Quaternion.Euler(pitch, yaw, roll); //working but not fluid movement
        //transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(pitch, yaw, roll), .1f); //attempt at making movement less jerky, not final
    }

    private void RotationFluid() //more fluid form of rotation by using Lerp, and better positional rotation based on position compared to center
    {
        float pitch             = transform.localPosition.y * positionPitchFactor   + (yThrow * controlPitchFactor) - transform.localPosition.y;
        float yaw               = transform.localPosition.x * positionYawFactor     + (xThrow * controlYawFactor)   - transform.localPosition.x;
        float roll              = xThrow * controlRollFactor + (transform.localRotation.x*transform.localRotation.y)* 360;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(pitch, yaw, roll), .1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartDeathSequence();
        /*if (other.tag == "Enemy")
        {
            Debug.Log("Enemy");
        }
        else
            Debug.Log("triggered");*/
    }

    private void StartDeathSequence()
    {
        controlEnabled = false;
        deathFX.SetActive(true);
        FindObjectOfType<LevelLoader>().RestartLevel();
    }
}
