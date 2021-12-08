using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static HumanManager instance;

    public Vector3 initPos;
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

    void Start()
    {
        instance = this;
        TPDelay = audioClip.length;

        /*Reset Cam*/
        //UnityEngine.VR.InputTracking.Recenter();
        Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
        transform.position = initPos;
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
        gameObject.transform.position = forcedTPLocations[Random.Range(0, forcedTPLocations.Length - 1)].transform.position;
        StartCoroutine(resetTPpower());
    }

    IEnumerator StunPlayer()
    {
        isTPavalaible = false;
        yield return new WaitForSeconds(stunDuration);
        isTPavalaible = true;
    }
}
