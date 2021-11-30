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

    public QTE_MiniGame.Gestures OnPoseDetected(QTE_MiniGame.Gestures pose)
    {
        Debug.Log($"<color=yellow>Pose {pose.ToString()} Detected</color>");
        return pose;
    }

    public void OnPrayDetected()
    {
        OnPoseDetected(QTE_MiniGame.Gestures.Pray);
    }

    public void OnVDetected()
    {
        OnPoseDetected(QTE_MiniGame.Gestures.V);
    }

    public void OnOkDetected()
    {
        OnPoseDetected(QTE_MiniGame.Gestures.Ok);
    }

    public void OnFistDetected()
    {
        OnPoseDetected(QTE_MiniGame.Gestures.Fist);
    }
}
