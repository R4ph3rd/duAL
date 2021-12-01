using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HandPoseDetector : MonoBehaviour
{
    public static HandPoseDetector instance;

    public delegate void OnHandPoseDelegate();
    public static event OnHandPoseDelegate newPoseEvent;
    public IA_QTE_MiniGame.Gestures lastRecordedPose= IA_QTE_MiniGame.Gestures.None;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"Last recorded = {lastRecordedPose}");
    }

    public void OnPoseDetected(IA_QTE_MiniGame.Gestures pose)
    {
        Debug.Log($"<color=yellow>Pose {pose.ToString()} Detected</color>");
        lastRecordedPose = pose;
        newPoseEvent.Invoke();
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

    public void test() { Debug.Log("Event INVOKED"); }
}
