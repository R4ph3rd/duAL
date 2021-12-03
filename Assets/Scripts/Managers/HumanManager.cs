using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static HumanManager instance;

    public bool isTPavalaible = true;
    private float TPDelay = 5.120f;

    public Room roomID = Room.bridge;

    public AudioSource audioSource;
    public AudioClip audioClip;
    private float timer = 0f;

    void Start()
    {
        instance = this;
        TPDelay = audioClip.length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TeleportPower()
    {
        isTPavalaible = false;
        StartCoroutine(resetTPpower());
    }

    IEnumerator resetTPpower()
    {
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSecondsRealtime(TPDelay);
        isTPavalaible = true;
    }
}
