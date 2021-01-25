using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _enemyAnimator;
    [SerializeField] private Rigidbody2D _enemyRB;
    [SerializeField] private Transform _groundDetection;

    [Header("Health")]
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;

    [Header("Moving")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _isMovingLeft = true;

    [Header("Knockback")]
    [SerializeField] private float _xKnockback;
    [SerializeField] private float _yKnockback;

    [Header("")]
    [SerializeField] private int _damage;
    [SerializeField] private float _rayDistance;

    public static Action<int> OnPlayerHit;


    private void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyRB = GetComponent<Rigidbody2D>();
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
        _enemyRB.AddForce(new Vector2(_xKnockback, _yKnockback), ForceMode2D.Impulse);

        if (_currentHealth <= 0)
        {
            _enemyAnimator.SetBool("IsDead", true);
            Destroy(this.gameObject, 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("da");
            if(OnPlayerHit != null)
            {
                OnPlayerHit(_damage);
            }
        }
    }
}
