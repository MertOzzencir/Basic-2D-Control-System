using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : Entity
{

    [SerializeField] protected float _jumpForce;
    [SerializeField] private float _jumpTimerSet;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundRayDistance;
    bool _canJump;
    private bool _isGrounded;


    protected override void Update()
    {
        MovementInput();
        JumpInput();
        base.Update();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Jump();
    }
    private void MovementInput()
    {
        movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), _rb.linearVelocity.y);

    }
    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
            _canJump = true;
    }

    protected override void AttackInput()
    {
        if (Input.GetMouseButtonDown(0) && GroundCheck())
        {
            base.TryAttack();
        }
    }

    private void Jump()
    {
        if (_canJump && _canJumpFromAttack)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
            _canJump = false;
        }


    }
    private bool GroundCheck()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _groundRayDistance, _groundMask);
        return _isGrounded;
    }


    protected override void RunningAnimationControl()
    {
        _animationController.SetBool("IsGrounded", GroundCheck());
        base.RunningAnimationControl();
    }

}
