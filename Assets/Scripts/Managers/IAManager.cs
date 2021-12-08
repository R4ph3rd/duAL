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

    /*AI SUPER POWERS*/
    public ParticleSystem smokeScreenParticleSystem;
    public bool isSmokeScreenAvailable = true;
    public float smokeScreenCooldown=30f;
    public float smokeScreenDuration=30f;

    void Start()
    {
        miniGame = GetComponent<IA_QTE_MiniGame>();
        //ChangeCam(true);
        IACam.transform.position = IACamTargets[(int)RoomID].transform.position;
        IACam.transform.rotation = IACamTargets[(int)RoomID].transform.rotation;
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
                cameraPlaceText.text = "(PONT)";
                break;
            case Room.control:
                ToggleButtons(false, false, false, false, false);
                cameraPlaceText.text = "(S. CONTROLE)";
                break;
            case Room.storage:
                ToggleButtons(false, false, false, false, false);
                cameraPlaceText.text = "(S. STOCKAGE)";
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

    public void TriggerSmokeScreen()
    {
        if (isSmokeScreenAvailable && RoomID == Room.bridge)
        {
            isSmokeScreenAvailable = false;
            StartCoroutine(TriggerSmokeScreenCoroutine());
        }
    }

    IEnumerator TriggerSmokeScreenCoroutine()
    {
        smokeScreenParticleSystem.Play();
        foreach(Computer comp in GameManager.GetManager().computers)
        {
            Outline compOutline;
            if (comp.RoomID == Room.bridge && TryGetComponent<Outline>(out compOutline))
            {
                compOutline.enabled = false;
            }
        }

        yield return new WaitForSeconds(smokeScreenDuration);
        smokeScreenParticleSystem.Stop();
        foreach (Computer comp in GameManager.GetManager().computers)
        {
            Outline compOutline;
            if (comp.RoomID == Room.bridge && TryGetComponent<Outline>(out compOutline))
            {
                compOutline.enabled = true;
            }
        }

        yield return new WaitForSeconds(smokeScreenCooldown);
        isSmokeScreenAvailable = true;
    }
}
