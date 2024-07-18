using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _dashSpeed = 4f;
    [SerializeField] private float _dashDuration = .2f;

    public PlayerControls Controls { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public bool IsFacingLeft { get; private set; } = false;

    private Vector2 _movement;
    private Coroutine _dashRoutine;

    private void OnEnable()
    {
        Controls ??= new PlayerControls();
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls?.Disable();
    }

    private void Awake()
    {
        Instance = this;
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Controls.Combat.Dash.performed += Dash;
    }

    private void OnDestroy()
    {
        Controls.Combat.Dash.performed -= Dash;
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
        _movement = Controls.Player.Move.ReadValue<Vector2>();

        Animator.SetFloat("MovementSpeed", _movement.magnitude);
    }

    private void Move()
    {
        RB.MovePosition(RB.position + _movement * (_moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustFacingDirection()
    {
        Vector2 mousePosition = Controls.Player.Look.ReadValue<Vector2>();
        Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(RB.position);

        IsFacingLeft = mousePosition.x < playerScreenPoint.x;
        Renderer.flipX = IsFacingLeft;
    }

    private void Dash(CallbackContext context)
    {
        _dashRoutine ??= StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        _moveSpeed *= _dashSpeed;
        yield return new WaitForSeconds(_dashDuration);
        _moveSpeed /= _dashSpeed;

        _dashRoutine = null;
    }
}
