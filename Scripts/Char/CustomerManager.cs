using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;
    void Awake()
    {
        if(instance != null)
            return;

        instance = this;
    }

    public List<CharGroup> customerGroupsInScene = new List<CharGroup>(); //groups in scene

    public GameObject customerGroupParent;
    public GameObject customerGroupSpawnPos;
    public GameObject customerGroupDespawnPos;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) // spawn new group when G pressed
            SpawnCustomerGroups(customerGroupSpawnPos.transform.position);
    }

    public void SpawnCustomerGroups(Vector3 spawnLoc)
    {
        GameObject group = Instantiate(customerGroupParent, spawnLoc, Quaternion.identity); // spawn new group
    }
}
