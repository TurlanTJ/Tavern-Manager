using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    void Awake()
    {
        if(instance != null)
            return;
        instance = this; 
    }

    // Map Initialization Variables
    [SerializeField] private GameObject _mapBaseVisual;
    [SerializeField] private GameObject _mapBaseParent;
    [SerializeField] private float _mapSize = 50f;

    // Gameplay Timer Variables
    [SerializeField] private float _workTime;
    [SerializeField] private float _currentTime;
    [SerializeField] private bool _isWorkTime;
    [SerializeField] private bool _isFreeTime;

    // Char/Customer Spawn VARIABLES
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerSpawnPoint;
    [SerializeField] private GameObject _customerSpawnPoint;
    [SerializeField] private float _customerGroupSpawnIntervalMin = 5f;
    [SerializeField] private float _customerGroupSpawnIntervalMax = 15f;

    private bool _isPlayerSpawned = false;

    private GameObject _mapBase;

    public Grid _grid;

    public bool isTutorialEnabled = false;

    public delegate void OnWorkTimeEnded();
    public OnWorkTimeEnded onWorkTimeEnded;
    public delegate void OnWorkTimeUpdated(float progress);
    public OnWorkTimeUpdated onWorkTimeUpdated;
    public delegate void OnTutorialStatusChanged(bool status);
    public OnTutorialStatusChanged onTutorialStatusChanged;

    private CustomerManager customerManager;

    void Start()
    {
        _isFreeTime = true;
        _isWorkTime = false;
        _currentTime = _workTime;

        customerManager = CustomerManager.instance;

        StartCoroutine(SpawnCharacter());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
            EnableTutorial(!isTutorialEnabled);

        if(isTutorialEnabled)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_isFreeTime)
                EndFreeTime();
        }
    }

    public void EnableTutorial(bool sts) // enable/disable tutorial panels
    {
        isTutorialEnabled = sts;
        onTutorialStatusChanged?.Invoke(isTutorialEnabled);
    }

    private IEnumerator SpawnCharacter()
    {
        if(!_isPlayerSpawned) // spawn player, if not spawned already
        {
            Instantiate(_playerPrefab, _playerSpawnPoint.transform.position, Quaternion.identity);
            _isPlayerSpawned = true;
        }

        while(_isWorkTime) // while work time spawn random custoemr groups with random intervals in between
        {
            float spawnInterval = UnityEngine.Random.Range(_customerGroupSpawnIntervalMin, _customerGroupSpawnIntervalMax);
            customerManager.SpawnCustomerGroups(_customerSpawnPoint.transform.position);
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator UpdateTime() // Update current time every second
    {
        while(_isWorkTime)
        {
            onWorkTimeUpdated?.Invoke(_currentTime / _workTime); // calculate the current work time left and update the clock UI

            yield return new WaitForSeconds(1f); // wait for 1 sec

            _currentTime -= 1f; // reduce timer

            if(_currentTime <= 0f) // if timer reached 0
            {
                _isWorkTime = false; // work time finished
                _currentTime = _isWorkTime ? _workTime : 0; // current time is reset
                _isFreeTime = true; // free time is activated
                onWorkTimeEnded?.Invoke();
            }
        }
    }

    public void EndFreeTime() // end free time
    {
        if(!_isFreeTime)
            return;

        Debug.Log("Work Day Started");

        _isFreeTime = false;
        _isWorkTime = true;
        _currentTime = _workTime; // work time reset
        onWorkTimeUpdated?.Invoke(_currentTime / _workTime);

        StartCoroutine(SpawnCharacter()); // spawn custoemr groups
        StartCoroutine(UpdateTime()); // update time
    }
}
