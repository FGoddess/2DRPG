using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public static Action<bool> OnGroundCheck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (OnGroundCheck != null)
            OnGroundCheck(true);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (OnGroundCheck != null)
            OnGroundCheck(false);
    }
}
