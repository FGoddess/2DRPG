using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    public void SetStats(int amount)
    {
        _slider.value = amount;
    }
}
