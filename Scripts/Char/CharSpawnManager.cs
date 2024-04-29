using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSpawnManager : MonoBehaviour
{
    public static CharSpawnManager instance;
    void Awake()
    {
        if(instance != null)
            return;
        instance = this;
    }

    [SerializeField] private List<GameObject> customerPrefabs = new List<GameObject>();

    public GameObject SpawnRandomCustomer(Vector3 spawnLoc) // spawn random character from the character list
    {
        int prefIdx = UnityEngine.Random.Range(0, customerPrefabs.Count); // random index
        GameObject spawnedChar = Instantiate(customerPrefabs[prefIdx], spawnLoc, Quaternion.identity); // spawn
        return spawnedChar;
    }
}
