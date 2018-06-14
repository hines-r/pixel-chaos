using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspect : MonoBehaviour
{
    void Start()
    {
        GetComponent<Camera>().aspect = 16f / 9f;
    }
}
