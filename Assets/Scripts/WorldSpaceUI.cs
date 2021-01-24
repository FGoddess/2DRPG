using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceUI : MonoBehaviour
{
    [SerializeField] private Canvas _worldSpaceUICanvas;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _worldSpaceUICanvas.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _worldSpaceUICanvas.gameObject.SetActive(false);
    }
}
