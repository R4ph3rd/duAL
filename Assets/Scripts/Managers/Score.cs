using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] ScoreBars;
    public Transform[] HumanScoreTexts; // add parent of texts
    public Transform[] IAScoreTexts; // idem
    private List<GameObject> ScoreFills = new List<GameObject>();
    private List<RectTransform> ScorePos = new List<RectTransform>();

    public Color HumanColor;
    public Color IAColor;
    public float ScoreUpdateInterval = 2;

    public float IAScore = 0;
    public float HumanScore = 0;
    private float GlobalScore = 0;

    public float TinyComputerValue = 1f;
    public float BigComputerValue = 3f;

    private static Score _this = null;
    public static Score GetScore()
    {
        if (_this == null)
        {
            _this = FindObjectOfType<Score>();
            //_this = _this == null ? new Score() : _this;
        }
        return _this;
    }

    void Start()
    {
        foreach(GameObject scorebar in ScoreBars)
        {
            GameObject scorefill = scorebar.transform.GetChild(2).GetChild(0).gameObject;
            ScoreFills.Add(scorefill);
            ScorePos.Add(scorefill.GetComponent<RectTransform>());

            scorebar.transform.GetChild(1).GetComponent<Image>().color = IAColor;
            scorefill.GetComponent<Image>().color = HumanColor;
        }

        StartCoroutine(UpdateScorePlayer());

        foreach(Transform humanText in HumanScoreTexts)
        {
            humanText.GetChild(0).GetComponent<Text>().color = HumanColor;
            humanText.GetChild(0).GetChild(0).GetComponent<Text>().color = HumanColor;
        }

        foreach (Transform iaText in IAScoreTexts)
        {
            iaText.GetChild(0).GetComponent<Text>().color = HumanColor;
            iaText.GetChild(0).GetChild(0).GetComponent<Text>().color = HumanColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreBar();
    }

    void UpdateScoreBar()
    {
        float diff = HumanScore - IAScore;
        float bestScore = HumanScore >= IAScore ? HumanScore : IAScore;

        //ScoreFill.GetComponent<Image>().color = diff > 0 ? HumanColor : IAColor;
        foreach (GameObject scorebar in ScoreBars)
        {
            scorebar.GetComponent<Slider>().value = Remap(diff, bestScore * 2, -bestScore * 2, 10, 0);
        }

        foreach (Transform humanText in HumanScoreTexts)
        {
            humanText.GetChild(0).GetChild(0).GetComponent<Text>().text = Mathf.Floor(HumanScore).ToString(); ;
        }

        foreach (Transform iaText in IAScoreTexts)
        {
            iaText.GetChild(0).GetChild(0).GetComponent<Text>().text = IAScore.ToString(); 
        }
    }

    IEnumerator UpdateScorePlayer()
    {
        yield return new WaitForSecondsRealtime(ScoreUpdateInterval);
        foreach (Computer c in FindObjectsOfType<Computer>())
        {
            switch (c.status) {
                case GameManager.Owner.IA:
                    IAScore += c.ComputerValue;
                    break;
                case GameManager.Owner.Human:
                    HumanScore += c.ComputerValue;
                    break;
                default:
                    break;
            }
        }

        StartCoroutine(UpdateScorePlayer());
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
