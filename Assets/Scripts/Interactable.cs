using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;

    public bool isFocus = false;
    Transform player;

    bool hasInteracted = false;
    public virtual void Interact () //virtual memungkinkan agar each Interaction itu berbeda.
    {
        //meant to be overwritten.
        // Debug.Log("Interacting with " + transform.name);
    }

    void Update ()
    {
        if (isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius) //waktu dia sudah masuk radius kuning maka interact. Tapi harus interact cara nyambunginnya gimana.
            {   
                Debug.Log("Interact");
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused (Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDeFocused ()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
