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

    public GameObject[] gravitySensitiveObjects;
    private bool isGravityPowerTriggered=false;


    void Start()
    {
        foreach (Computer c in FindObjectsOfType<Computer>()){
            computers.Add(c);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
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
                    rigidbody.AddForce(new Vector3(0, 0.1f, 0), ForceMode.Impulse);
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
}
