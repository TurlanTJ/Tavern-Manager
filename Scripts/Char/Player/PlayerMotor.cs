using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public PlayerInputActions playerInputActions;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMovementVector() // return vector2 direction when wasd btn pressed
    {
        Vector2 movementVector = playerInputActions.Player.Movement.ReadValue<Vector2>().normalized;

        return movementVector;
    }
}
