using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;
        [SerializeField] private float _knockedbackThrust = 10f;

    private int _currentHealth;
    private Knockback _knockback;
    private Flash _flash;

    private void Awake()
    {
        _knockback = GetComponent<Knockback>();
        _flash = GetComponent<Flash>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);

        _knockback.GetKnockedBack(PlayerController.Instance.transform, _knockedbackThrust);
        StartCoroutine(_flash.FlashRoutine());

        StartCoroutine(DetectDeathRoutine());
    }

    private IEnumerator DetectDeathRoutine()
    {
        yield return new WaitForSeconds(_flash.RestoreDefaultMaterialTime);
        DetectDeath();
    }

    public void DetectDeath()
    {
        if (_currentHealth != 0) return;

        Destroy(gameObject);
    }
}
