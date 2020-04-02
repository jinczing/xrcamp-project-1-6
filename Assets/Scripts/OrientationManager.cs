using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class OrientationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.XR.InputTracking.Recenter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
