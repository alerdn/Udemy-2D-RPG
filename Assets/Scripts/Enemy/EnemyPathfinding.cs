using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;

    public Rigidbody2D RB { get; private set; }

    private Vector2 _movement;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetMovement(Vector2 movement)
    {
        _movement = movement;
    }

    public void Move()
    {
        RB.MovePosition(RB.position + _movement * (_moveSpeed * Time.fixedDeltaTime));
    }
}
