using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gas", menuName = "Mixing/Gas")]
public class Gas : ScriptableObject
{
    [SerializeField] private Sprite logo;
    public Sprite Logo { get {  return logo; } }

    //Effects
    private int neighboringTileCalls;
    public int NeighboringTileCall {  get { return neighboringTileCalls; } set { neighboringTileCalls = value; } }
    private float speedMultiplyer;
    public float SpeedMultiplyer { get { return speedMultiplyer; } set { speedMultiplyer = value; } }
    private int rotation;
    public int Rotation { get { return rotation; } set { rotation = value; } }
}
