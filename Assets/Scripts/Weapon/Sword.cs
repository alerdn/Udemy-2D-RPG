using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private AnimationEffect _slashAnimationPrefab;
    [SerializeField] private Transform _slashAnimationSpawnPoint;

    private PlayerController _playerController;
    private ActiveWeapon _activeWeapon;
    private Animator _animator;

    private PlayerControls _controls;
    private AnimationEffect _slashEffect;

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
        _controls.Combat.Attack.started += _ => Attack();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    private void Attack()
    {
        _slashEffect = Instantiate(_slashAnimationPrefab, _slashAnimationSpawnPoint);
        if (_playerController.IsFacingLeft)
        {
            _slashEffect.Renderer.flipX = true;
        }

        _animator.SetTrigger("Attack");
    }

    public void SwingUpFlipAnim()
    {
        _slashEffect?.ChangeRotation(new Vector3(180f, 0f, 0f));
    }

    public void SwingDownFlipAnim()
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
        }
        else
        {
            _activeWeapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
