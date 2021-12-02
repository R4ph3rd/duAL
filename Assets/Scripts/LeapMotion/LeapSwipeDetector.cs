using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
using UnityEngine.Events;

public class LeapSwipeDetector : Detector
{
    public enum SwipingDirection { left, right, down, up};

    public HandModelBase handModel = null;
    public SwipingDirection swipeDir = SwipingDirection.left;
    private Vector3 swipeVector;
    public float swipeDist = 0.05f;
    public float swipeDetectTime = 2f;
    private PalmDirectionDetector palmDirDetect;

    public bool isSwiping = false;
    private Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        swipeVector = GetSwipingVector(swipeDir);
        if (handModel!= null)
        {
            /*On initialise le détecteur de direction de la main*/
            palmDirDetect = gameObject.AddComponent<PalmDirectionDetector>();
            palmDirDetect.HandModel = handModel;
            palmDirDetect.PointingDirection = swipeVector;
            palmDirDetect.PointingType = PointingType.RelativeToWorld;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (palmDirDetect.IsActive)
        {
            StartCoroutine(CheckSwipe());
        }
    }

    IEnumerator CheckSwipe()
    {
        hand = handModel.GetLeapHand();
        if ((hand.IsLeft && handModel.Handedness == Chirality.Left) || (hand.IsRight && handModel.Handedness == Chirality.Right)) 
        {
            Vector3 initHandPos = hand.PalmPosition.ToVector3();
            yield return new WaitForSeconds(swipeDetectTime);

            //Vector3 crossProdInit = Vector3.Project(initHandPos, swipeVector);
            //Vector3 crossProdEnd = Vector3.Project(hand.PalmPosition.ToVector3(), swipeVector);
            float dist = Vector3.Dot(hand.PalmPosition.ToVector3() - initHandPos, swipeVector);

            if (handModel.IsTracked && dist >= swipeDist && !isSwiping)
            {
                isSwiping = true;
                OnActivate.Invoke();
                Debug.Log($"<color=yellow>Swiped {swipeDir.ToString()}</color>");
                yield return new WaitForSeconds(1f);
                isSwiping = false;
            }
        }
    }

    //public UnityEvent OnActivate;

    private Vector3 GetSwipingVector(SwipingDirection dir)
    {
        switch(dir)
        {
            case SwipingDirection.left:
                return new Vector3(-1,0,0);
            case SwipingDirection.right:
                return new Vector3(1, 0, 0);
            case SwipingDirection.up:
                return new Vector3(0, 1, 0);
            case SwipingDirection.down:
                return new Vector3(0, -1, 0);
            default:
                return new Vector3(0,0,0);
        }
    }
}
