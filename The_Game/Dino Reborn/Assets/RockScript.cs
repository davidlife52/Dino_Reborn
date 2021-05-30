using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rigidbody2D;
    public MapHandler mapHandler;
    public Vector2 direction;

    public float speed;
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        mapHandler = GameObject.FindGameObjectWithTag("MapHandler").GetComponent<MapHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (rigidbody2D.bodyType != RigidbodyType2D.Static)
        {
            rigidbody2D.velocity = direction;
        }
    }

    public void Move (Vector2 direction)
    {
        rigidbody2D.velocity = direction;
    }


    public void MoveUp()
    {
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
        direction = new Vector2(0,speed);
    }
    public void MoveDown()
    {
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
        direction = new Vector2(0, -speed);
    }
    public void MoveRight()
    {
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
        direction = new Vector2(speed,0);
    }
    public void MoveLeft()
    {
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
        direction = new Vector2(-speed, 0);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag!="Player")
        {
         
            if (direction != default(Vector2))
            {
                if (col.gameObject.tag == "Enemy")
                {
                    //    Debug.Log("Test");
                    // ScoreManager.instance.AddPoints(50);
                    DestroyEnemy(col.gameObject);
                    return;
                }
                else
                {
                //    Debug.Log(col.gameObject.name);
                  //  Debug.Log("Rock hit something");

                    mapHandler.GameMap[-1 * (int)(transform.position.y - 5.5f), (int)(transform.position.x + 7.5f)] = this.gameObject;
                 //   Debug.Log(transform.position);

                    transform.position = new Vector3((int)(transform.position.x+ (transform.position.x > 0 ? 0.5f : -0.5f)), (int)(transform.position.y+ (transform.position.y > 0 ? 0.5f : -0.5f)), 0);
                    rigidbody2D.bodyType = RigidbodyType2D.Static;
                    rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                    direction = new Vector2(0, 0);
                }

              //  Destroy(this.gameObject);
            }
        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        mapHandler.RemoveEnemy();
        Destroy(enemy);
        ScoreManager.instance.AddPoints(50);
    }


}
