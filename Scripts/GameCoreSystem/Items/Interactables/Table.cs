using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Interactable
{
    public int tableId;
    public int tableCap; // max number of customer who can sit
    public TableStatus tableStatus;

    public CharGroup customerGroup; // occupied/reserved group

    //private OrderSystem orderSystem;
    private TableManager tableManager;

    private bool isAddedToList = false;

    [SerializeField] private List<GameObject> chairs = new List<GameObject>();
    private List<bool> chairSts = new List<bool>();
    private List<GameObject> availableChairs = new List<GameObject>();

    void Start()
    {
        //orderSystem = OrderSystem.instance;
        tableManager = TableManager.instance;

        // init chair statuses
        var i = 0;
        while(i < tableCap)
        {
            chairSts.Add(true);
            i++;
        }

        availableChairs = chairs;
    }

    void Update()
    {   // if table is placed to the scene, add it to available tables list
        if(tableStatus != TableStatus.Stored && !isAddedToList)
        {
            isAddedToList = true;
            tableManager.availableTables.Add(this);
        }

        // if the table is stored, then remove it from available tables list
        if(tableStatus == TableStatus.Stored && isAddedToList)
        {
            isAddedToList = false;
            tableManager.availableTables.Remove(this);
        }
    }

    public override void Interact()
    {

    }

    // Deliver the ordered meal to table
    public override void Interact(Item item, int itemIdx)
    {
        if(customerGroup == null)
            return;
        bool success;
        customerGroup.RecieveOrder(item, out success); // Deliver order
        if(success) //if this table is the one who ordered remove item from carrying list
        {
            PlayerManager playerManager = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
            playerManager.RemoveCarryingItem(itemIdx);
        }
    }

    // Occupy table for group of customers
    public void OccupieTable(out bool success, CharGroup group)
    {
        success = true;
        
        // if occupied terminate func
        if(tableStatus == TableStatus.Occupied)
        {
            success = false;
            return;
        } // if reserved by the given group, set table status to occupied
        else if(tableStatus == TableStatus.Reserved && group.groupId == customerGroup.groupId)
        {
            tableStatus = TableStatus.Occupied;
        }
    }

    //Place Customer at chair loc
    public void SitAtChair(ICharacter customer)
    {
        // Selecting a random chair
        int randomChair = UnityEngine.Random.Range(0, availableChairs.Count); 

        // if chair is Unoccupied customer is sits there, else functions calls itself until char finds a chair
        if(availableChairs[randomChair]) 
        {
            customer.gameObject.transform.position = availableChairs[randomChair].transform.position;
            availableChairs.Remove(availableChairs[randomChair]);
            chairSts[randomChair] = false;
        }
        else
            SitAtChair(customer);
    }

    public void ResetChairs()
    {
        availableChairs = chairs;
        var i = 0;
        while(i < availableChairs.Count)
        {
            i++;
            chairSts[i] = true;
        }
    }
}

public enum TableStatus
{
    Stored,
    Reserved,
    Occupied,
    OrderWaiting,
    Consuming,
    Unoccupied
}
