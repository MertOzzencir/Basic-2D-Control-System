using System;
using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    private Player _player;
    [SerializeField] private float _attackDistance;

    bool canAttack;
    void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        Debug.Log(_player.name);
    }

    protected override void Update()
    {
        MovementDirection();
        base.Update();
    }
    protected override void AttackInput()
    {
        canAttack = Physics2D.Raycast(transform.position, transform.forward, _attackDistance, _targetLayerMask);
        if (canAttack && _canMove)
        {
            base.AttackInput();
        }
    }

    private void MovementDirection()
    {
        if(_player != null)
            movementVector = (_player.transform.position - transform.position).normalized;
    }


}
