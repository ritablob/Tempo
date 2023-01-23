using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float speed;

    private float lerpAmnt;
    private Vector3 direction;

    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    public void SetEndPosition() { direction = transform.forward; direction.y = 0; }
}
