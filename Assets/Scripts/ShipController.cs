using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ShipController : MonoBehaviour
{

    [Tooltip("In m/s")] [SerializeField] float speed = 4f;
    [SerializeField] float xRange;
    [SerializeField] float yRange;
    [SerializeField] float positionPitchFactor = -5f;
    [SerializeField] float controlPitchFactor = -20f;
    [SerializeField] float positionYawFactor = 5f;
    [SerializeField] float controlYawFactor = 20f;
    [SerializeField] float controlRollFactor = -45f;

    float xThrow, yThrow;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //xMovement();
        //yMovement();
        Movement();
        Rotation();
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
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        float xOffsetThisFrame = speed * xThrow * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffsetThisFrame;
        float xClamped = Mathf.Clamp(rawXPos, -xRange, xRange);

        //set vertical movement values
        yThrow = -CrossPlatformInputManager.GetAxis("Vertical"); //minus because flight controls
        float yOffsetThisFrame = speed * yThrow * Time.deltaTime;
        float rawYPos = transform.localPosition.y + yOffsetThisFrame;
        float yClamped = Mathf.Clamp(rawYPos, -yRange, yRange);

        //combine both h & z values into new ship position
        transform.localPosition = new Vector3(xClamped, yClamped, transform.localPosition.z);
    }

    private void Rotation() //TODO vloeiend maken
    {
        float pitch = transform.localPosition.y * positionPitchFactor + (yThrow * controlPitchFactor);
        float yaw = transform.localPosition.x * positionYawFactor + (xThrow * controlYawFactor);
        float roll = xThrow * controlRollFactor;
        transform.localRotation =  Quaternion.Euler(pitch, yaw, roll); //working but not fluid movement
        //transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(pitch, yaw, roll), .1f); //attempt at making movement less jerky, not final
    }

    private void Rotation_Test()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("Enemy");
        }
        else
            Debug.Log("triggered");
    }
}
