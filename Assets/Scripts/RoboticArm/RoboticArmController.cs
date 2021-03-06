using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
using UnityEngine.UI;

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

    public GameObject attachmentPoint;
    public Vector3 ikCalibrationPosition = new Vector3(0, 0.2f, 0.3f);

    private Hand hand;
    private Vector3 handFormerNormal;
    private Vector3 handFormerPosition;
    private Space robotSpace;
    private bool isPinching = false;
    private bool isTryingToGrab = false;
    private bool isPositionInitialized = false;
    public bool isControlEnable = true;

    private GameObject grabbedGameObject;


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
        if (isControlEnable && GameManager.GetManager().aiPlayer.RoomID == Room.storage)
        {
            
            hand = trackedHandModel.GetLeapHand();

            /*Check grab action*/
            if (isPinching && !trackedHandModel.IsTracked)
            {
                OnPinchDeactivated();
            }

            /*Update hand position*/
            if ((hand.IsLeft && trackedHandModel.Handedness == Chirality.Left) || (hand.IsRight && trackedHandModel.Handedness == Chirality.Right))
            {
                Vector3 handoffset = handFormerPosition - hand.PalmPosition.ToVector3();//.InLocalSpace(trackedHandModel.transform);

                handFormerPosition = hand.PalmPosition.ToVector3();

                /*Affecting each coordinate of the handOffset vector3 to the robot position*/
                robotRotatingBase.transform.Rotate(Vector3.up, handoffset.z * rotationIncreaseFactor, Space.World);
                robotHeadIkTarget.transform.Translate(new Vector3(0, (-handoffset.x) * translationIncreaseFactor*1.5f, handoffset.y * translationIncreaseFactor), Space.Self);
                if (!isPositionInitialized)
                {
                    robotHeadIkTarget.transform.localPosition = ikCalibrationPosition;
                    isPositionInitialized = true;
                }

                
            }
        }
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
        isTryingToGrab = true;
        while (timer <= 1f)
        {
            lowerWrenchIKTarget.transform.Translate(-Vector3.left * 0.0001f, Space.Self);
            upperWrenchIKTarget.transform.Translate(Vector3.left * 0.0001f, Space.Self);
            timer += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.2f);
        isTryingToGrab = false;

    }

    IEnumerator ReleasePinching()
    {
        float timer = 0f;
        if (grabbedGameObject != null)
        {
            StartCoroutine(Ungrab());
        }

        while (timer <= 1f)
        {
            lowerWrenchIKTarget.transform.Translate(Vector3.left * 0.0001f, Space.Self);
            upperWrenchIKTarget.transform.Translate(-Vector3.left * 0.0001f, Space.Self);
            timer += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    public void Grab(Grabbable grabbedObject)
    {
        if(grabbedGameObject == null && isTryingToGrab)
        {
            /*changing gesture image*/
            IAManager.GetIAManager().UIGestureControls[(int)Room.storage].GetComponent<UnityEngine.UI.Image>().color = Color.red;

            Debug.Log("GRABBED!");
            grabbedObject.UpdateGrabStatus(true);
            grabbedGameObject = grabbedObject.gameObject;

            /*Putting the grabbed object as a children of the lower wrench*/
            grabbedGameObject.transform.SetParent(attachmentPoint.transform);
            grabbedGameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    IEnumerator Ungrab()
    {
        /*changing gesture image*/
        IAManager.GetIAManager().UIGestureControls[(int)Room.storage].GetComponent<UnityEngine.UI.Image>().color = Color.white;

        Debug.Log("UNGRABBED!");
        /*Releasing the grabbed object*/
        yield return new WaitForSeconds(0.5f);
        if (attachmentPoint.transform.childCount > 0)
        {
            grabbedGameObject.GetComponent<Grabbable>().UpdateGrabStatus(false);
            attachmentPoint.transform.DetachChildren();
        }
        grabbedGameObject = null;

    }
}
