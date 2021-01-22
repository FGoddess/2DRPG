using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;

    [SerializeField] private Animator _enemyAnimator;

    private void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
    }

    private void OnEnable()
    {
        PlayerController.OnEnemyHit += TakeDamage;
    }

    private void OnDisable()
    {
        PlayerController.OnEnemyHit -= TakeDamage;
    }

    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _enemyAnimator.SetTrigger("Hurt");
        if (_currentHealth <= 0)
        {
            _enemyAnimator.SetBool("IsDead", true);
            Destroy(this.gameObject, 2f);
        }
    }
}
