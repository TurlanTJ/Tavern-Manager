using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.TryGetComponent(out ICharacter character))
        {
            CharGroup group = character.gameObject.GetComponentInParent<CharGroup>();
            if(group.destination == this.gameObject)
                character.StopMovement();
        }
    }
}
