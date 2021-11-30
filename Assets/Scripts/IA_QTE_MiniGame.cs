using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 
    IA_QTE_MiniGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject QTEMiniGameUI;
    public AudioClip SuccessStepSound, FailureStepSound;
    public Slider progressionBar;
    public Image LedImg;

    public Image NextGestureImage;
    public Sprite[] GesturesImg = new Sprite[4];
    public Sprite[] LedStatus = new Sprite[3];
    public Sprite[] EndQTEScreen = new Sprite[3];
    public Text RemainingSteps;

    public enum Gestures
    {
        Square,
        Round,
        Triangle,
        Cross,
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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (WaitingForKey)
        {
            if (Input.anyKeyDown)
            {
                WaitingForKey = !WaitingForKey;
                Gestures key = Gestures.None;

                if(Input.GetKeyDown("a"))
                {
                    key = Gestures.Square;
                }
                if (Input.GetKeyDown("z"))
                {
                    key = Gestures.Cross;
                }
                if (Input.GetKeyDown("e"))
                {
                    key = Gestures.Triangle;
                }
                if (Input.GetKeyDown("r"))
                {
                    key = Gestures.Round;
                }

                Debug.Log("key detected : " + key);


                if (key == NextKey)
                { // step successfully done in time
                    StepSuccess++;
                    PlaySound(SuccessStepSound);

                    progressionBar.value = (float)StepSuccess / InitialNumberOfSteps;
                    LedImg.sprite = LedStatus[1];
                } else
                { // recognized step is not the expected one
                    PlaySound(FailureStepSound);
                    LedImg.sprite = LedStatus[2];
                }
            }
        }
    }

    public void StartQTE()
    {
        Debug.Log("start qte mini game");
        QTEMiniGameUI.SetActive(true);
        ResetUI();
        resetStatus();
        StartCoroutine(GenerateStep());
    }

    void MissStep()
    {
        PlaySound(FailureStepSound);
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
        } else
        {
            Debug.Log("-- QTE mini game failed -- ");
            NextGestureImage.sprite = EndQTEScreen[2];
        }

        resetStatus();

        StartCoroutine(FadeQTEUI());
    }

    IEnumerator FadeQTEUI()
    {
        yield return new WaitForSecondsRealtime(3);
        QTEMiniGameUI.SetActive(false);
    }

    private void PlaySound(AudioClip sound)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = sound;

        if (audio.isPlaying)
        {
            audio.time = 0;
            audio.Play();
        }
        else
        {
            audio.Play();
        }
    }
}
