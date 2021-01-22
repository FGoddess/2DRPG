using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRB;
    [SerializeField] private BoxCollider2D _groundCheck;
    [SerializeField] private Animator _playerAnim;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _isOnGround;
    [SerializeField] private bool _isAttacking = false;

    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRadius;
    [SerializeField] private LayerMask _enemyLayers;

    [SerializeField] private int _damage;

    [SerializeField] private float _attackRate;
    [SerializeField] private float _nextAttackTime;

    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private bool _isDead;

    public static Action<int> OnEnemyHit;

    void Start()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        if(!_isAttacking)
            Movement(horizontalInput);
        Jump();
        Attack();

        //ПЕРЕДЕЛАТЬ
        if(_currentHealth <= 0)
        {
            _playerAnim.SetBool("IsDead", true);
            
            _isDead = true;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            _currentHealth -= 10;
            _playerAnim.SetTrigger("Hurt");
            GameManager.Instance.SetSliderValue(_currentHealth);
        }

    }

    private void Attack()
    {
        if(Input.GetMouseButtonDown(0) && Time.time >= _nextAttackTime)
        {
            _playerRB.velocity = Vector2.zero;
            StartCoroutine("AttackDelay");
            _playerAnim.SetTrigger("Attack");

            Collider2D[] hits = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _enemyLayers);

            foreach(var hit in hits)
            {
                if (OnEnemyHit != null)
                    OnEnemyHit(_damage);
            }

            _nextAttackTime = Time.time + 1f / _attackRate;
            Debug.Log(_nextAttackTime);
        }
    }

    private IEnumerator AttackDelay()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(0.8f);
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
    }

    private void OnDisable()
    {
        GroundCheck.OnGroundCheck -= SetGround;
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
