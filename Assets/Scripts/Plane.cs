using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] private GameObject chemTrail;
    private Rigidbody rb;
    private float currentMultipliyer = 1;
    public float CurrentMultipliyer { set { currentMultipliyer = value; } }
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
        rb.velocity = transform.forward * GameManager.Instance.PlaneSpeed * currentMultipliyer;
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

        if (Physics.Raycast(ray, out hit, 2f))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile != null)
            {
                //Destroy city
                tile.DestroyCity();
            }
        }
    }
}
