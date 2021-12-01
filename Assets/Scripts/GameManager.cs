using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Capture the flag manager
    public List<Computer> computers = new List<Computer>();

    private static GameManager _this = null;
    public static GameManager GetManager()
    {
        if (_this == null)
        {
            _this = new GameManager();
        }
        return _this;
    }

    public enum Owner
    {
        IA,
        Human,
        None
    }

    [Header("Gravity Power")]
    public GameObject[] gravitySensitiveObjects;
    public float gravityPowerImpulse = 1f;
    private bool isGravityPowerTriggered = false;

    [Header("Diskette")]
    public DisketteDispenser disketteDispenser; //UNUSED
    public DiskettePort diskettePort;
    public float hackingTime = 20f;
    private bool isPlayerHackingAIComputers = false;
    private float hackingTimer=0f;


    


    void Start()
    {
        foreach (Computer c in FindObjectsOfType<Computer>()){
            computers.Add(c);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        /*Handling the hacking of the AI computers*/
        if (isPlayerHackingAIComputers)
        {
            hackingTimer += Time.deltaTime;
            /*Case of a successfull hacking*/
            if (hackingTimer >= hackingTime)
            {
                Debug.Log("<color=green>Successfull hacking ! AI looses all its computers</color>");
                foreach (Computer c in computers)
                {
                    if(c.status == Owner.IA)
                    {
                        c.status = Owner.None;
                    }
                }
                /*Display a log on the AI interface*/
                /*Play a hacking sound ?*/
            }
            /*Case someone removed the diskette before the end of the hacking sequence*/
            else if((!diskettePort.isDisketteIn)&&(hackingTimer < hackingTime))
            {
                Debug.Log("<color=red>Diskette removed before the end of the hacking sequence</color>");
            }
        }
    }

    /// <summary>
    /// Allows the AI to stop the gravity system on the ship for xx secs
    /// </summary>
    public void TriggerGravityPower()
    {
        if (!isGravityPowerTriggered)
        {
            Debug.Log("<color=red>GRAVITY SYSTEM DISABLED</color>");
            isGravityPowerTriggered = true;
            StartCoroutine(DisableGravityRoutine());
        }
    }

    IEnumerator DisableGravityRoutine()
    {
        /*Reducing the gravity force to zero*/
        Physics.gravity = new Vector3(0, 0, 0);

        /*Giving a small impule to the objects with physics*/
        if (gravitySensitiveObjects.Length > 0)
        {
            Rigidbody rigidbody;
            foreach (GameObject obj in gravitySensitiveObjects)
            {
                if(obj.TryGetComponent<Rigidbody>(out rigidbody))
                {
                    rigidbody.AddForce(new Vector3(0, gravityPowerImpulse, 0), ForceMode.Impulse);
                    rigidbody.AddTorque(new Vector3(Random.Range(0.05f, 0.1f), Random.Range(0.05f, 0.1f), Random.Range(0.05f, 0.1f)));

                }
            }
        }

        yield return new WaitForSeconds(15f);

        /*Reactivitating the gravity*/
        Physics.gravity = new Vector3(0, -9.81f, 0);
        isGravityPowerTriggered = false;
        Debug.Log("<color=green>GRAVITY SYSTEM ENABLED</color>");
    }

    /// <summary>
    /// Allows the human to trigger an Electromagnetic impulse to disturb the AI's systems
    /// </summary>
    public void TriggerEMIPower()
    {

    }


    public void TriggerDisketteSequence()
    {
        isPlayerHackingAIComputers = true;
        hackingTimer = 0f;
    }
}
