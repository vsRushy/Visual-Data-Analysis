﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePick : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse down");
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit something");
                VisualizationManager.Instance.AddValue(hitInfo.transform.gameObject);
            }
        }
    }
}