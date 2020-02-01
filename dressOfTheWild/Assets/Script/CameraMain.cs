using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMain : MonoBehaviour
{
    public static CameraMain instance;
    public Camera mainCam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            mainCam = this.GetComponent<Camera>();
        }
    }
}
