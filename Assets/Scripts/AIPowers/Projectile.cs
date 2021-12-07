using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    public float projectileLifeTime = 5f;

    private bool isEjected = false;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isEjected)
        {
            timer += Time.deltaTime;
            if(timer >= projectileLifeTime)
            {
                DispenserManager.instance.DestroyObject(this.gameObject);
            }
        }
    }

    public void InitializeProjectile(Vector3 ejectionDir, float ejectionForce, Vector3 pos)
    {
        this.transform.position = pos;
        Rigidbody rb;
        if(TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(ejectionDir*ejectionForce, ForceMode.Impulse);
        }
        isEjected = true;
        timer = 0f;
    }
}
