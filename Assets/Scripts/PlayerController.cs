using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [Header("Stats values")] [SerializeField] int playerHealth = 5;
    [Tooltip("In m/s")] [SerializeField] float moveSpeed = 8f;
    [SerializeField] float deathDelay = 1f;
    [SerializeField] float hurtCooldown = 1f;
    [SerializeField] float waitToStart = 3f;

    [Header("Range of movement on screen")]
    [SerializeField] float xRange = 7f;
    [SerializeField] float yRange = 4f;

    [Header("Screen-position based")]
    [Tooltip("Change these if Z-position is changed")] [SerializeField] float positionPitchFactor = -3.5f;
    [Tooltip("Change these if Z-position is changed")] [SerializeField] float positionYawFactor = 5f;

    [Header("Control-throw based")]
    [SerializeField] float controlPitchFactor = -30f;
    [SerializeField] float controlYawFactor = 30;
    [SerializeField] float controlRollFactor = -30f;

    [Header("Effects")]
    [SerializeField] GameObject deathFX;
    [SerializeField] GameObject hurtFX;

    [Header("Weapons")] //whole weapon select mechanism needs to be rewritten, this array system isn't handy
    [SerializeField] Transform[] weaponLocations;
    [SerializeField] ParticleSystem[] weaponEffects;
    [SerializeField] float[] firingCooldowns = { 2f, .25f };
    [SerializeField] int[] damageValues = { 5, 1 };
    [Tooltip("To prevent the hierarchy from clogging up")] [SerializeField] Transform projectileHolder;
    int selectedWeapon = 0;
    
    float xThrow, yThrow, cooldownTimer;
    bool controlEnabled = true;
    bool firing = false;
    bool onCooldown = false;
    bool weaponIsSwitching = true;
    bool rightHasFired = false;

    Coroutine firingCoroutine;

    private void Start()
    {
        cooldownTimer = firingCooldowns[selectedWeapon];
        FindObjectOfType<HealthDisplay>().UpdateHealthDisplay(playerHealth);
        StartCoroutine(WaitToStart());
    }

    private IEnumerator WaitToStart()
    {
        controlEnabled = false;
        var myWaypointFollower = GetComponentInParent<BetterWaypointFollower>();
        myWaypointFollower.enabled = false;
        yield return new WaitForSeconds(waitToStart);
        myWaypointFollower.enabled = true;
        controlEnabled = true;
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
        if (Input.GetButtonDown("Fire1") && !onCooldown && controlEnabled)
        {
            onCooldown = true;
            firingCoroutine = StartCoroutine(FirePlasmaContiniously());
        }
        else if (Input.GetButtonUp("Fire1") || !controlEnabled)
        {
            if(firingCoroutine!=null)
                StopCoroutine(firingCoroutine);
        }

        if(onCooldown)
        {
            CooldownTimer();
        }

        if (Input.GetButtonDown("Fire2") )
        {
            WeaponSwitcher();
        }
    }

    private void WeaponSwitcher()
    {
        if (selectedWeapon + 1 < weaponEffects.Length)
        {
            selectedWeapon++;
        }
        else
        {
            selectedWeapon = 0;
        }

        cooldownTimer = firingCooldowns[selectedWeapon];
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
            cooldownTimer = firingCooldowns[selectedWeapon];
        }
    }

    IEnumerator FirePlasmaContiniously() //written for 2 weapon placements firing one by one, needs rewrite if more/less
    {
        while (true)
        {
            if (!rightHasFired)
            {
                var weapon = Instantiate(weaponEffects[selectedWeapon], weaponLocations[0].position, weaponLocations[0].rotation);
                weapon.transform.parent = projectileHolder;
                weapon.Emit(1);
                rightHasFired = true;
                yield return new WaitForSeconds(firingCooldowns[selectedWeapon]);
            }

            else
            {
                var weapon = Instantiate(weaponEffects[selectedWeapon], weaponLocations[1].position, weaponLocations[1].rotation);
                weapon.transform.parent = projectileHolder;
                weapon.Emit(1);
                rightHasFired = false;
                yield return new WaitForSeconds(firingCooldowns[selectedWeapon]);
            }
        }
    }

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
            if (firingCoroutine != null)
                StopCoroutine(firingCoroutine);
            StartCoroutine(HurtCooldown());

            //hurt animation/sound 
        }
        else
            StartDeathSequence();
    }

    private IEnumerator HurtCooldown()
    {
        controlEnabled = false;
        yield return new WaitForSeconds(hurtCooldown);
        controlEnabled = true;

    }

    private void StartDeathSequence()
    {
        controlEnabled = false;
        deathFX.SetActive(true);
        FindObjectOfType<ResultScreenHandler>().HasWon(false);
        Destroy(gameObject, deathDelay);
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public int GetDamageValue()
    {
        return damageValues[selectedWeapon];
    }

    public int GetWeaponSelected()
    {
        return selectedWeapon;
    }
}
