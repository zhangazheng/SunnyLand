using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy_Base
{
    [SerializeField] private float speed, jumpForce;
    [SerializeField] private Transform leftPoint, rightPoint;
    [SerializeField] private LayerMask ground;
    private bool faceLeft;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        transform.DetachChildren();
        // 离左边近，先向左移动
        if (Vector3.Distance(transform.position, leftPoint.position) < Vector3.Distance(transform.position, rightPoint.position))
        {
            faceLeft = true;
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!dead)
        {
            CheckPositionAndMovement();
            SwitchAnim();
        }
    }
    public void Jump()
    {
        if (rb.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(0, jumpForce * Time.fixedDeltaTime);
            ani.SetBool("jump", true);
        }
    }
    void CheckPositionAndMovement()
    {
        if (faceLeft)
        {
            rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, rb.velocity.y);
            if (transform.position.x < leftPoint.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
                faceLeft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
            if (transform.position.x > rightPoint.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, rb.velocity.y);
                faceLeft = true;
            }
        }
    }
    void SwitchAnim()
    {
        if (rb.velocity.y < 0.1f)
        {
            if (rb.IsTouchingLayers(ground))
            {
                ani.SetBool("jump", false);
                ani.SetBool("fall", false);
            } else
            {
                ani.SetBool("jump", false);
                ani.SetBool("fall", true);
            }
        }
    }
}
