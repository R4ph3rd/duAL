using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Diskette : MonoBehaviour
{
    public float timeOut = 10f;
    public bool isDispensed = false;
    private float timer = 0f;
    private Rigidbody rb;

    public float coolDown = 180f;
    private bool isSpawned=true;
    public GameObject disketteSpawnLocation;
    private Renderer renderer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawned)
        {
            /*Case the Diskette is not carried*/
            if (isDispensed && (transform.parent == null || transform.parent.name == "DisketteSpawnPoint"))
            {
                Debug.Log("<color=red>disappears</color>");
                rb.useGravity = true;
                rb.isKinematic = false;
                timer += Time.deltaTime;
                if (timer >= timeOut)
                {
                    /*Resetting the diskette*/
                    StartCoroutine(ResetDiskette());
                }
            }
            else if (isDispensed && transform.parent != null)
            {
                Debug.Log("<color=red>disappears2</color>");
                rb.useGravity = false;
                rb.isKinematic = true;
                timer = 0f;
            }
            /*Case the player takes the spawned diskette*/
            else if (!isDispensed && transform.parent != disketteSpawnLocation.transform)
            {
                Debug.Log("<color=red>dispensed</color>");
                isDispensed = true;
            }
        }
    }

    /// <summary>
    /// Reset the diskette position if it is destroyed or after a successfull hacking
    /// </summary>
    public IEnumerator ResetDiskette()
    {
        /*Disabling the gameObject*/
        transform.parent = disketteSpawnLocation.transform;

        transform.rotation = Quaternion.identity;
        transform.localPosition = new Vector3(0, 0, 0);
        renderer.enabled = false;
        isDispensed = false;
        isSpawned = false;

        print("timer begin");

        yield return new WaitForSeconds(coolDown);

        print("timer end");

        //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        isSpawned = true;
        renderer.enabled = true;
        timer = 0f;

    }
}
