using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    Transform spriteTransform;
    [SerializeField]
    float RotationSpeed = 50;
    void Start()
    {
        spriteTransform = GetComponentInChildren<SpriteRenderer>().gameObject.transform;
    }
    private void Update()
    {
        spriteTransform.Rotate(new Vector3(0, 0, RotationSpeed * Time.deltaTime));
    }
}
