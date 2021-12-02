using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isTPavalaible = true;
    public float TPDelay = 5.120f;

    public Room roomID = Room.bridge;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TeleportPower()
    {
        isTPavalaible = false;
        StartCoroutine(resetTPpower());
    }

    IEnumerator resetTPpower()
    {
        yield return new WaitForSecondsRealtime(TPDelay);
        isTPavalaible = true;
    }
}
