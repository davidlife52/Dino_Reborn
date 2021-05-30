using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    // Start is called before the first frame update



    public Rigidbody2D rigidbody2D;
    public GameObject player;
    public MapHandler mapHandler;

    public Vector3 targetPosition;
    public bool InTransition = false;

    public float speed = 2;

    public PlayerMovement playerMovement;

    public Vector2 direction;

    public int tileX;
    public int tileY;
    void Start()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        mapHandler = GameObject.FindGameObjectWithTag("MapHandler").GetComponent<MapHandler>();
        tileY = (int)(transform.position.x + 7.5f);
        tileX = -1 * (int)(transform.position.y - 5.5f);

        Recalculate();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
          //  Debug.Log(FindPlayer());
        }

        tileY = (int)(transform.position.x + 7.5f);
        tileX = -1 * (int)(transform.position.y - 5.5f);        
    }
    private void FixedUpdate()
    {
        var diff = targetPosition - transform.position;
        if (!InTransition)
        {
            Recalculate();
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


    public void Recalculate()
    {
        var d = FindPlayer();

        direction = new Vector2(d.y, d.x);
       /// Debug.Log(direction);
        SetNextPosition(tileX+(int)direction.y, tileY+(int)direction.x);
    }

   

    public Vector2 FindPlayer ()
    {
        List<Vector2> visited = new List<Vector2>();

        List<Node> queue = new List<Node>();

        Node parent = new Node(null, new Vector2(tileX, tileY));


        queue.Add(new Node(parent,new Vector2(tileX - 1, tileY)));
        queue.Add(new Node(parent, new Vector2(tileX + 1, tileY)));
        queue.Add(new Node(parent, new Vector2(tileX, tileY +1)));
        queue.Add(new Node(parent, new Vector2(tileX, tileY -1)));

        visited.Add( new Vector2(tileX - 1, tileY));
        visited.Add( new Vector2(tileX + 1, tileY));
        visited.Add( new Vector2(tileX, tileY + 1));
        visited.Add( new Vector2(tileX, tileY - 1));

        bool cont = true;

        while (queue.Count > 0)
        {
            string vis = "Visited: ";
            foreach (var v in visited)
            {
                vis += v.ToString() + " ";
            }
          //  Debug.Log("Visited: " + visited.Count);

         //   Debug.Log("Queue:" + queue.Count);


            List<Node> newQueue = new List<Node>();
            foreach (var i in queue)
            {   

              //  Debug.Log(i.position);
                if (i.position.x < 0 || i.position.x > 10 || i.position.y < 0 || i.position.y > 14)
                {
                //    Debug.Log("skip");
                    continue;
                }
              //  Debug.Log("=>??");
                if (playerMovement.tileX == i.position.x && playerMovement.tileY == i.position.y)
                {
                    Node p = i;

                    int cnt = 0;
                  //  Debug.Log("Reached this");
                    while (cnt < 100)
                    {
                        if (p.parent.parent == null)
                        {
                        //    Debug.Log("--------");
                        //    Debug.Log(p.parent.position);
                         //   Debug.Log(p.position);
                         //   Debug.Log("--------");
                            return p.position - p.parent.position;
                        }

                        cnt++;
                        p = p.parent;
                    }
                }
                else if (mapHandler.GameMap[(int)i.position.x, (int)i.position.y] != null)
                {
                    continue;
                }
                else
                {
                  //  Debug.Log("this=>??");
                    List<Vector2> adjs = new List<Vector2>();

                    adjs.Add(new Vector2(i.position.x + 1, i.position.y));
                    adjs.Add(new Vector2(i.position.x - 1, i.position.y));
                    adjs.Add(new Vector2(i.position.x, i.position.y + 1));
                    adjs.Add(new Vector2(i.position.x, i.position.y - 1));


                    foreach (var o in adjs)
                    {
                        if (!visited.Exists(n =>( n.x == o.x && n.y == o.y) ))
                        {

                            if (newQueue.Exists( q => q.position.x == o.x && q.position.y == o.y))
                            {
                                //Debug.Log("skip");
                                continue;
                            }

                            if (o.x < 0 || o.x > 10 || o.y < 0 || o.y > 14)
                            {
                                //Debug.Log("skip");
                                continue;
                            }
                            newQueue.Add(new Node(i, o));
                        }
                        else
                        {
                        //    Debug.Log("????");
                        }
                    }
                }
            }

           // Debug.Log(newQueue.Count);

            queue = newQueue;

            foreach ( var q in queue)
            {
                visited.Add(q.position);
            }
        }

        return default(Vector2);
    }
    class Node
    {
        public Node parent { get; set; }

        public Vector2 position { get; set; }

        public Node(Node parent, Vector2 position)
        {
            this.parent = parent;
            this.position = position;
        }
    }
}
