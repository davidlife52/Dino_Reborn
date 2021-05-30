using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public MapHandler mapHandler;
    public Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;
    public Animator animator;


    public Vector3 targetPosition;
    public bool InTransition = false;
    public bool Active = false;
    public bool Stop = false;
    public float speed;


    public int tileX = 0;
    public int tileY = 0;

    public int viewDirection;

    public int dirX = 0;
    public int dirY = 0;

    public int dir = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        CalcTilePos();
        SetNextPosition(tileX, tileY);

        Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        CalcTilePos();

        if (!InTransition && Active && !Stop)
        {
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetBool("RunSide", false);
                animator.SetBool("RunDown", false);
                animator.SetBool("RunUp", true);
                viewDirection = 1;
                SetNextPosition(tileX - 1, tileY);
            }
            if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("RunSide", false);
                animator.SetBool("RunUp", false);
                animator.SetBool("RunDown", true);
                viewDirection = 3;
                SetNextPosition(tileX + 1, tileY);
            }
            if (Input.GetKey(KeyCode.D))
            {

                animator.SetBool("RunUp", false);
                animator.SetBool("RunDown", false);
                animator.SetBool("RunSide", true);
                viewDirection = 2;
                SetNextPosition(tileX, tileY + 1);
                var scale = animator.gameObject.transform.localScale;
                animator.gameObject.transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
            }

            if (Input.GetKey(KeyCode.A))
            {
                animator.SetBool("RunUp", false);
                animator.SetBool("RunDown", false);
                animator.SetBool("RunSide", true);
                viewDirection = 4;
                SetNextPosition(tileX, tileY - 1);
                var scale = animator.gameObject.transform.localScale;
                animator.gameObject.transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("RunUp", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("RunDown", false);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("RunSide", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("RunSide", false);
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            Active = false;
            mapHandler.MoveRock(viewDirection, tileX, tileY);
            Invoke("Reactivate", 0.2f);
        }

    }


    void FixedUpdate()
    {
        var diff = targetPosition - transform.position;

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Debug.Log( "ee " + diff);
        }
        if (Mathf.Abs(diff.x) < 0.05f && Mathf.Abs(diff.y) < 0.05f)
        {
            InTransition = false;
            rigidbody2D.velocity = new Vector2(0, 0);
        }
        else
        {
            InTransition = true;
            int dX = 0;
            int dY = 0;
            if (Mathf.Abs(diff.x) < Mathf.Abs(diff.y))
            {
                dY = 1 * Sigma(diff.y);
            }
            else
            {
                dX = 1 * Sigma(diff.x);
            }

            rigidbody2D.velocity = new Vector2(dX * speed, dY * speed);
        }
        //rigidbody2D.velocity = new Vector2(dirX * speed * (dir % 2), dirY * speed * ((dir + 1) % 2));
    }
    public void Reactivate()
    {
        Active = true;
    }
    public void CalcTilePos()
    {
        tileY = (int)(transform.position.x + 7.5f);
        tileX = -1 * (int)(transform.position.y - 5.5f);
    }
    public Vector3 CalculatePosition(int x, int y)
    {

        int X = y - 7;
        int Y = -x + 5;

        return new Vector3(X, Y, 0);
    }

    public void SetNextPosition(int x, int y)
    {
        if (x < 0 || x > 10 || y < 0 || y > 14)
        {
            return;
        }
        if (mapHandler.GameMap[x, y] != null)
        {
            return;
        }
        targetPosition = CalculatePosition(x, y);
        return;
    }
    public static int Sigma(int a)
    {
        if (a == 0)
        {
            return a;
        }
        else if (a > 0)
        {
            return 1;
        }
        return -1;
    }
    public static int Sigma(float a)
    {
        if (a == 0)
        {
            return 0;
        }
        else if (a > 0)
        {
            return 1;
        }
        return -1;
    }

    public void Reappear()
    {
        Physics2D.IgnoreLayerCollision(4, 6, false);
        spriteRenderer.color = Color.white;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            HitByEnemy();
        }
    }

    public void HitByEnemy()
    {
        Physics2D.IgnoreLayerCollision(4, 6);
        spriteRenderer.color = new Color(1, 0.5f, 0.5f, 0.5f);
        Invoke("Reappear", 5);
        if (spriteRenderer.color != Color.white)
        {
            ScoreManager.instance.LooseLife();
        }
    }
}
