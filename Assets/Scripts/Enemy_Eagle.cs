using UnityEngine;

public class Enemy_Eagle : Enemy_Base
{
    [SerializeField] private float speed;
    [SerializeField] private Transform topPoint, bottomPoint;
    private bool up;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // 和父级断绝关系
        transform.DetachChildren();
        // 离左边近，先向左移动
        if (Vector3.Distance(transform.position, topPoint.position) < Vector3.Distance(transform.position, bottomPoint.position))
        {
            up = true;
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!dead)
        {
            Movement();
        }
    }
    void Movement()
    {
        if (up)
        {
            rb.velocity = new Vector2(0, speed * Time.fixedDeltaTime);
            if (transform.position.y > topPoint.position.y)
            {
                up = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, -speed * Time.fixedDeltaTime);
            if (transform.position.y < bottomPoint.position.y)
            {
                up = true;
            }
        }
    }
}
