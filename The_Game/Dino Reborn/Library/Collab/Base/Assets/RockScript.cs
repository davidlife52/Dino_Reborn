using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rigidbody2D;
    public Vector2 direction;

    public float speed;
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
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
                Debug.Log("Rock hit something");
                
                Destroy(this.gameObject);
            }
        }
    }


}
