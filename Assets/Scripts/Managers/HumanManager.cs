using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve;

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

    public GameObject[] forcedTPLocations;

    /*Player malus*/
    public float stunDuration = 30f;
    public float paralizedDuration = 30f;

    public VignettePostProcess vignette;

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
        print("telport power");
    }

    IEnumerator resetTPpower()
    {
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSecondsRealtime(TPDelay);
        isTPavalaible = true;
    }

    /// <summary>
    /// Used to force the teleportation of the human player to a registered location
    /// </summary>
    public void ForceTeleportation()
    {
        HumanManager.instance.audioSource.PlayOneShot(SoundManager.GetSoundManager().tpSound);
        gameObject.transform.position = forcedTPLocations[Random.Range(0, forcedTPLocations.Length - 1)].transform.position;
        StartCoroutine(resetTPpower());
    }

    public IEnumerator StunPlayer()
    {

        isTPavalaible = false;
        vignette.VignetteOn = true;
        yield return new WaitForSeconds(stunDuration);
        vignette.VignetteOn=false;
        isTPavalaible = true;
    }
}
