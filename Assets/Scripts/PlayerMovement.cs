using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
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

    float xThrow, yThrow;

    // Update is called once per frame
    void Update()
    {
        //xMovement();
        //yMovement();
        Movement();
        RotationFluid();
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
}
