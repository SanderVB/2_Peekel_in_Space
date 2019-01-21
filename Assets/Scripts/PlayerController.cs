using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [Header("Stats values")] [SerializeField] int playerHealth = 5;
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

    [Header("Weapons")] //whole weapon select mechanism needs to be rewritten for multiple weapons, this array system isn't handy
    [SerializeField] Transform[] weaponLocations;
    [SerializeField] ParticleSystem[] weaponEffects;
    [SerializeField] float[] firingCooldowns = { 2f, .25f };
    [SerializeField] int[] damageValues = { 5, 1 };
    [Tooltip("To prevent the hierarchy from clogging up")] [SerializeField] Transform projectileHolder;
    int weaponSelected = 0;

    float xThrow, yThrow, cooldownTimer;
    bool controlEnabled = true;
    bool firing = false;
    bool onCooldown = false;
    bool weaponIsSwitching = true;
    bool rightHasFired = false;

    Coroutine firingCoroutine;

    private void Start()
    {
        cooldownTimer = firingCooldowns[weaponSelected];
        FindObjectOfType<HealthDisplay>().UpdateHealthDisplay(playerHealth);
    }

    void Update()
    {
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
                firingCoroutine = StartCoroutine(FirePlasmaContiniously());
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }

        if(onCooldown)
        {
            CooldownTimer();
        }

        if (Input.GetButtonDown("Fire2") )
        {
            WeaponSwitcher();
            Debug.Log("Selected weapon: " + weaponSelected);
        }
    }

    private void WeaponSwitcher()
    {
        if (weaponSelected + 1 < weaponEffects.Length)
        {
            weaponSelected++;
        }
        else
        {
            weaponSelected = 0;
        }
        cooldownTimer = firingCooldowns[weaponSelected];
        FindObjectOfType<WeaponDisplay>().WeaponDisplayChanger();
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
            cooldownTimer = firingCooldowns[weaponSelected];
        }
    }

    IEnumerator FirePlasmaContiniously() //written for 2 weapons firing one by one, needs rewrite if more/less
    {
        while (true)
        {
            if (!rightHasFired)
            {
                ParticleSystem laser = Instantiate(weaponEffects[weaponSelected], weaponLocations[0].position, weaponLocations[0].rotation);
                laser.transform.parent = projectileHolder;
                laser.Emit(1);
                rightHasFired = true;
                yield return new WaitForSeconds(firingCooldowns[weaponSelected]);
            }

            else
            {
                ParticleSystem laser = Instantiate(weaponEffects[weaponSelected], weaponLocations[1].position, weaponLocations[1].rotation);
                laser.transform.parent = projectileHolder;
                laser.Emit(1);
                rightHasFired = false;
                yield return new WaitForSeconds(firingCooldowns[weaponSelected]);
            }
        }
    }

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

    private void Rotation() //working but not fluid movement
    {
        float pitch             = transform.localPosition.y * positionPitchFactor + (yThrow * controlPitchFactor);
        float yaw               = transform.localPosition.x * positionYawFactor + (xThrow * controlYawFactor);
        float roll              = xThrow * controlRollFactor;
        transform.localRotation =  Quaternion.Euler(pitch, yaw, roll); 
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
        playerHealth--;
        FindObjectOfType<HealthDisplay>().UpdateHealthDisplay(playerHealth);
        if(playerHealth>0)
        {
            //hurt animation/sound + controls disabled for a short time
        }
        else
            StartDeathSequence();
    }

    private void StartDeathSequence()
    {
        controlEnabled = false;
        deathFX.SetActive(true);
        FindObjectOfType<LevelLoader>().RestartLevel();
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public int GetDamageValue()
    {
        return damageValues[weaponSelected];
    }

    public int GetWeaponSelected()
    {
        return weaponSelected;
    }
}
