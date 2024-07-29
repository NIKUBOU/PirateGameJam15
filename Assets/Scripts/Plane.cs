using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject chemTrail;
    private Rigidbody rb;
    private float currentMultipliyer = 1;
    private int currentDestroyMultiplyer = 0;
    private bool chemtrailActive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        chemtrailActive = false;
    }

    private void Update()
    {
        if (chemtrailActive)
        {
            DestroyCity();
        }
    }

    public void GoForward()
    {
        rb.velocity = transform.forward * speed * currentMultipliyer;
    }

    public void ApplyGasEffects(Gas appliedGas)
    {
        //Rotation
        this.gameObject.transform.Rotate(0, this.gameObject.transform.rotation.y + appliedGas.Rotation, 0);

        //Speed
        currentMultipliyer = appliedGas.SpeedMultiplyer;

        //Destroy Neighbors
        currentDestroyMultiplyer = appliedGas.NeighboringTileCall;

        //Apply movement
        GoForward();
    }

    public void StartChemTrail()
    {
        //Get the rotation of the trail opposite of the current forward direction
        var trailRotation = Quaternion.LookRotation(-transform.forward);

        var spawnedTrail = Instantiate(chemTrail, this.gameObject.transform.position, trailRotation);
        spawnedTrail.transform.parent = this.gameObject.transform;

        chemtrailActive = true;
    }

    private void DestroyCity()
    {
        //Find the city bellow
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile != null)
            {
                //Destroy cities
                tile.DestroyCity();

                if (currentDestroyMultiplyer == 1)
                {
                    foreach (Tile tileToDestroy in GridManager.Instance.FindNeighbors(tile))
                    {
                        tileToDestroy.DestroyCity();
                    }
                }
                else if (currentDestroyMultiplyer >= 2)
                {
                    foreach (Tile tileToDestroy in GridManager.Instance.FindNeighbors(tile))
                    {
                        tileToDestroy.DestroyCity();

                        foreach (Tile neighborTileToDestroy in GridManager.Instance.FindNeighbors(tileToDestroy))
                        {
                            neighborTileToDestroy.DestroyCity();
                        }
                    }
                }
            }
        }
    }
}
