using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskettePort : MonoBehaviour
{
    public bool isDisketteIn=false;
    private Diskette diskette;

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
            isDisketteIn = true;
            newDiskette.transform.SetParent(this.transform);
            newDiskette.transform.localPosition = new Vector3(0, 0, 0);
            diskette = newDiskette;
            GameManager.GetManager().TriggerDisketteSequence();
        }
    }

    
}
