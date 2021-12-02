using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserManager : MonoBehaviour
{
    public static DispenserManager instance;
    public GameObject projectilePrefab;

    private Stack<GameObject> projectilesStack;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        projectilesStack = new Stack<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyObject(GameObject proj)
    {
        proj.SetActive(false);
        projectilesStack.Push(proj);
    }

    public GameObject InstantiateProjectile()
    {
        GameObject newProj;

        if(projectilesStack.Count > 0)
        {
            newProj = projectilesStack.Pop();
            newProj.SetActive(true);
        }
        else
        {
            newProj = Instantiate(projectilePrefab);
        }
        return newProj;
    }

}
