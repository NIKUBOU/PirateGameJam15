using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Mixing/Ingredient")]
public class Ingredient : ScriptableObject
{
    [SerializeField] private Sprite logo;
    public Sprite Logo { get { return logo; } }

    [Header("Effects")]

    [Tooltip("1 = 3x3, 2 = 5x5, 2 is the max tho")]
    [SerializeField] private int neighboringTileCalls;
    public int NeighboringTileCalls { get { return neighboringTileCalls; } }

    [Tooltip("If you don't want to affect speed, leave it at 1. 0 will make it stop")]
    [SerializeField] private float speedMultiplyer = 1;
    public float SpeedMultiplyer { get { return speedMultiplyer; } }

    [SerializeField] private int rotation;
    public int Rotation { get { return rotation; } }
}
