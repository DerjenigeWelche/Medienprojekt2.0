﻿using UnityEngine;
using System.Collections;

public class CullingDisabler : MonoBehaviour {

    private Camera thisCamera;
    public void Start()
    {
        thisCamera = GetComponent<Camera>();
    }


    public void PreRenderAdjustFOV(Camera cam)
    {

        if (cam == thisCamera)
        {
            cam.fieldOfView = 60;
        }
    }

    // callback to be called before any culling
    public void PreCullAdjustFOV(Camera cam)
    {
        if (cam == thisCamera)
        {
            cam.fieldOfView = 70;
            //These are needed for the FOV change to take effect.
            cam.ResetWorldToCameraMatrix();
            cam.ResetProjectionMatrix();
        }
    }
    public void OnEnable()
    {
        // register the callback when enabling object
        Camera.onPreCull += PreCullAdjustFOV;
        Camera.onPreRender += PreRenderAdjustFOV;
    }
    public void OnDisable()
    {
        // remove the callback when disabling object
        Camera.onPreCull -= PreCullAdjustFOV;
        Camera.onPreRender -= PreRenderAdjustFOV;
    }
}
