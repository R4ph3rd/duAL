using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private bool isGrabbed = false;
    public bool IsGrabbed { get => isGrabbed;}
    
    /// <summary>
    /// Update the grabbed state of the object as a boolean
    /// </summary>
    /// <param name="status">true if the gameobject is being grabbed</param>
    public void UpdateGrabStatus(bool status)
    {
        Rigidbody rb;
        if (isGrabbed && !status)
        {
            isGrabbed = false;
            if(TryGetComponent<Rigidbody>(out rb))
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }
        else if (!isGrabbed && status)
        {
            isGrabbed = true;
            if (TryGetComponent<Rigidbody>(out rb))
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }
}
