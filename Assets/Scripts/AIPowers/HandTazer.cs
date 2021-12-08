using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTazer : MonoBehaviour
{
    private bool isStunningWorking = true;
    public float stunCooldown = 30f;

    [SerializeField] private RoboticArmController armController;

    private void OnTriggerEnter(Collider other)
    {
        if (isStunningWorking && other.gameObject.tag == "Player")
        {
            isStunningWorking = false;
            HumanManager.instance.audioSource.PlayOneShot(SoundManager.GetSoundManager().tazzerSound);
            HumanManager.instance.StartCoroutine(HumanManager.instance.StunPlayer());
            HumanManager.instance.StartCoroutine(StunPlayerCooldown());
        }
    }

    IEnumerator StunPlayerCooldown()
    {
        yield return new WaitForSeconds(stunCooldown);
        isStunningWorking = true;
    }
}
