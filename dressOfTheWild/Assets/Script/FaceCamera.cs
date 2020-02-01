using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Transform visual;
    Transform mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (CameraMain.instance == null)
            GameObject.Find("Main Camera").AddComponent<CameraMain>();


        mainCamera = CameraMain.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
       this.transform.localRotation = new Quaternion(
            mainCamera.transform.rotation.x,
            mainCamera.transform.rotation.y,
            mainCamera.transform.rotation.z,
             transform.rotation.w);
    }
}
