using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class RoboticArmController : MonoBehaviour
{
    [Tooltip("The hand model to track")]
    public HandModelBase trackedHandModel;

    [Tooltip("The ik target that controls the robot's head")]
    public GameObject robotHeadIkTarget;

    [Tooltip("The ik target that controls the robot's head")]
    public GameObject robotRotatingBase;

    public float rotationIncreaseFactor = 650f;
    public float translationIncreaseFactor = 2f;

    public GameObject upperWrenchIKTarget;
    public GameObject lowerWrenchIKTarget;

    private Hand hand;
    private Vector3 handFormerNormal;
    private Vector3 handFormerPosition;
    private Space robotSpace;
    private bool isPinching = false;

    // Start is called before the first frame update
    void Start()
    {
        hand = trackedHandModel.GetLeapHand();

        /*Robot calibration wrt the hand position*/
        handFormerPosition = hand.PalmPosition.ToVector3();
        handFormerNormal = hand.PalmNormal.ToVector3();

    }

    // Update is called once per frame
    void Update()
    {
        hand = trackedHandModel.GetLeapHand();

        Vector3 handoffset = handFormerPosition - hand.PalmPosition.ToVector3();
        
        handFormerPosition = hand.PalmPosition.ToVector3();

        /*Affecting each coordinate of the handOffset vector3 to the robot position*/
        robotRotatingBase.transform.Rotate(Vector3.up, handoffset.x*rotationIncreaseFactor,Space.World);
        robotHeadIkTarget.transform.Translate(new Vector3(0, -handoffset.z * translationIncreaseFactor, handoffset.y * translationIncreaseFactor), Space.Self);
    }

    public void OnPinchDetected()
    {
        Debug.Log("Pinch Detected");
        if (!isPinching)
        {
            isPinching = true;
            StartCoroutine(PinchingInteraction());
        }
    }

    public void OnPinchDeactivated()
    {
        Debug.Log("Pinch Released");
        if (isPinching)
        {
            isPinching = false;
            /*DropItem*/
            StartCoroutine(ReleasePinching());
        }
    }

    IEnumerator PinchingInteraction()
    {


        float timer = 0f;
        while (timer <= 1f)
        {
            lowerWrenchIKTarget.transform.Translate(-Vector3.left * 0.0001f, Space.Self);
            upperWrenchIKTarget.transform.Translate(Vector3.left * 0.0001f, Space.Self);
            timer += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

    }

    IEnumerator ReleasePinching()
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            lowerWrenchIKTarget.transform.Translate(Vector3.left * 0.0001f, Space.Self);
            upperWrenchIKTarget.transform.Translate(-Vector3.left * 0.0001f, Space.Self);
            timer += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
