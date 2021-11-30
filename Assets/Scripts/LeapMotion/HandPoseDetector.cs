using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HandPoseDetector : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IA_QTE_MiniGame.Gestures OnPoseDetected(IA_QTE_MiniGame.Gestures pose)
    {
        Debug.Log($"<color=yellow>Pose {pose.ToString()} Detected</color>");
        return pose;
    }

    public void OnPrayDetected()
    {
        OnPoseDetected(IA_QTE_MiniGame.Gestures.Pray);
    }

    public void OnVDetected()
    {
        OnPoseDetected(IA_QTE_MiniGame.Gestures.V);
    }

    public void OnOkDetected()
    {
        OnPoseDetected(IA_QTE_MiniGame.Gestures.Ok);
    }

    public void OnFistDetected()
    {
        OnPoseDetected(IA_QTE_MiniGame.Gestures.Fist);
    }
}
