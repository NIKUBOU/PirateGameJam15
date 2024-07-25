using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //GameManager Instance
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    [Header("Value Shortcuts")]

    [Tooltip("Controls the base speed of the planes")]
    [SerializeField] private float planeSpeed;
    public float PlaneSpeed { get { return planeSpeed; } }

    private void Awake()
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
