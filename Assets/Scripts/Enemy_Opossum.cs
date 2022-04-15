using UnityEngine;

public class Enemy_Opossum : Enemy_Base
{
    [SerializeField] private float speed;
    [SerializeField] private Transform leftPoint, rightPoint;
    private float leftX, rightX;
    private bool faceLeft;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
        // 离左边近，先向左移动
        if (Mathf.Abs(transform.position.x - leftX) < Mathf.Abs(transform.position.x - rightX))
        {
            faceLeft = true;
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!dead)
        {
            CheckPositionAndMovement();
        }
    }
    void CheckPositionAndMovement()
    {
        if (faceLeft)
        {
            rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, rb.velocity.y);
            if (transform.position.x < leftX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
                faceLeft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
            if (transform.position.x > rightX)
            {
                transform.localScale = new Vector3(1, 1, 1);
                rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, rb.velocity.y);
                faceLeft = true;
            }
        }
    }
}
