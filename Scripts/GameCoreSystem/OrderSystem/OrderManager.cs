using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    void Awake()
    {
        if(instance != null)
            return;
        instance = this;
    }

    public List<RecipeSO> availableRecipes = new List<RecipeSO>();
    public List<Order> ordersList = new List<Order>();
    public List<Order> activeOrders = new List<Order>();

    public delegate void OnOrderPlaced(Order newOrder);
    public OnOrderPlaced onOrderPlaced;
    public delegate void OnOrderCompleted(Order order);
    public OnOrderCompleted onOrderCompleted;

    private CookingManager cookingManager;

    void Start()
    {
        cookingManager = CookingManager.instance;
        availableRecipes = cookingManager.GetAvailableRecipes();
    }

    public void PlaceOrder(Order newOrder) // place order
    {
        ordersList.Add(newOrder); // add new order to all orders list
        activeOrders.Add(newOrder); // add it to active orders, too
        onOrderPlaced?.Invoke(newOrder);   // call ui to update the active orders list
    }

    public void CompleteOrder(Order order)
    {
        if(!activeOrders.Contains(order))
            return;
        // complete the given order, adn remove it from active orders list, call ui to update the list
        order.orderComplete = true;
        activeOrders.Remove(order);
        onOrderCompleted?.Invoke(order);
    }
}
