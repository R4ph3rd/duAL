using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class DiskettePort : MonoBehaviour
{
    public bool isDisketteIn=false;
    public Diskette diskette;

    

    private void Update()
    {
        /*Checking if the diskette is still in*/
        if(isDisketteIn)
        {
            if(transform.childCount == 0)
            {
                isDisketteIn = false;
                diskette = null;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Diskette newDiskette;
        if(other.TryGetComponent<Diskette>(out newDiskette))
        {
            Debug.Log("<color=yellow>Diskette Pluged-in</color>");
            Grabbable grabbable;
            if(TryGetComponent<Grabbable>(out grabbable))
            {
                grabbable.enabled = false;
            }

            Interactable interactable;
            if (TryGetComponent<Interactable>(out interactable))
            {
                interactable.enabled = false;
            }

            isDisketteIn = true;
            newDiskette.transform.SetParent(this.transform);
            newDiskette.transform.localPosition = new Vector3(0, 0, 0);
            newDiskette.transform.localRotation = Quaternion.identity;
            diskette = newDiskette;
            GameManager.GetManager().TriggerDisketteSequence();
        }
    }

    
    
}
