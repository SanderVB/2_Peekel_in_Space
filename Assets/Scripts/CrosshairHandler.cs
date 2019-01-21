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
        Vector3 screenPos = cam.WorldToScreenPoint(crosshairTarget.position);
        transform.position = screenPos;
    }
}