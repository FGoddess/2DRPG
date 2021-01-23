using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string[] _npcsDialogs;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E) && !DialogManager.Inctance.DialogBox.activeInHierarchy)
            {
                DialogManager.Inctance.StartDialog(_npcsDialogs);
            }
        }
    }
}
