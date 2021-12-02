using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    public GameObject IACam;
    public Room RoomID = Room.bridge;
    public GameObject[] IACamTargets = new GameObject[3];

    private static IAManager _this = null;
    public static IAManager GetIAManager()
    {
        if (_this == null)
        {
            _this = new IAManager();
        }
        return _this;
    }
    void Start()
    {
        ChangeCam(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            print("swipe left");
            ChangeCam(false);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            print("swipe right");
            ChangeCam(true);
        }
    }

    public void ChangeCam(bool SwipeDir)
    {
        if (SwipeDir)
        {
            RoomID++;
            RoomID = (int)RoomID > 2 ? Room.bridge : RoomID;
        } else
        {
            RoomID--;
            RoomID = (int)RoomID < 0 ? Room.storage : RoomID;
        }

        IACam.transform.position = IACamTargets[(int)RoomID].transform.position;
        IACam.transform.rotation = IACamTargets[(int)RoomID].transform.rotation;
    }
}
