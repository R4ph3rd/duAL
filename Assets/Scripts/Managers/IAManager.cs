using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAManager : MonoBehaviour
{
    public GameObject IACam;
    public Room RoomID = Room.bridge;
    public GameObject[] IACamTargets = new GameObject[3];

    public GameObject[] UIGestureControls = new GameObject[3];
    public GameObject[] UIHackButtons = new GameObject[2];

    //public Color HumanColor;
    //public Color IAColor;
    //public GameObject ScoreBar;
    //private GameObject ScoreFill;
    //private RectTransform ScorePos;

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
        ScoreFill = ScoreBar.transform.GetChild(2).GetChild(0).gameObject;
        ScorePos = ScoreFill.GetComponent<RectTransform>();
        ScoreBar.transform.GetChild(1).GetComponent<Image>().color = IAColor;
        ScoreFill.GetComponent<Image>().color = HumanColor;
    }

    void Update()
    {
        //Debug
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

        /*Updating button display*/
        UpdateUIDisplay();
        

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

    private void UpdateUIDisplay()
    {
        switch (RoomID)
        {
            case Room.bridge:
                UIHackButtons[1].SetActive(true);
                break;
            case Room.control:
                UIHackButtons[1].SetActive(false);
                break;
            case Room.storage:
                UIHackButtons[1].SetActive(true);
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
}
