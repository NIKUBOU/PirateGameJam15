using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    //Holds the plane prefab
    [SerializeField] private GameObject plane;

    private GameManager gM;

    public void SpawnPlane()
    {
        //Edits the spawn position
        var planeSpawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);

        //Spawns the plane
        var spawnedPlane = Instantiate(plane, planeSpawnPosition, this.gameObject.transform.rotation);

        //Grabs the necessary components to make the plane go forward
        var spRB = spawnedPlane.GetComponent<Rigidbody>();
        spRB.useGravity = false;

        //Makes the plane go forward
        spRB.velocity = spawnedPlane.transform.forward * GameManager.Instance.PlaneSpeed;
    }
}
