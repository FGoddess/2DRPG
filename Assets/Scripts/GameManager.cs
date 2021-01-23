using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("GAMEMANAGER IS NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    [SerializeField] private StatsBar _healthSlider;

    public void SetSliderValue(int amount)
    {
        _healthSlider.SetStats(amount);
    }
}
