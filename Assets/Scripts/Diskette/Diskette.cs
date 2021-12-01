using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diskette : MonoBehaviour
{
    public float timeOut = 10f;
    public bool isDispensed = false;
    private float timer = 0f;
    private Rigidbody rigidbody;


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
            rigidbody.isKinematic = true;
            timer = 0f;
        }
    }

    /// <summary>
    /// Reset the diskette position if it is destroyed or after a successfull hacking
    /// </summary>
    public void ResetDiskette()
    {
        /*Disabling the gameObject*/
        gameObject.SetActive(false);

        /**/
        //GameManager.GetManager().disketteDispenser.DestroyDiskette();
    }
}
