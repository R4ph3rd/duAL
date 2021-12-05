using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class IAManager : MonoBehaviour
{
    public GameObject IACam;
    public Room RoomID = Room.storage;
    public GameObject[] IACamTargets = new GameObject[3];

    public GameObject[] UIGestureControls = new GameObject[3];
    public GameObject[] UIHackButtons = new GameObject[5];

    public Text cameraPlaceText;

    private static IAManager _this = null;
    private IA_QTE_MiniGame miniGame;
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
        miniGame = GetComponent<IA_QTE_MiniGame>();
        //ChangeCam(true);
        IACam.transform.position = IACamTargets[(int)Room.storage].transform.position;
        IACam.transform.rotation = IACamTargets[(int)Room.storage].transform.rotation;
        UpdateUIDisplay();
    }

    void Update()
    {
        //Debug
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    print("swipe left");
        //    ChangeCam(false);
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    print("swipe right");
        //    ChangeCam(true);
        //}

        /*Updating button display*/
        UpdateUIDisplay();
        

    }

    public void ChangeCam(bool SwipeDir)
    {
        if (miniGame.isMiniGameInstanceRunning) return;
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

    private void UpdateUIDisplay()
    {
        switch (RoomID)
        {
            case Room.bridge:
                ToggleButtons(false, false, true, false, false);
                cameraPlaceText.text = "(BRIDGE)";
                break;
            case Room.control:
                ToggleButtons(true, true, false, false, false);
                cameraPlaceText.text = "(CONTROL ROOM)";
                break;
            case Room.storage:
                ToggleButtons(false, false, false, true, true);
                cameraPlaceText.text = "(STORAGE ROOM)";
                break;

        }

        for(int i=0 ; i < UIGestureControls.Length ; i++)
        {
            if(i == (int)RoomID)
            {
                UIGestureControls[i].SetActive(true);
            }
            else
            {
                UIGestureControls[i].SetActive(false);
            }
        }
    }

    private void ToggleButtons(bool c1, bool c2, bool c3, bool c4, bool c5)
    {
        UIHackButtons[0].SetActive(c1);
        UIHackButtons[1].SetActive(c2);
        UIHackButtons[2].SetActive(c3);
        UIHackButtons[3].SetActive(c4);
        UIHackButtons[4].SetActive(c5);
    }
}
