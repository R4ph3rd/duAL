using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTheFlag : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gm;
    void Start()
    {
        gm = GameManager.GetManager();
    }

    void WinComputer(GameManager.Owner owner)
    {

    }
}
