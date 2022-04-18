using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, jumpForce;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Text score;
    [SerializeField] private Transform cellingPoint, footPoint;
    [SerializeField] private int jumpCount;
    [SerializeField] private AudioClip jumpAudio, hurtAudio;
    [SerializeField] private bool isHurt;
    private AudioSource audioSource;
    private int collectionCount;
    private Animator ani;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private BoxCollider2D boxCol;
    [SerializeField]private bool jumpPressed,isOnGround, isJumped;
    private int hurtFrameCount = SettingVariable.hurtFrame;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        boxCol = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        hurtFrameCount = SettingVariable.hurtFrame;
}
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount >0 && !isHurt)
        {
            jumpPressed = true;
        }
        Crouch();
        CheckDead();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapCircle(footPoint.position, 0.2f, ground);
        if (!isHurt)
        {
            Movement();
        }
        SwitchAnim();
        Jump();
        
    }
    void Jump()
    {
        if (isOnGround && !isJumped)
        {
            jumpCount = SettingVariable.jumpCount;
        }
        if (jumpCount <= 0) jumpPressed = false;
        if (jumpPressed && jumpCount > 0)
        {
            rb.velocity = new Vector2(0, jumpForce * Time.fixedDeltaTime);
            ani.SetBool("jumping", true);
            audioSource.clip = jumpAudio;
            audioSource.Play();
            jumpCount--;
            jumpPressed = false;
            isJumped = true;
        }
    }
    void Crouch()
    {
        if (Input.GetButton("Crouch"))
        {
            boxCol.enabled = false;
            ani.SetBool("crouch", true);
        } else
        {
            if(!Physics2D.OverlapCircle(cellingPoint.position, 0.2f, ground))
            {
                boxCol.enabled = true;
                ani.SetBool("crouch", false);
            }
        }
    }
    void CheckDead()
    {
        if (transform.position.y < SettingVariable.deadLine)
        {
            StartCoroutine(_Dead());
        }
    }
    IEnumerator _Dead()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
    void Movement()
    {
        float move = Input.GetAxis("Horizontal");
        float face = Input.GetAxisRaw("Horizontal");
        if (move != 0)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime, rb.velocity.y);
        }
        ani.SetFloat("running", Mathf.Abs(move));

        if (face != 0) 
        {
            transform.localScale = new Vector3(face, 1, 1);
        }
    }
    IEnumerator Hurt()
    {
        //判断伤害帧数
        if(hurtFrameCount > 0)
        {
            ani.SetBool("hurt", true);
            ani.SetFloat("running", 0);
            hurtFrameCount--;
            yield return null;
        }
        if (hurtFrameCount == 0)
        {
            isHurt = false;
            ani.SetBool("hurt", false);
            hurtFrameCount = SettingVariable.hurtFrame;
            yield break;
        }

    }
    void SwitchAnim()
    {
        if (isHurt)
        {
            StartCoroutine(Hurt());
        } 
        else
        {
            if (rb.velocity.y < 0.1f && !isOnGround)
            {
                isJumped = false;
                ani.SetBool("jumping", false);
                ani.SetBool("falling", true);
            }
            if (!isHurt && ani.GetBool("jumping"))
            {
                if (rb.velocity.y < 0)
                {
                    isJumped = false;
                    ani.SetBool("jumping", false);
                    ani.SetBool("falling", true);
                }
            }
            else if (isOnGround)
            {
                ani.SetBool("falling", false);
            }
        }
    }
    // 收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collection"))
        {
            collision.gameObject.tag = "Untagged";
            collision.gameObject.GetComponent<Collection>().Collected();
            collectionCount++;
            score.text = collectionCount + "";
        }
    }
    // 消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isHurt)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                if (ani.GetBool("falling"))
                {
                    collision.collider.gameObject.tag = "Untagged";
                    Enemy_Base enemy = collision.gameObject.GetComponent<Enemy_Base>();
                    enemy.Dead();
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
                    ani.SetBool("jumping", true);
                }
                else if (transform.position.x < collision.gameObject.transform.position.x)
                {
                    audioSource.clip = hurtAudio;
                    audioSource.Play();
                    hurtFrameCount = SettingVariable.hurtFrame;
                    isHurt = true;
                    rb.velocity = new Vector2(-5, rb.velocity.y);
                }
                else if (transform.position.x > collision.gameObject.transform.position.x)
                {
                    audioSource.clip = hurtAudio;
                    audioSource.Play();
                    hurtFrameCount = SettingVariable.hurtFrame;
                    isHurt = true;
                    rb.velocity = new Vector2(5, rb.velocity.y);
                }

            }
        }
    }
}
