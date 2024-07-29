using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //GameManager Instance
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        CreateInstance();
    }

    //Creates an instance of this manager
    private void CreateInstance()
    {
        //Instance stuff
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

}
