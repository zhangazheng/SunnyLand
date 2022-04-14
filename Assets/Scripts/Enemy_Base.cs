using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    protected Animator ani;
    protected Rigidbody2D rb;
    public bool dead;
    public void Dead()
    {
        dead = true;
        ani.SetTrigger("dead");
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (!dead && transform.position.y < SettingVariable.deadLine)
        {
            Dead();
        }
    }
    protected virtual void FixedUpdate()
    {
        if (dead)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
}
