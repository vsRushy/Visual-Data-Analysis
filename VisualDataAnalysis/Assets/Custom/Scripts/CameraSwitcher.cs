using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject main_camera;
    public GameObject heatmap_camera;

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            main_camera.SetActive(!main_camera.activeSelf);
            heatmap_camera.SetActive(!main_camera.activeSelf);
        }
    }
}
