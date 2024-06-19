using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Sword : MonoBehaviour
{
    [SerializeField] private AnimationEffect _slashAnimationPrefab;
    [SerializeField] private Transform _slashAnimationSpawnPoint;
    [SerializeField] private Transform _weaponCollider;
    [SerializeField] private float _attackCooldown = .5f;

    private PlayerController _playerController;
    private ActiveWeapon _activeWeapon;
    private Animator _animator;

    private PlayerControls _controls;
    private AnimationEffect _slashEffect;
    private Coroutine _attackRoutine;
    private bool _isAttacking;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _activeWeapon = GetComponentInParent<ActiveWeapon>();
        _animator = GetComponent<Animator>();

        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void Start()
    {
        _controls.Combat.Attack.started += StartAttacking;
        _controls.Combat.Attack.canceled += StopAttacking;
    }

    private void OnDestroy()
    {
        _controls.Combat.Attack.started -= StartAttacking;
        _controls.Combat.Attack.canceled -= StopAttacking;
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    private IEnumerator AttackRoutine()
    {
        while (_isAttacking)
        {
            _slashEffect = Instantiate(_slashAnimationPrefab, _slashAnimationSpawnPoint);
            if (_playerController.IsFacingLeft)
            {
                _slashEffect.Renderer.flipX = true;
            }

            _weaponCollider.gameObject.SetActive(true);
            _animator.SetTrigger("Attack");

            yield return new WaitForSeconds(_attackCooldown);
        }

        _attackRoutine = null;
    }

    private void StartAttacking(CallbackContext context)
    {
        _isAttacking = true;
        _attackRoutine ??= StartCoroutine(AttackRoutine());
    }

    private void StopAttacking(CallbackContext context)
    {
        _isAttacking = false;
    }

    public void DoneAttackingAnimEvent()
    {
        _weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        _slashEffect?.ChangeRotation(new Vector3(180f, 0f, 0f));
    }

    public void SwingDownFlipAnimEvent()
    {
        _slashEffect?.ChangeRotation(new Vector3(0f, 0f, 0f));
    }

    private void MouseFollowWithOffset()
    {
        Vector2 mousePosition = _controls.Player.Look.ReadValue<Vector2>();
        Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(_playerController.transform.position);

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        if (mousePosition.x < playerScreenPoint.x)
        {
            _activeWeapon.transform.rotation = Quaternion.Euler(0f, -180f, angle);
            _weaponCollider.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
        }
        else
        {
            _activeWeapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            _weaponCollider.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
