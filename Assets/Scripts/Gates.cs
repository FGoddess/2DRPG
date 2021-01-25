using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gates : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            player.transform.position = new Vector3(-7, -3, transform.position.z);
        }
    }
}
