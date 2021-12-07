using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve;
using Valve.VR.InteractionSystem;

public class EnhancedTeleportPoint : MonoBehaviour
{
    public GameObject target;
    public Room destination;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("ENHANCED TP POINT DETECTED");
            HumanManager.instance.TeleportPower();
            HumanManager.instance.roomID = destination;
            Player.instance.transform.position = target.transform.position;
        }
    }
}
