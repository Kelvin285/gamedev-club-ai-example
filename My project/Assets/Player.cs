using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 Motion { get; private set; }

    // Update is called once per frame
    void FixedUpdate()
    {
        float speed = 0.1f;

        Motion = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            Motion += new Vector3(0, 0, 1) * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Motion += new Vector3(-1, 0, 0) * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Motion += new Vector3(0, 0, -1) * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Motion += new Vector3(1, 0, 0) * speed;
        }

        transform.position += Motion;
    }
}
