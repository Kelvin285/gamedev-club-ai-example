using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 speed;

    public float lifetime = 3;

    void FixedUpdate()
    {
        transform.position += speed;

        lifetime -= Time.fixedDeltaTime;
        if (lifetime < 0) Destroy(gameObject);
    }
}
