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
    [SerializeField] private Transform cellingPoint;
    [SerializeField] private int jumpCount;
    private int collectionCount;
    private Animator ani;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private BoxCollider2D boxCol;
    private bool isHurt;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        boxCol = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        Jump();
        Crouch();
        CheckDead();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isHurt)
        {
            Movement();
        }
        SwitchAnim();
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 1 && !isHurt)
        {
            rb.velocity = new Vector2(0, jumpForce * Time.fixedDeltaTime);
            ani.SetBool("jumping", true);
            jumpCount--;
        }
        if (col.IsTouchingLayers(ground))
        {
            jumpCount = 2;
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
    void SwitchAnim()
    {
        if (rb.velocity.y < 0.1f && !col.IsTouchingLayers(ground))
        {
            ani.SetBool("jumping", false);
            ani.SetBool("falling", true);
        }
        if (ani.GetBool("jumping"))
        {
            if(rb.velocity.y < 0)
            {
                ani.SetBool("jumping", false);
                ani.SetBool("falling", true);
            }
        } else if (isHurt)
        {
            ani.SetBool("hurt", true);
            ani.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                isHurt = false;
                ani.SetBool("hurt", false);
            }
        }
        else if (col.IsTouchingLayers(ground))
        {
            ani.SetBool("falling", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collection"))
        {
            collision.gameObject.tag = "Untagged";
            Destroy(collision.gameObject);
            collectionCount++;
            score.text = collectionCount + "";
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (ani.GetBool("falling"))
            {
                Enemy_Base enemy = collision.gameObject.GetComponent<Enemy_Base>();
                enemy.Dead();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
                ani.SetBool("jumping", true);
            }
            else if(transform.position.x < collision.gameObject.transform.position.x)
            {
                isHurt = true;
                rb.velocity = new Vector2(-5, rb.velocity.y);
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                isHurt = true;
                rb.velocity = new Vector2(5, rb.velocity.y);
            }

        }
    }
}
