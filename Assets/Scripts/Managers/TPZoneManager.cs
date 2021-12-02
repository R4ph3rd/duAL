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
        switch (humanManager.roomID)
        {
            case 1:
                if (humanManager.isTPavalaible)
                {
                    foreach (TeleportPoint tp in bridgeTeleportPoints)
                    {
                        tp.SetLocked(false);
                    }
                }
                else
                {
                    foreach (TeleportPoint tp in bridgeTeleportPoints)
                    {
                        tp.SetLocked(true);
                    }
                }

                foreach(TeleportPoint tp in controlRoomTeleportPoints)
                {
                    tp.SetLocked(true);
                }
                foreach (TeleportPoint tp in storageRoomTeleportPoints)
                {
                    tp.SetLocked(true);
                }
                break;
            case 2:
                if (humanManager.isTPavalaible)
                {
                    foreach (TeleportPoint tp in controlRoomTeleportPoints)
                    {
                        tp.SetLocked(false);
                    }
                }
                else
                {
                    foreach (TeleportPoint tp in controlRoomTeleportPoints)
                    {
                        tp.SetLocked(true);
                    }
                }

                foreach (TeleportPoint tp in bridgeTeleportPoints)
                {
                    tp.SetLocked(true);
                }
                foreach (TeleportPoint tp in storageRoomTeleportPoints)
                {
                    tp.SetLocked(true);
                }
                break;
            case 3:
                if (humanManager.isTPavalaible)
                {
                    foreach (TeleportPoint tp in storageRoomTeleportPoints)
                    {
                        tp.SetLocked(false);
                    }
                }
                else
                {
                    foreach (TeleportPoint tp in storageRoomTeleportPoints)
                    {
                        tp.SetLocked(true);
                    }
                }

                foreach (TeleportPoint tp in controlRoomTeleportPoints)
                {
                    tp.SetLocked(true);
                }
                foreach (TeleportPoint tp in bridgeTeleportPoints)
                {
                    tp.SetLocked(true);
                }
                break;
        }
    }
}
