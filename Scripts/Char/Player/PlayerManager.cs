using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject back;
    private GameObject currentSide;

    private GameObject interactable;
    private GameObject pickUpItem;
    private int carryCap = 4;
    public List<GameObject> carryingRightNow = new List<GameObject>();

    private Vector3 currentDirection;

    private Rigidbody2D playerRB;
    private PlayerMotor playerMotor;

    private MapManager mapManager;
    private MarketManager marketManager;
    private TavernInventoryManager inventoryManager;

    public int _cash { get; private set; }
    public float moveSpeed;
    public bool canMove = true;

    public GameObject carryPos;

    // Start is called before the first frame update
    void Start()
    {
        mapManager = MapManager.instance;
        marketManager = MarketManager.instance;
        inventoryManager = TavernInventoryManager.instance;

        playerRB = gameObject.GetComponent<Rigidbody2D>();
        playerMotor = gameObject.GetComponent<PlayerMotor>();

        // subscribing to the player control events, like WASD, Interact and PickUp button pressed
        playerMotor.playerInputActions.Player.Interaction.performed += Interact;
        playerMotor.playerInputActions.Player.PickUp.performed += PickUp;
        playerMotor.playerInputActions.Player.Market.performed += OpenMarket;

        currentSide = front;
        currentDirection = -transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        ShootRaycast(); // shoot raycast in faced direction
        Move(playerMotor.GetMovementVector().normalized); // get movement direction and move player object
        if(Input.GetKeyDown(KeyCode.Space)) // if space bar is pressed start the work day
            mapManager.EndFreeTime();
    }

    private void Move(Vector2 vector)
    {
        if(!canMove)
            return;

        playerRB.velocity = vector * moveSpeed; // move player to given direction
        ChangeCharSide(vector); // change side and direction faced
    }

    private void ChangeCharSide(Vector2 vector)
    {
        if(vector.y == -1f) // S btn pressed
        {
            currentSide.SetActive(false); // hide current direction
            front.SetActive(true); // set front face of visual object
            currentSide = front;    // set curren direction to front
            currentDirection = -transform.up; // set facedd direction to down
        }
        else if(vector.y == 1f) // W btn pressed, rest are the same as above, instead of front, back face is activated
        {
            currentSide.SetActive(false);
            back.SetActive(true);
            currentSide = back;
            currentDirection = transform.up;
        }
        else if(vector.x == -1f) // D btn pressed, same as above, instead rightside is active
        {
            currentSide.SetActive(false);
            left.SetActive(true);
            currentSide = left;
            currentDirection = -transform.right;
        }
        else if(vector.x == 1f) // A btn pressed, same as above, instead leftside is active
        {
            currentSide.SetActive(false);
            right.SetActive(true);
            currentSide = right;
            currentDirection = transform.right;
        }
    }

    private void ShootRaycast() // shoot raycast in a faced direction
    {
        float rayLength = 0.5f; // raycast length

        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, rayLength); // shooting raycast

        if(hit.collider != null) // if hit
        {
            if(hit.collider.gameObject.tag == "Interactable") // set interactble obj the hit object
                interactable = hit.collider.gameObject;

            if(hit.collider.gameObject.tag == "Item") // set pickUp obj the hit object
                pickUpItem = hit.collider.gameObject;
        }
        else
        {   // else set both to null
            interactable = null;
            pickUpItem = null;
        }
    }


    private void PickUp(InputAction.CallbackContext context) // pick up btn pressed
    {
        // Player Item Pickup Logic
        if(pickUpItem == null) // if pick up item is null
        {
            if(interactable.TryGetComponent(out Interactable interactableScript)) // try to get interactable
            {
                if(carryingRightNow.Count <= 0)
                {
                    if(interactable.TryGetComponent(out CookingPod cookingPod)) // if interactable is cooking pod pick up ready meal
                        cookingPod.PickUpMeal();
                    else
                        return;
                }
                else
                {   // player carrying item use this item in interactable class (e.g., cookingpod = addingrideint, storage box = store item)
                    interactableScript.Interact(carryingRightNow[carryingRightNow.Count - 1].
                        GetComponent<ItemGameObject>().item, carryingRightNow.Count - 1);

                    return;
                }
            }

            DropCarryingItem(); // player not facing interactable drop carrying item to floor
        }
        else // if theres is pick up item, pick it up
            CarryItem();
    }

    public void CarryItem() // add item to carry list
    {
        if(carryingRightNow.Count + 1 > carryCap) // if new item + existing items amount is more than cap, dont pick up
            Debug.Log("Not Enough Room to Carry");
        else
        {
            carryingRightNow.Add(pickUpItem); // else pick up
            pickUpItem.transform.parent = carryPos.transform;
            pickUpItem.transform.position = new Vector3();
        }
    }

    public bool CarryItem(GameObject item) // first check if can carry, then carry the given item 
    {
        if(carryingRightNow.Count + 1 > carryCap)
            return false;

        carryingRightNow.Add(item);
        return true;
    }

    public void DropCarryingItem() // drop the last item from carrying list;
    {
        carryingRightNow[carryingRightNow.Count - 1].transform.parent = null;
        carryingRightNow[carryingRightNow.Count - 1].transform.position = gameObject.transform.position;
        carryingRightNow[carryingRightNow.Count - 1].GetComponent<BoxCollider2D>().enabled = true;
        carryingRightNow.Remove(carryingRightNow[carryingRightNow.Count - 1]);
    }

    private void Interact(InputAction.CallbackContext context) // interact with interactbel item
    {
        // Player Interaction Logic
        if(interactable == null)
            return;
            
        if(interactable.TryGetComponent(out Interactable interactableScript)) // if interactable has relevant script, interact with it
            interactableScript.Interact();
    }

    public void RemoveCarryingItem(int index) // Remove specific item from carrying list
    {                                           // NOTE: this function is not used for dropping items, but used
        if(carryingRightNow.Count <= index)     // adding item to interactable classes, like storage box and cooking pod
            return;
        GameObject obj = carryingRightNow[index];
        carryingRightNow.Remove(carryingRightNow[index]);
        DestroyImmediate(obj, true);
    }

    private void OpenMarket(InputAction.CallbackContext context)   // open market panel
    {
        marketManager.OpenMarketPanel();
    }
}
