using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Enemy_Base : MonoBehaviour
{
    protected Animator ani;
    protected Rigidbody2D rb;
    [SerializeField] protected AudioClip deadAudio;
    protected AudioSource audioSource;
    public bool dead; // 被玩家踩死
    private bool fallDead; // 掉落到死亡线时的死亡
    // Start is called before the first frame update
    protected virtual void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!dead && transform.position.y < SettingVariable.deadLine)
        {
            fallDead = true;
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
    public void Dead()
    {
        if (!dead)
        {
            dead = true;
            if (!fallDead)
            {
                audioSource.clip = deadAudio;
                audioSource.Play();
                ani.SetTrigger("dead");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
