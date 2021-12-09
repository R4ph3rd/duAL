using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class HandMovementEnhancer : MonoBehaviour
{
    public HandModelBase trackedHand;
    public Room handRoom = Room.control;

    private Hand hand;
    private Vector3 handFormerPosition;
    private float translationIncreaseFactor;

    // Start is called before the first frame update
    void Start()
    {
        hand = trackedHand.GetLeapHand();
        handFormerPosition = hand.PalmPosition.ToVector3();
    }

    // Update is called once per frame
    void Update()
    {
        if(IAManager.GetIAManager().RoomID == handRoom)
        {
            
            hand = trackedHand.GetLeapHand();

            if ((hand.IsLeft && trackedHand.Handedness == Chirality.Left) || (hand.IsRight && trackedHand.Handedness == Chirality.Right))
            {
                Vector3 handoffset = handFormerPosition - hand.PalmPosition.ToVector3();//.InLocalSpace(trackedHandModel.transform);

                handFormerPosition = hand.PalmPosition.ToVector3();

                /*Affecting each coordinate of the handOffset vector3 to the robot position*/
                transform.Translate(new Vector3(0, (-handoffset.x) * translationIncreaseFactor * 1.5f, handoffset.y * translationIncreaseFactor), Space.Self);


            }
        }

        
    }
}
