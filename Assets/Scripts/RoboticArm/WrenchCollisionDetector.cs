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
        if (isStunningWorking && HumanManager.instance.isTPavalaible && other.gameObject.tag == "Player")
        {
            isStunningWorking = false;
            HumanManager.instance.audioSource.PlayOneShot(SoundManager.GetSoundManager().impactSound);
            HumanManager.instance.StartCoroutine(HumanManager.instance.StunPlayer());
            HumanManager.instance.StartCoroutine(StunPlayerCooldown());
        }
    }

    IEnumerator StunPlayerCooldown()
    {
        yield return new WaitForSeconds(1f);
        HumanManager.instance.audioSource.PlayOneShot(SoundManager.GetSoundManager().stunSound);
        yield return new WaitForSeconds(stunCooldown);
        isStunningWorking = true;
    }

}
