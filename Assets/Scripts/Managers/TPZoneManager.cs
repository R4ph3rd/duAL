using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TPZoneManager : MonoBehaviour
{
    public HumanManager humanManager;

    public TeleportMarkerBase[] bridgeTeleportPoints;
    public TeleportMarkerBase[] controlRoomTeleportPoints;
    public TeleportMarkerBase[] storageRoomTeleportPoints;



    private void Start()
    {

    }

    private void Update()
    {
        UpdateLockStates();
    }


    /// <summary>
    /// Locks or unlocks the tp zones depending on the location of the player and on the availability of its power
    /// </summary>
    public void UpdateLockStates()
    {
        if (humanManager.isTPavalaible)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            //foreach (TeleportMarkerBase tp in bridgeTeleportPoints)
            //{
            //    tp.SetLocked(humanManager.roomID == Room.bridge);
            //    SetTPPoint(tp, humanManager.roomID == Room.bridge);
            //}

            //foreach (TeleportMarkerBase tp in controlRoomTeleportPoints)
            //{
            //    tp.SetLocked(humanManager.roomID == Room.control);
            //    SetTPPoint(tp, humanManager.roomID == Room.control);
            //}

            //foreach (TeleportMarkerBase tp in storageRoomTeleportPoints)
            //{
            //    tp.SetLocked(humanManager.roomID == Room.storage);
            //    SetTPPoint(tp, humanManager.roomID == Room.storage);
            //}
        } else
        {
            //foreach (TeleportMarkerBase tp in bridgeTeleportPoints)
            //{
            //    tp.SetLocked(false);
            //    SetTPPoint(tp, false);
            //}

            //foreach (TeleportMarkerBase tp in controlRoomTeleportPoints)
            //{
            //    SetTPPoint(tp, false);
            //        tp.SetLocked(false);
            //}

            //foreach (TeleportMarkerBase tp in storageRoomTeleportPoints)
            //{
            //    SetTPPoint(tp, false);
            //    tp.SetLocked(false);
            //}
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void SetTPPoint(TeleportMarkerBase tp, bool val)
    {
        if (tp.GetComponent<TeleportPoint>())
        {
            tp.GetComponent<TeleportPoint>().enabled = val;
            tp.GetComponent<TeleportPoint>().locked = val;
        }
        if (tp.GetComponent<TeleportArea>())
        {
            tp.GetComponent<TeleportArea>().enabled = val;
            tp.GetComponent<TeleportArea>().locked = val;
        }
    }
}
