using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 
    IA_QTE_MiniGame : MonoBehaviour
{

    /* MUST BE ATTACHED TO AN OBJECT WITH Computer COMPONENT */

    // Start is called before the first frame update
    //public GameObject QTEMiniGameUI;
    public Slider progressionBar;
    public Image LedImg;

    public Image NextGestureImage;
    public Sprite[] GesturesImg = new Sprite[4];
    public Sprite[] LedStatus = new Sprite[3];
    public Sprite[] EndQTEScreen = new Sprite[3];
    public Text RemainingSteps;

    public enum Gestures
    {
        //Square,
        //Round,
        //Triangle,
        //Cross,
        V,
        Ok,
        Fist,
        Pray,
        None
    }

    private Gestures NextKey;
    private int InitialNumberOfSteps;
    private int NumberOfSteps;

    private bool WaitingForKey = false;
    private int StepSuccess = 0;

    public float ErrorMargin = .5f;
    public float minValueToWin = .5f;
    public float StepDelay = 1f;
    public int maxSteps = 20;

    private SoundManager sm;
    private AudioSource source;

    private Computer BindedComputer;

    void Start()
    {
        HandPoseDetector.newPoseEvent += OnNewPoseDetected;
        sm = SoundManager.GetSoundManager();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void OnNewPoseDetected()
    {
        Debug.Log($"<color=red>Event raised</color>");
        if (WaitingForKey)
        {
            WaitingForKey = !WaitingForKey;
            Gestures key = HandPoseDetector.instance.lastRecordedPose;
            Debug.Log($"<color=orange>Gesture {key}</color>");

            if (key == NextKey)
            { // step successfully done in time
                StepSuccess++;
                source.clip = sm.SuccessStepSound;
                sm.PlaySound(source);

                progressionBar.value = (float)StepSuccess / InitialNumberOfSteps;
                LedImg.sprite = LedStatus[1];
            }
            else
            { // recognized step is not the expected one
                source.clip = sm.FailureStepSound;
                sm.PlaySound(source);
                LedImg.sprite = LedStatus[2];
            }

            key = Gestures.None;
        }

    }

    public void StartQTE(Computer computer)
    {
        BindedComputer = computer;
        Debug.Log("start qte mini game");
        //QTEMiniGameUI.SetActive(true);
        ResetUI();
        resetStatus();
        StartCoroutine(GenerateStep());
    }

    void MissStep()
    {
        source.clip = sm.FailureStepSound;
        sm.PlaySound(source);
        LedImg.sprite = LedStatus[2];
    }

    void ResetUI()
    {
        NextGestureImage.sprite = null;
        NextGestureImage.sprite = EndQTEScreen[0];
        LedImg.sprite = LedStatus[0];
    }

    void resetStatus()
    {
        InitialNumberOfSteps = (int)Random.Range(8, maxSteps);
        NumberOfSteps = InitialNumberOfSteps;
        StepSuccess = 0;
        progressionBar.value = 0;
    }

    IEnumerator GenerateStep()
    {
        yield return new WaitForSecondsRealtime(StepDelay);

        // if gesture hasn't been done in time, consider it's a failure
        if (WaitingForKey) MissStep();
        if (NumberOfSteps > 0)
        {
            StartCoroutine(GenerateStep());
            NextKey = (Gestures)Random.Range(0, System.Enum.GetValues(typeof(Gestures)).Length - 1);
            NextGestureImage.sprite = null;
            StartCoroutine(FadeNextStep());
            NumberOfSteps--;
            RemainingSteps.text = NumberOfSteps.ToString();
            WaitingForKey = true;
            
        } else
        {
            EndMiniGame();
        }
    }

    IEnumerator FadeNextStep()
    {
        yield return new WaitForSeconds(.2f);
        NextGestureImage.sprite = GesturesImg[(int)NextKey];
    }

    void EndMiniGame()
    {

        ResetUI();

        Debug.Log("Score : " + StepSuccess + " / " + InitialNumberOfSteps + " = " + ((float)StepSuccess / (float)InitialNumberOfSteps));
        if (((float)StepSuccess / (float)InitialNumberOfSteps) > minValueToWin)
        {
            Debug.Log("-- QTE mini game success -- ");
            NextGestureImage.sprite = EndQTEScreen[1];
            BindedComputer.GetComponent<Computer>().CaptureComputer(GameManager.Owner.IA);
        } else
        {
            Debug.Log("-- QTE mini game failed -- ");
            NextGestureImage.sprite = EndQTEScreen[2];
            BindedComputer.GetComponent<Computer>().FailedMiniGame(GameManager.Owner.IA);
        }

        resetStatus();

        StartCoroutine(FadeQTEUI());
    }

    IEnumerator FadeQTEUI()
    {
        yield return new WaitForSecondsRealtime(3);
        //QTEMiniGameUI.SetActive(false);
    }
}
