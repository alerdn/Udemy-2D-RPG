using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float _knockbackTime = .5f;

    public bool IsGettingKnockedback { get; private set; } = false;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float thrust)
    {
        IsGettingKnockedback = true;
        Vector2 force = (transform.position - damageSource.position).normalized * thrust * _rb.mass;
        
        _rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(KnockbackRoutine());
    }

    private IEnumerator KnockbackRoutine()
    {
        yield return new WaitForSeconds(_knockbackTime);
        _rb.velocity = Vector2.zero;
        IsGettingKnockedback = false;
    }
}
