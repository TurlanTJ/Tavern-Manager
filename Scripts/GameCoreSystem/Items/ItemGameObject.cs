using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGameObject : MonoBehaviour
{
    public Item item;

    public void InitItem(Item item, Transform parent)
    {
        this.item = item; // store item count class, with itemData info in it
        gameObject.GetComponent<BoxCollider2D>().enabled = false; // diable collider so that it dont cause problem when pick up
        gameObject.transform.parent = parent; // set current parent
        gameObject.transform.localPosition = Vector3.zero; // set local/relative position to center;=
    }

    public void DestroyPrefab()
    {
        DestroyImmediate(this.gameObject, true); // destroy visual object
    }
}
