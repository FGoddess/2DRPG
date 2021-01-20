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
    [SerializeField] public bool _isOnGround;

    void Start()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        Movement(horizontalInput);
        Jump();
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


}
