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
    private bool isStunningWorking = true;
    public float stunCooldown = 30f;

    [SerializeField] private RoboticArmController armController;


    private void OnTriggerStay (Collider other)
    {
        Grabbable grabbableObject;

        if (other.gameObject.TryGetComponent<Grabbable>(out grabbableObject))
        {
            armController.Grab(grabbableObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isStunningWorking && other.gameObject.tag == "Player")
        {
            isStunningWorking = false;
        }
    }

    IEnumerator StunPlayerCooldown()
    {
        HumanManager.instance.StartCoroutine(StunPlayerCooldown());
        yield return new WaitForSeconds(stunCooldown);
        isStunningWorking = true;
    }

}
