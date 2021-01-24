using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _enemyAnimator;

    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rayDistance;
    [SerializeField] private bool _isMovingLeft = true;
    [SerializeField] private Transform _groundDetection;


    private void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        transform.Translate(Vector2.left * _moveSpeed * Time.deltaTime);
        Patrolling();
    }

    private void Patrolling()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(_groundDetection.position, Vector2.down, 2f, LayerMask.GetMask("Foreground"));
        Debug.DrawRay(_groundDetection.position, Vector2.down, Color.red, 2f);
        if (groundInfo.collider == false)
        {
            if (_isMovingLeft)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                _isMovingLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                _isMovingLeft = true;
            }
        }
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
