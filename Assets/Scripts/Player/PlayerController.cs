using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float _moveSpeed = 1f;

    public PlayerControls PlayerControls { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public bool IsFacingLeft { get; private set; } = false;

    private Vector2 _movement;

    private void Awake()
    {
        Instance = this;
        PlayerControls = new PlayerControls();
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
        AdjustFacingDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        _movement = PlayerControls.Player.Move.ReadValue<Vector2>();

        Animator.SetFloat("MovementSpeed", _movement.magnitude);
    }

    private void Move()
    {
        RB.MovePosition(RB.position + _movement * (_moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustFacingDirection()
    {
        Vector2 mousePosition = PlayerControls.Player.Look.ReadValue<Vector2>();
        Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(RB.position);

        IsFacingLeft = mousePosition.x < playerScreenPoint.x;
        Renderer.flipX = IsFacingLeft;
    }
}
