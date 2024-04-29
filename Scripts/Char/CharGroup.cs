using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharGroup : MonoBehaviour
{
    private int minGroupNumber = 1;
    private int maxGroupNumber = 3;
    private int decidedGroupNumber;

    public List<GameObject> groupMembers = new List<GameObject>();
    public List<Vector3> spawnPositions = new List<Vector3>();
    public GameObject despawnPoint;

    public GameObject destination;
    public GroupStatus groupStatus;
    public Table table = null;

    private GameObject groupParent;

    private OrderManager orderManager;
    private TableManager tableManager;

    private bool despawning = false;

    public int groupId;

    public int startedConsuming = 0;
    public int finishedConsuming = 0;
    public int numberOfOrderMember = -1;
    public int reachedDestination = 0;

    public Order groupOrder = null;
    public List<Item> recievedMeals = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        orderManager = OrderManager.instance;
        tableManager = TableManager.instance;

        InitGroup();
    }

    // Update is called once per frame
    void Update()
    {
        if(finishedConsuming == numberOfOrderMember && groupStatus == GroupStatus.ConsumingOrder)
        {
            orderManager.CompleteOrder(groupOrder);
            table.tableStatus = TableStatus.Unoccupied;
            table.ResetChairs();
            table.customerGroup = null;
            table = null;
            groupStatus = GroupStatus.MovingToDestination;
            destination = despawnPoint;
            reachedDestination = 0;
            foreach(GameObject member in groupMembers)
            {
                ICharacter character = member.GetComponent<ICharacter>();
                    character.MoveToDestion(destination);
            }
        }

        if(reachedDestination == groupMembers.Count && destination == despawnPoint)
        {
            if(!despawning)
                StartCoroutine(DespawnThisGroup());
        }
    }

    public void InitGroup() // initializing group
    {
        groupStatus = GroupStatus.WaitingForTable; // set group status
        decidedGroupNumber = Mathf.RoundToInt(UnityEngine.Random.Range(minGroupNumber, maxGroupNumber + 1)); // decide group number

        Vector3 spawnPosDef = gameObject.transform.position; 

        // add spawn position for each group member;
        spawnPositions.Add(spawnPosDef);
        spawnPositions.Add(new Vector3(spawnPosDef.x + 1f, spawnPosDef.y, spawnPosDef.z));
        spawnPositions.Add(new Vector3(spawnPosDef.x - 1f, spawnPosDef.y, spawnPosDef.z));
        spawnPositions.Add(new Vector3(spawnPosDef.x + 2f, spawnPosDef.y, spawnPosDef.z));

        int spawnedChars = 0;
        while(spawnedChars < decidedGroupNumber) // spawn a group member
        {
            GameObject newChar = CharSpawnManager.instance.SpawnRandomCustomer(spawnPositions[spawnedChars]);
            newChar.transform.parent = this.gameObject.transform;
            groupMembers.Add(newChar);
            spawnedChars++;
        }
        despawnPoint = CustomerManager.instance.customerGroupDespawnPos; // set despawn point//
        StartCoroutine(SearchEmptyTable()); // start searching for empty table
    }

    // recieve order function
    public void RecieveOrder(Item order, out bool success)
    {
        success = false;
        
        foreach(Item i in groupOrder.expectedResuls) // if group order has this meal success
        {
            if(i.itemData != order.itemData)
                continue;

            success = true;
            recievedMeals.Add(order);
        }

        if(!success) // else not succes
            return;

        // if success start eating
        groupStatus = GroupStatus.ConsumingOrder; 

        foreach(GameObject member in groupMembers)
        {
            // if group member is the one who ordered s/he start eating
            if(member.GetComponent<ICharacter>().expectingMeal.itemData != order.itemData)
                continue;
            else
            {
                if(member.GetComponent<ICharacter>().recievedMeal)
                    continue;

                StartCoroutine(member.GetComponent<ICharacter>().StartConsuming());
            }
        }
    }

    // Search for empty table function
    private IEnumerator SearchEmptyTable()
    {
        bool needToSearchTable  = true;
        while(needToSearchTable)
        {
            bool foundTable = false;
            table = tableManager.FindUnoccupiedTable(out foundTable, decidedGroupNumber); // get availablt table

            if(!foundTable)
            {
                groupStatus = GroupStatus.WaitingForTable; // if table not found wait
                yield return null;
            }
            else
            {   // assign the table to group
                needToSearchTable = false;
                groupStatus = GroupStatus.MovingToDestination; // move towards table
                destination = table.gameObject;
                table.tableStatus = TableStatus.Reserved; // set table status to reserved and reservee as this group
                table.customerGroup = this;
                reachedDestination = 0;

                foreach(GameObject member in groupMembers) // Move each group member to destination (table)
                {
                    ICharacter character = member.GetComponent<ICharacter>();
                    character.MoveToDestion(destination);
                }

                break;
            }
        }

        yield return null;

        StartCoroutine(OccupyTable()); // call occupy function
    }

    private IEnumerator OccupyTable()
    {
        bool everyoneMoving = true; // if group members moving run this function
        while(everyoneMoving)
        {
            if(reachedDestination != decidedGroupNumber) // if group havent reached - wait
            {
                yield return null;
                continue;
            }
            else
            {   // everyone reached to table
                everyoneMoving = false;
                Table table = destination.GetComponent<Table>(); // get table script from destination
                bool success = true;
                table.OccupieTable(out success, this);

                if(!success) // if could not occupied the table, search again for another table
                {
                    StartCoroutine(SearchEmptyTable());
                    break;
                }

                List<RecipeSO> orderedMeals = new List<RecipeSO>(); // if successfull, each member selects meal and adds it to list

                foreach(GameObject character in groupMembers)
                {
                    RecipeSO newMeal = character.GetComponent<ICharacter>().OrderMeal();
                    if(newMeal != null)
                        orderedMeals.Add(newMeal);
                }
                numberOfOrderMember = orderedMeals.Count;
                groupOrder = new Order(orderManager.ordersList.Count, table.tableId, orderedMeals); // initialize new order
                orderManager.PlaceOrder(groupOrder); // place order

                groupStatus = GroupStatus.WaitingForOrder; // set group status

                break;
            }
        }
    }

    public IEnumerator DespawnThisGroup() // despawn group members
    {
        despawning = true;
        yield return new WaitForSeconds(2f);
        while(gameObject.transform.childCount != 0) // delete first child of group parent object until theres no one
        {
            DestroyImmediate(gameObject.transform.GetChild(0).gameObject, true);
        }
        
        if(gameObject.transform.childCount == 0) // then delete the parent object
            DestroyImmediate(this.gameObject);
    }
}

public enum GroupStatus
{
    WaitingForTable,
    WaitingForOrder,
    ConsumingOrder,
    MovingToDestination
}
