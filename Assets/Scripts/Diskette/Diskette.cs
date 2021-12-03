using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diskette : MonoBehaviour
{
    public float timeOut = 10f;
    public bool isDispensed = false;
    private float timer = 0f;
    private Rigidbody rigidbody;

    public float coolDown = 180f;
    private bool isSpawned;
    public Vector3 disketteSpawnLocation;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Case the Diskette is not carried*/
        if (isDispensed && transform.parent == null)
        {
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            timer += Time.deltaTime;
            if (timer >= timeOut)
            {
                /*Resetting the diskette*/
                ResetDiskette();
            }
        }
        else if(isDispensed && transform.parent != null)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            timer = 0f;
        }
        /*Case the player takes the spawned diskette*/
        else if (!isDispensed && transform.parent != null)
        {
            isDispensed = true;
        }

        if (!isSpawned)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                print("timer cool " + timer);
                isSpawned = true;
                gameObject.SetActive(true);
                transform.rotation = Quaternion.identity;
                transform.position = disketteSpawnLocation;
            }
        }
    }

    /// <summary>
    /// Reset the diskette position if it is destroyed or after a successfull hacking
    /// </summary>
    public void ResetDiskette()
    {
        /*Disabling the gameObject*/
        gameObject.transform.parent = null;
        gameObject.SetActive(false);
        isDispensed = false;
        isSpawned = false;
    }
}
