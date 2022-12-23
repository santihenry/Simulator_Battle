using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Canvas canvas;
    void Start()
    {
        canvas.worldCamera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(canvas.worldCamera.transform);
    }
}
