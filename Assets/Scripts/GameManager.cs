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
}
