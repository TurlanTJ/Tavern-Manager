using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ICharacter : MonoBehaviour
{
    private int minMoney = 100;
    private int maxMoney = 5000;
    private bool isMoving = false;

    private GameObject currentDestination = null;
    private NavMeshAgent charAgent;
    public Item expectingMeal = null;
    public bool recievedMeal = false;

    public int money;

    void Awake()
    {
        charAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>(); // set up navmesh agent
        charAgent.updateRotation = false; // make it so it doesnt rotate when moving
        charAgent.updateUpAxis = false;
        money = UnityEngine.Random.Range(minMoney, maxMoney + 1); // generate char money
    }

    public void HasReachedDestination() // notify parent CharGroup when reached destination
    {
        CharGroup parentGroup = gameObject.GetComponentInParent<CharGroup>();
        parentGroup.reachedDestination++;
    }

    public void MoveToDestion(GameObject dest) // move character
    {
        currentDestination = dest; // set new destination
        charAgent.SetDestination(currentDestination.transform.position);
        isMoving = true;
    }

    public void StopMovement()
    {
        if(currentDestination.TryGetComponent(out Table table)) // if reach table, sit at chair
            table.SitAtChair(this);

        currentDestination = null; // remove current desintaion
        charAgent.SetDestination(gameObject.transform.position); // stop navmesh agent at current position
        isMoving = false;
        HasReachedDestination(); // notify parent CharGroup class
    }

    public RecipeSO OrderMeal() // order meal
    {
        List<RecipeSO> recipes = OrderManager.instance.availableRecipes; // get available meals list
        int i = 0;
        RecipeSO selection = null;
        while(i < recipes.Count)
        {
            i++;
        
            int selectedMealIdx = UnityEngine.Random.Range(0, recipes.Count); // choose random one

            selection = recipes[selectedMealIdx];
        
            if(selection.result.itemPrice <= money) // if can afford
            {   
                expectingMeal = new Item(selection.result); // add the meal to group order list
                return selection;
            }
        }

        return selection;
    }

    public IEnumerator StartConsuming() // start consuming delivered meal
    {
        recievedMeal = true;
        GetComponentInParent<CharGroup>().startedConsuming++; // notify parent class about current status
        yield return new WaitForSeconds(2f); // consume for 2 seconds (here can added things like animation and etc.)
        GetComponentInParent<CharGroup>().finishedConsuming++; // notify about finishing the meal
        money -= expectingMeal.itemData.itemPrice; // pay for the meal
        TavernInventoryManager.instance.UpdateMoney(expectingMeal.itemData.itemPrice); // player recieve money
    }

    private void OnTriggerEnter2D(Collider2D col) // if char reached table
    {
        Table triggeredTable;
        if(!isMoving || !col.gameObject.TryGetComponent(out triggeredTable)) // check if triggered table is ours
            return; // if not simple dont do anything

        if(currentDestination.TryGetComponent(out Table table)) // else stop moving
        {
            if(triggeredTable.tableId == table.tableId)
                StopMovement();
        }
    }
}

public enum CharGender // not used rought now, can be used in the future for a specific char generation
{
    Male,
    Female,
}
