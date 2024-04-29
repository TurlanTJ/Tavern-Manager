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

    public delegate void OnOrderListUpdated(List<Order> newOrder);
    public OnOrderListUpdated onOrderListUpdated;

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
        onOrderListUpdated?.Invoke(activeOrders);   // call ui to update the active orders list
    }

    public void CompleteOrder(Order order)
    {
        if(!activeOrders.Contains(order))
            return;
        // complete the given order, adn remove it from active orders list, call ui to update the list
        order.orderComplete = true;
        activeOrders.Remove(order);
        onOrderListUpdated?.Invoke(activeOrders);Debug.Log(1);
    }
}
