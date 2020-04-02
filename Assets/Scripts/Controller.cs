using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.VR;

public class Controller : MonoBehaviour
{
    public List<InputDevice> devices = new List<InputDevice>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //InputDevices.GetDevices(devices);
        InputDevices.GetDevicesWithRole(InputDeviceRole.RightHanded, devices);
        Debug.Log(devices.Count);
        foreach(InputDevice device in devices)
        {
            Debug.Log(device.role);
        }
    }
}
