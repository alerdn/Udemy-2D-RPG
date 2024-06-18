using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffect : MonoBehaviour
{
    public SpriteRenderer Renderer { get; private set; }

    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeRotation(Vector3 eulerRotation)
    {
        transform.rotation = Quaternion.Euler(eulerRotation);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
