using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public float speed;
    public Action OnHit;
    private bool isAlive=true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive)
        {
            isAlive = false;
            OnHit?.Invoke();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.up * speed;
    }


}
