using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public float ejectionForce = 10f;
    public Vector3 ejectionDir = new Vector3(0, 0, 0);

    public void LaunchProjectile()
    {
        if(GameManager.GetManager().aiPlayer.RoomID == Room.bridge)
        {
            GameObject projectile = DispenserManager.instance.InstantiateProjectile();
            Projectile proj;
            if (projectile.TryGetComponent<Projectile>(out proj))
            {
                proj.InitializeProjectile(ejectionDir, ejectionForce, this.transform.position);
            }
        }
        
    }

    
}
