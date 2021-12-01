using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to a wrench, detects the collision with a grabbable object in the range of the triggered collider
/// </summary>
/// 
[RequireComponent(typeof(SphereCollider))]
public class WrenchCollisionDetector : MonoBehaviour
{
    [SerializeField] private RoboticArmController armController;


    private void OnTriggerStay (Collider other)
    {
        Grabbable grabbableObject;

        if (other.gameObject.TryGetComponent<Grabbable>(out grabbableObject))
        {
            armController.Grab(grabbableObject);
        }
    }


}
