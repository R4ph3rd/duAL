using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.XR;
using Valve;
using UnityEngine.XR;
using Valve.VR;

public enum Room { bridge = 0, storage =1, control =2};
public enum State { Intro, Game, GameOver};

public class GameManager : MonoBehaviour
{
    //GameState
    public State gameState = State.Intro;
    public AudioSource mainAudioSource;
    public GameObject[] vfxGameObjects;

    // Capture the flag manager
    public List<Computer> computers = new List<Computer>();
    public IAManager aiPlayer;
    public GameObject aiInstructionsPanel;

    private static GameManager _this = null;
    public static GameManager GetManager()
    {
        if (_this == null)
        {
            _this = FindObjectOfType<GameManager>(); ;
        }
        return _this;
    }

    public enum Owner
    {
        IA,
        Human,
        None
    }

    [Header("Gravity Power")]
    public GameObject[] gravitySensitiveObjects;
    public float gravityPowerImpulse = 1f;
    public float gravityPowerCoolDown = 60f;
    public float gravityPowerDuration = 10f;
    private bool isGravityPowerTriggered = false;

    [Header("Diskette")]
    //public DisketteDispenser disketteDispenser; //UNUSED
    public DiskettePort diskettePort;
    public float hackingTime = 20f;
    private bool isPlayerHackingAIComputers = false;
    private float hackingTimer=0f;

    public Canvas endscoreImg;

    public GameObject ScoreScreen;

    private Score score;
    public float gameduration = 5;

    public XRInputSubsystem xrInputs = new XRInputSubsystem();

    public RoboticArmController armController;
    public float armDeactivationDuration = 20f;

    void Start()
    {
        /*Setting up player pos*/
        print(xrInputs.TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor));
        xrInputs.TryRecenter();
        Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseStanding);
        Valve.VR.OpenVR.Chaperone.ResetZeroPose(ETrackingUniverseOrigin.TrackingUniverseStanding);

        


        StartCoroutine(PlayIntroSequence());

        StartCoroutine(EndGame());

        foreach (Computer c in FindObjectsOfType<Computer>()){
            computers.Add(c);
        }

        score = Score.GetScore();
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(gameduration * 60);
        print("end game !");
        bool isHumanWining = true;

        foreach(Computer comp in computers)
        {
            if(comp.status == Owner.IA)
            {
                isHumanWining = false;
            }
        }

        if (isHumanWining)
        {

        }
        else
        {

        }
        string winner = score.IAScore > score.HumanScore ? "AI" : score.IAScore == score.HumanScore ? "Nobody" : "Humanoïd Entity";
        endscoreImg.transform.GetChild(1).GetComponent<Text>().text = winner;
        endscoreImg.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        /*Handling the hacking of the AI computers*/
        if (isPlayerHackingAIComputers)
        {
            hackingTimer += Time.deltaTime;
            /*Case of a successfull hacking*/
            if (hackingTimer >= hackingTime)
            {
                Debug.Log("<color=green>Successfull hacking ! AI looses all its computers</color>");
                //foreach (Computer c in computers)
                //{
                //    if(c.status == Owner.IA)
                //    {
                //        c.CaptureComputer(Owner.None);
                //    }
                //}
                StartCoroutine(ShutdownArm());
                print("diskette reseted");
                StartCoroutine(diskettePort.diskette.ResetDiskette());
                isPlayerHackingAIComputers = false;
                
                /*Display a log on the AI interface*/
                /*Play a hacking sound ?*/
            }
            /*Case someone removed the diskette before the end of the hacking sequence*/
            else if((!diskettePort.isDisketteIn)&&(hackingTimer < hackingTime))
            {
                Debug.Log("<color=red>Diskette removed before the end of the hacking sequence</color>");
                isPlayerHackingAIComputers = false;
            }
        }
    }

    /// <summary>
    /// Allows the AI to stop the gravity system on the ship for xx secs
    /// </summary>
    public void TriggerGravityPower()
    {
        if (!isGravityPowerTriggered && aiPlayer.RoomID != Room.storage)
        {
            Debug.Log("<color=red>GRAVITY SYSTEM DISABLED</color>");
            isGravityPowerTriggered = true;
            StartCoroutine(DisableGravityRoutine());
        }
    }

    IEnumerator DisableGravityRoutine()
    {
        /*AudioClip*/
        mainAudioSource.PlayOneShot(SoundManager.GetSoundManager().gravitySound);

        /*Reducing the gravity force to zero*/
        Physics.gravity = new Vector3(0, 0, 0);

        /*Giving a small impule to the objects with physics*/
        if (gravitySensitiveObjects.Length > 0)
        {
            Rigidbody rigidbody;
            foreach (GameObject obj in gravitySensitiveObjects)
            {
                if(obj.TryGetComponent<Rigidbody>(out rigidbody))
                {
                    rigidbody.AddForce(new Vector3(0, gravityPowerImpulse, 0), ForceMode.Impulse);
                    rigidbody.AddTorque(new Vector3(Random.Range(0.05f, 0.1f), Random.Range(0.05f, 0.1f), Random.Range(0.05f, 0.1f)));
                }
            }
        }

        yield return new WaitForSeconds(gravityPowerDuration);

        /*Reactivitating the gravity*/
        Physics.gravity = new Vector3(0, -9.81f, 0);

        Debug.Log("<color=green>GRAVITY SYSTEM ENABLED</color>");

        /*cooldown phase*/
        yield return new WaitForSeconds(gravityPowerCoolDown);
        isGravityPowerTriggered = false;
    }

    /// <summary>
    /// Allows the human to trigger an Electromagnetic impulse to disturb the AI's systems
    /// </summary>
    public void TriggerEMIPower()
    {

    }


    public void TriggerDisketteSequence()
    {
        isPlayerHackingAIComputers = true;
        hackingTimer = 0f;
    }

    IEnumerator PlayIntroSequence()
    {
        yield return new WaitForSeconds(2f);
        mainAudioSource.PlayOneShot(SoundManager.GetSoundManager().introVoice);

        yield return new WaitForSeconds(6f);
        mainAudioSource.PlayOneShot(SoundManager.GetSoundManager().instruTPVoice);

        yield return new WaitForSeconds(4f);
        mainAudioSource.PlayOneShot(SoundManager.GetSoundManager().instruHackVoice);

        foreach (GameObject vfx in vfxGameObjects)
        {
            vfx.SetActive(true);
        }

        gameState = State.Game;

        

        yield return new WaitForSeconds(12f);
        mainAudioSource.PlayOneShot(SoundManager.GetSoundManager().infoDiskVoice);

        yield return new WaitForSeconds(16.5f);
        mainAudioSource.PlayOneShot(SoundManager.GetSoundManager().goodLuckVoice);

        aiInstructionsPanel.SetActive(false);

    }

    IEnumerator ShutdownArm()
    {
        armController.isControlEnable = false;
        yield return new WaitForSeconds(armDeactivationDuration);
        armController.isControlEnable = true;
    }
}
