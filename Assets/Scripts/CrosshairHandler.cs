using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CrosshairHandler : MonoBehaviour
{
    [Tooltip("Select the target on the ship")] [SerializeField] Transform crosshairTarget;
    Camera cam;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        try
        {
            Vector3 screenPos = cam.WorldToScreenPoint(crosshairTarget.position);
            transform.position = screenPos;
        }
        catch { Debug.Log("crosshair trying to move while object is destroyed/can't pass transform"); }
    }
}