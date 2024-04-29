using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{   // Interactable Interface
    public virtual void Interact(){}
    public virtual void Interact(Item item, int itemIdx){}
}
