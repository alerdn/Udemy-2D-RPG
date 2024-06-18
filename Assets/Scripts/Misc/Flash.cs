using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [field: SerializeField] public float RestoreDefaultMaterialTime { get; private set; } = .2f;

    [SerializeField] private Material _whiteMaterial;

    private SpriteRenderer _renderer;
    private Material _defaultMaterial;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _renderer.material;
    }

    public IEnumerator FlashRoutine()
    {
        _renderer.material = _whiteMaterial;
        yield return new WaitForSeconds(RestoreDefaultMaterialTime);
        _renderer.material = _defaultMaterial;
    }
}
