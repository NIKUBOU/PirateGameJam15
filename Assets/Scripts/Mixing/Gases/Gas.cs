using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gas", menuName = "Mixing/Gas")]
public class Gas : ScriptableObject
{
    [SerializeField] private Sprite logo;
    [SerializeField] private float speedMultipliyer;
    public float SpeedMultipliyer { get { return speedMultipliyer; } }

    public Sprite GetSprite()
    {
        return logo;
    }
}
