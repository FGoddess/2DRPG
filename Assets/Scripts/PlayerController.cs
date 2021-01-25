using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _playerRB;
    [SerializeField] private BoxCollider2D _groundCheck;
    [SerializeField] private Animator _playerAnim;
    [SerializeField] private Transform _attackPoint;
    
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    [Header("Dash")]
    [SerializeField] private float _nextDashTime;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _startDashTimer;
    [SerializeField] private float _currentDashTimer;

    [Header("Booleans")]
    [SerializeField] private bool _isOnGround;
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private bool _isDashing = false;
    [SerializeField] private bool _isDead;

    [Header("Attacking")]
    [SerializeField] private int _damage;
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _attackRate;
    [SerializeField] private float _nextAttackTime;
    [SerializeField] private LayerMask _enemyLayers;

    [Header("Health")]
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;

    public static Action<int> OnEnemyHit;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");

        if (!_isAttacking)
            Movement(horizontalInput);
        if (!_isDashing)
            Attack();
        Jump();
        Dash(horizontalInput);
    }

    private void TakeDamage(int damage)
    {
        _currentHealth -= 10;
        _playerAnim.SetTrigger("Hurt");
        GameManager.Instance.SetSliderValue(_currentHealth);

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        _playerAnim.SetBool("IsDead", true);

        SceneManager.LoadScene(0);

        transform.position = new Vector3(-5,-3,0);
        _currentHealth = _maxHealth;
    }

    private void Dash(float horizontalInput)
    {
        if (Input.GetKeyDown(KeyCode.X) && !_isDashing && horizontalInput != 0)
        {
            _isDashing = true;
            _currentDashTimer = _startDashTimer;
            _playerRB.velocity = Vector2.zero;
        }

        if(_isDashing)
        {
            if (horizontalInput > 0)
            {
                _playerRB.velocity = Vector2.right * _dashForce;
            }
            else
            {
                _playerRB.velocity = Vector2.left * _dashForce;
            }

            _currentDashTimer -= Time.deltaTime;

            if (_currentDashTimer <= 0)
            {
                _isDashing = false;
            }
        }
    }

    private void Attack()
    {
        if(Input.GetMouseButtonDown(0) && Time.time >= _nextAttackTime)
        {
            _playerRB.velocity = Vector2.zero;
            StartCoroutine(AttackDelay());
            _playerAnim.SetTrigger("Attack");

            Collider2D[] hits = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _enemyLayers);

            foreach(var hit in hits)
            {
                if (OnEnemyHit != null)
                    OnEnemyHit(_damage);
            }

            _nextAttackTime = Time.time + 1f / _attackRate;
        }
    }

    private IEnumerator AttackDelay()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(0.8f); //animation time
        _isAttacking = false;
    }
    private void Movement(float horizontalInput)
    {
        _playerRB.velocity = new Vector2(horizontalInput * _moveSpeed, _playerRB.velocity.y);

        if (horizontalInput > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (Mathf.Abs(horizontalInput) > Mathf.Epsilon)
            _playerAnim.SetInteger("State", 1);
        else 
            _playerAnim.SetInteger("State", 0);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isOnGround)
        {
            _playerRB.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnEnable()
    {
        GroundCheck.OnGroundCheck += SetGround;
        Enemy.OnPlayerHit += TakeDamage;
    }

    private void OnDisable()
    {
        GroundCheck.OnGroundCheck -= SetGround;
        Enemy.OnPlayerHit -= TakeDamage;
    }

    private void SetGround(bool temp)
    {
        _isOnGround = temp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Spikes"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }

}
