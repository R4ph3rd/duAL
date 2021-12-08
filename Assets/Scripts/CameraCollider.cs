using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraCollider : MonoBehaviour
{
    public float blurDuration = 20f;
    public float powerCooldown = 30f;

    private bool isBreakable=true;
    public Room roomID;
    public ParticleSystem smokeParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isBreakable)
        {
            isBreakable = false;
            StartCoroutine(TriggerBlurSequence());
        }
    }

    IEnumerator TriggerBlurSequence()
    {
        smokeParticleSystem.Play();
        IAManager.GetIAManager().UpdateCamStatus(roomID, false);
        yield return new WaitForSeconds(blurDuration);
        smokeParticleSystem.Stop();
        IAManager.GetIAManager().UpdateCamStatus(roomID, true);
        yield return new WaitForSeconds(powerCooldown);
        isBreakable = true;
    }
}
