using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] protected float _speed;
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected int _attackPower;
    [SerializeField] protected Animator _animationController;
    [SerializeField] protected Transform _attackPoint;
    [SerializeField] protected LayerMask _targetLayerMask;
    [SerializeField] protected float _attackRadius;
    [SerializeField] protected Material _damageFeedBackMaterial;
    [SerializeField] protected float _damageFeedBackTimer;

    protected SpriteRenderer sp;
    protected Rigidbody2D _rb;
    protected Vector2 movementVector;
    protected bool _isFacingRight;
    protected bool _canMove;
    protected bool _canJumpFromAttack;
    protected float _currentHealth;
    protected Collider2D col;


    void Start()
    {
        col = GetComponent<Collider2D>();
        _canMove = true;
        _canJumpFromAttack = true;
        _rb = GetComponent<Rigidbody2D>();
        _isFacingRight = true;
        _currentHealth = _maxHealth;
        sp = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        AttackInput();
        HandleFlip();
        RunningAnimationControl();
    }
     protected virtual void FixedUpdate()
    {
        Movement();
    }

    public void AttackStateChanging(bool enable)
    {
        _canMove = enable;
        _canJumpFromAttack = enable;
    }

    protected virtual void AttackInput()
    {
        TryAttack();
    }

    protected void TryAttack()
    {
        _animationController.SetTrigger("attack");
    }
    public virtual void AttackLogic()
    {
        List<Collider2D> enemyLists = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _targetLayerMask).ToList();
        foreach (Collider2D a in enemyLists)
        {
            if (a.GetComponent<Entity>() != null)
                a.GetComponent<Entity>().GetDamage(_attackPower);
        }
    }

    private void GetDamage(int damage)
    {
        _currentHealth -= damage;
        StartCoroutine(DamageFeedBackCoroutine());
        Debug.Log("Entity-" + transform.name + " Get Damaged" + "Health: " + _currentHealth);
        if (_currentHealth <= 0)
            Die();        
    }

    protected virtual void Die()
    {
        _animationController.enabled = false;
        col.enabled = false;
        _rb.gravityScale = 12f;
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 20);
        Destroy(gameObject, 3f);

    }

    IEnumerator DamageFeedBackCoroutine()
    {
        Material originalMaterial = sp.material;
        sp.material = _damageFeedBackMaterial;
        yield return new WaitForSeconds(_damageFeedBackTimer);
        sp.material = originalMaterial;
    }

    

   

    protected virtual void RunningAnimationControl()
    {
        _animationController.SetFloat("xVelocity", _rb.linearVelocity.x);
        _animationController.SetFloat("yVelocity", _rb.linearVelocity.y);
    }

    private void Movement()
    {
        if (_canMove)
            _rb.linearVelocity = new Vector2(movementVector.x * _speed, _rb.linearVelocity.y);
        else
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
    }

    private void HandleFlip()
    {
        if (movementVector.x > 0 && _isFacingRight == false)
        {
            Flip();

        }
        else if (movementVector.x < 0 && _isFacingRight == true)
        {
            Flip();

        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        _isFacingRight = !_isFacingRight;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }

}