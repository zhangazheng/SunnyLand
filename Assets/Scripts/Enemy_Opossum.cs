using UnityEngine;

public class Enemy_Opossum : Enemy_Base
{
    [SerializeField] private float speed;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
        rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, rb.velocity.y);
    }
}
