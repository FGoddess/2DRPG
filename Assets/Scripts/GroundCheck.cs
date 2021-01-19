using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponentInParent<PlayerController>()._isOnGround = true;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.GetComponentInParent<PlayerController>()._isOnGround = false;
    }
}
