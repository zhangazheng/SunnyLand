using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform leftPoint, rightPoint;
    private Rigidbody2D rb;
    private bool faceLeft;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();
        // 离左边近，先向左移动
        if (Vector3.Distance(transform.position, leftPoint.position) < Vector3.Distance(transform.position, rightPoint.position))
        {
            faceLeft = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (faceLeft)
        {
            rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, 0);
            if (transform.position.x <= leftPoint.position.x)
            {
                faceLeft = false;
            }
        } else
        {
            rb.velocity = new Vector2(speed * Time.fixedDeltaTime, 0);
            if (transform.position.x >= rightPoint.position.x)
            {
                faceLeft = true;
            }
        }
    }
}
