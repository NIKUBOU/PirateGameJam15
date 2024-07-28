using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    //Holds the plane prefab
    [SerializeField] private GameObject plane;

    //Spawns a plane on top of the spawner when called
    public void SpawnPlane()
    {
        //Edits the spawn position
        var planeSpawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);

        //Spawns the plane
        var spawnedPlane = Instantiate(plane, planeSpawnPosition, this.gameObject.transform.rotation);
        spawnedPlane.name = $"Plane {this.gameObject.name}";

        //Grabs the necessary components to make the plane go forward
        var spRB = spawnedPlane.GetComponent<Rigidbody>();
        spRB.useGravity = false;

        //Makes the plane go forward
        spawnedPlane.GetComponent<Plane>().GoForward();
    }


    private void OnTriggerEnter(Collider other)
    {
        // Try to get the Plane component
        if (other.TryGetComponent(out Plane plane))
        {
            // Check if the ID does not match
            if (other.name != $"Plane {this.gameObject.name}")
            {
                Destroy(other.gameObject);
            }
        }
    }
}
