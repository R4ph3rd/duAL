using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

/* MUST BE ATTACHED TO AN OBJECT WITH Computer COMPONENT */

public class InteractableKeyboard_MiniGame : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera HumanCam;
    private List<GameObject> Buttons = new List<GameObject>();
    public GameObject ButtonsParent;
    public GameObject StartButton;
    public GameObject ActiveMiniGameScreens;

    private List<Slider> progressionBar = new List<Slider>();
    private List<Text> RemainingSteps = new List<Text>();
    public Material[] ButtonStatus = new Material[4]; // 0 : neutral | 1 : to push | 2 : success | 3 : wrong 
    public Material[] InteractableStatus = new Material[2]; // active / inactive;
    public Color HumanColor;
    public Color IAColor;

    public bool isMiniGamePlaying = false;
    private GameObject NextKey = null;
    private int InitialNumberOfSteps;
    private int NumberOfSteps;
    private bool WaitingForKey = false;
    private int StepSuccess = 0;

    public float ErrorMargin = .5f;
    public float minRatioToWin = .5f;
    public float StepDelay = 2f;
    public int maxSteps = 20;
    public int minSteps = 8;
    public int blinkRepeat = 6;
    public float blinkFrequence = .3f;

    private SoundManager sm;
    private AudioSource source;

    void Start()
    {
        sm = SoundManager.GetSoundManager();
        source = GetComponent<AudioSource>();

        foreach(Canvas cnv in ActiveMiniGameScreens.GetComponentsInChildren<Canvas>())
        {
            Slider Progbar = cnv.GetComponentInChildren<Slider>(true);
            Progbar.maxValue = maxSteps;
            Progbar.minValue = minSteps;
            progressionBar.Add(Progbar);
            RemainingSteps.Add(cnv.transform.GetChild(2).GetChild(0).GetComponent<Text>());
        }

        if (HumanCam != null) { }
        else
        {
            HumanCam = GameObject.Find("VRCamera").GetComponent<Camera>();
        }

        Transform[] buttons = ButtonsParent.GetComponentsInChildren<Transform>();
        print("buttons are " + buttons.Length);
        foreach(Transform button in buttons)
        {
            if (button.gameObject.name.Contains("Push button"))
            {
                Buttons.Add(button.gameObject);
            }
        }

        print("buttons length : " + Buttons.Count);

        // Instantiate game screens
        //foreach(GameObject ActiveScreen in ActiveMiniGameScreens)
        //{
        //    GameObject screen = Instantiate(GameScreenTemplate);
        //    screen.transform.position = ActiveScreen.transform.position;
        //    screen.transform.localRotation = ActiveScreen.transform.rotation;
        //    screen.transform.parent = ActiveScreen.transform;
        //    screen.GetComponent<Canvas>().worldCamera = HumanCam;
        //    screen.SetActive(false);
        //    GameScreensCanvas.Add(screen);
        //}
    }


    public void StartMiniGame()
    {
        if (!isMiniGamePlaying)
        {
            isMiniGamePlaying = !isMiniGamePlaying;

            ResetUI();
            resetStatus();
            StartCoroutine(GenerateStep());
            StartButton.GetComponentInChildren<Renderer>().material = InteractableStatus[1];
            ActiveMiniGameScreens.SetActive(true);
        }
    }

    public void ButtonPushed(GameObject btn)
    {
        print(btn.name + "has been touched, expected : " + NextKey.name);
        if (btn.name == NextKey.name)
        {
            StepSuccess++;
            print("great ! score : " + StepSuccess);

            foreach (Slider progBar in progressionBar)
            {
                progBar.value = StepSuccess;
            }

            SuccessStepFX(btn);
        } else
        {
            MissStepFX(btn);
        }
    }

    void MissStepFX(GameObject btn)
    {
        source.clip = sm.FailureStepSound;
        sm.PlaySound(source);
        btn.GetComponentInChildren<Renderer>().material = ButtonStatus[3];

        StartCoroutine(FadeKey(btn));
    }

    void MissStepFX()
    {
        source.clip = sm.FailureStepSound;
        sm.PlaySound(source);
    }

    void SuccessStepFX(GameObject btn)
    {
        source.clip = sm.SuccessStepSound;
        sm.PlaySound(source);
        btn.GetComponentInChildren<Renderer>().material = ButtonStatus[2];

        StartCoroutine(FadeKey(btn));
    }

    void ResetUI()
    {
        foreach (GameObject btn in Buttons)
        {
            btn.GetComponentInChildren<Renderer>().material = ButtonStatus[0];
        }
        StartButton.GetComponentInChildren<Renderer>().material = InteractableStatus[0];
    }

    void resetStatus()
    {
        InitialNumberOfSteps = (int)Random.Range(minSteps, maxSteps);
        NumberOfSteps = InitialNumberOfSteps;
        StepSuccess = 0;

        foreach (Slider progBar in progressionBar)
        {
            progBar.value = 0;
        }
    }

    IEnumerator GenerateStep()
    {
        yield return new WaitForSecondsRealtime(StepDelay);

        // if gesture hasn't been done in time, consider it's a failure
        if (WaitingForKey) MissStepFX();
        if (NumberOfSteps > 0)
        {
            StartCoroutine(GenerateStep());

            if (NextKey != null)
            {
                NextKey.GetComponentInChildren<Renderer>().material = ButtonStatus[0];
            }

            NextKey = Buttons[Random.Range(0, Buttons.Count)];
            //print("generate step :" + NextKey.name);
            StartCoroutine(FadeNextStep());
            NumberOfSteps--;

            foreach (Text txt in RemainingSteps)
            {
                txt.text = NumberOfSteps.ToString();
            }

            WaitingForKey = true;
        }
        else
        {
            EndMiniGame();
        }
    }

    IEnumerator FadeNextStep()
    {
        yield return new WaitForSeconds(.2f);
        NextKey.GetComponentInChildren<Renderer>().material = ButtonStatus[1];
    }

    IEnumerator FadeKey(GameObject btn)
    {
        yield return new WaitForSecondsRealtime(.2f);
        btn.GetComponentInChildren<Renderer>().material = ButtonStatus[0];
    }

    void EndMiniGame()
    {

        ResetUI();

        Debug.Log("Score : " + StepSuccess + " / " + InitialNumberOfSteps + " = " + ((float)StepSuccess / (float)InitialNumberOfSteps));
        if (((float)StepSuccess / (float)InitialNumberOfSteps) > minRatioToWin)
        {
            Debug.Log("-- QTE mini game success -- ");
            GetComponent<Computer>().CaptureComputer(GameManager.Owner.Human);
            GameManager.GetManager().mainAudioSource.PlayOneShot(SoundManager.GetSoundManager().humanHackedSound);
        }
        else
        {
            Debug.Log("-- QTE mini game failed -- ");
            GetComponent<Computer>().FailedMiniGame(GameManager.Owner.Human);
        }

        for (int i = 0; i < blinkRepeat; i++)
        {
            // if (int)Mathf.Round((float)StepSuccess / (float)InitialNumberOfSteps) < threshold , return 1 so take the [2] => success material
            StartCoroutine(BlinkKeyboard(3 - (int)Mathf.Round((float)StepSuccess / (float)InitialNumberOfSteps), i));
        }

        resetStatus();

        StartCoroutine(FadeQTEUI());
        isMiniGamePlaying = false;
    }

    IEnumerator FadeQTEUI()
    {
        yield return new WaitForSecondsRealtime((blinkFrequence * blinkRepeat) + 1);
        ActiveMiniGameScreens.SetActive(false);
    }

    IEnumerator BlinkKeyboard(int GameResult, int i)
    {
        yield return new WaitForSecondsRealtime(i * blinkFrequence);

        if (i % 2 == 0)
        {
            foreach(GameObject btn in Buttons)
            {
                btn.GetComponentInChildren<Renderer>().material = ButtonStatus[GameResult];
            }
        } else
        {
            foreach (GameObject btn in Buttons)
            {
                btn.GetComponentInChildren<Renderer>().material = ButtonStatus[0];
            }
        }
    }
}
