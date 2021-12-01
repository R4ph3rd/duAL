using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

/* MUST BE ATTACHED TO AN OBJECT WITH Computer COMPONENT */

public class InteractableKeyboard_MiniGame : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> Buttons = new List<GameObject>();
    public GameObject HumanMiniGameUI;

    public Slider progressionBar;
    public Text RemainingSteps;
    public Material[] ButtonStatus = new Material[4]; // 0 : neutral | 1 : to push | 2 : success | 3 : wrong 
    //public Sprite[] LedStatus = new Sprite[3];

    private GameObject NextKey = null;
    private int InitialNumberOfSteps;
    private int NumberOfSteps;
    private bool WaitingForKey = false;
    private int StepSuccess = 0;

    public float ErrorMargin = .5f;
    public float minValueToWin = .5f;
    public float StepDelay = 1f;
    public int maxSteps = 20;
    public int blinkRepeat = 6;
    public float blinkFrequence = .3f;

    private SoundManager sm;
    private AudioSource source;

    void Start()
    {
        sm = SoundManager.GetSoundManager();
        source = GetComponent<AudioSource>();

        Transform[] buttons = GetComponentsInChildren<Transform>();

        foreach(Transform button in buttons)
        {
            if (button.gameObject.name.Contains("Push button"))
            {
                Buttons.Add(button.gameObject);

            }
        }
    }


    public void StartMiniGame(GameObject PlaypauseBtn)
    {
        Debug.Log("start mini game ");
        PlaypauseBtn.GetComponentInChildren<Renderer>().material = ButtonStatus[1];
        HumanMiniGameUI.SetActive(true);
        ResetUI();
        resetStatus();
        StartCoroutine(GenerateStep());
    }

    public void ButtonPushed(GameObject btn)
    {
        if (btn.name == NextKey.name)
        {
            StepSuccess++;
            print("great ! score : " + StepSuccess);
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
    }

    void ResetUI()
    {
        foreach (GameObject btn in Buttons)
        {
            btn.GetComponentInChildren<Renderer>().material = ButtonStatus[0];
        }
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
        if (WaitingForKey) MissStepFX();
        if (NumberOfSteps > 0)
        {
            StartCoroutine(GenerateStep());

            if (NextKey != null)
            {
                NextKey.GetComponentInChildren<Renderer>().material = ButtonStatus[0];
            }

            NextKey = Buttons[Random.Range(0, Buttons.Count)];
            StartCoroutine(FadeNextStep());
            NumberOfSteps--;
            RemainingSteps.text = NumberOfSteps.ToString();
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
        if (((float)StepSuccess / (float)InitialNumberOfSteps) > minValueToWin)
        {
            Debug.Log("-- QTE mini game success -- ");
            GetComponent<Computer>().CaptureComputer(GameManager.Owner.Human);
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
    }

    IEnumerator FadeQTEUI()
    {
        yield return new WaitForSecondsRealtime((blinkFrequence * blinkRepeat) + 1);
        HumanMiniGameUI.SetActive(false);
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
