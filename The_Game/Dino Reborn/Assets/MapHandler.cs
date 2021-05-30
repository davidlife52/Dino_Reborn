using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform Origin;

    public GameObject[,] GameMap = new GameObject[11, 15];

    public string StartObjects;

    public GameObject RockPrefab;
    public GameObject EnemyPrefab;


    public int RockCount = 0;
    public int EnemyCount = 0;
    void Start()
    {
        for (int i = 0; i < 11 * 15; i++)
        {
            if (StartObjects[i] == 'R')
            {
                var newRock = Instantiate(RockPrefab, new Vector3(Origin.position.x + (1f) * (i % 15), Origin.position.y - (1f) * ((int)(i / 15)), 1), default(Quaternion));
                newRock.name = "Rock" + i;
                RockCount += 1;
                GameMap[(int)i / 15, i % 15] = newRock;
            }
            else if (StartObjects[i] == 'E')
            {
                EnemyCount += 1;
                var newEnemy = Instantiate(EnemyPrefab, new Vector3(Origin.position.x + (1f) * (i % 15), Origin.position.y - (1f) * ((int)(i / 15)), 1), default(Quaternion));
            }
            else
            {
                GameMap[(int)i / 15, i % 15] = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemoveEnemy()
    {
        EnemyCount -= 1;

        if (EnemyCount <= 0)
        {
            Debug.Log("Level cleared");
            ScoreManager.instance.OnLevelClearedNoAliens();
        }
    }
    public void RemoveRock(GameObject rock)
    {
        Destroy(rock);
        RockCount -= 1;

        if (RockCount <= 0)
        {
            Debug.Log("Level cleared");
            ScoreManager.instance.OnLevelClearedNoBlocks();
        }
    }

    public bool IsInBounds(int x, int y)
    {
        if (x < 0 || x > 10 || y < 0 || y > 14)
        {
            return false;
        }
        return true;
    }
    public void MoveRock(int direction, int x, int y)
    {
        switch (direction)
        {
            case 1:
                {
                    if (x - 1 < 0)
                    {
                        return;
                    }
                    var rock = GameMap[x - 1, y];
                    if (rock == null)
                    {
                        return;
                    }
                    GameMap[x - 1, y] = null;
                    // Destroy(rock);
                    var sc = rock.GetComponent<RockScript>();
                    if (sc != null)
                    {
                        if (!IsInBounds(x - 2, y))
                        {
                            RemoveRock(rock);
                            ScoreManager.instance.AddPoints(1);
                            return;
                        }
                        if (GameMap[x - 2, y]!= null)
                        {
                            RemoveRock(rock);
                            ScoreManager.instance.AddPoints(1);
                            return;
                        }

                        sc.MoveUp();
                    }

                }
                break;
            case 2:
                {
                    if (y + 1 > 14)
                    {
                        return;
                    }
                    var rock = GameMap[x, y + 1];
                    if (rock == null)
                    {
                        return;
                    }
                    GameMap[x, y + 1] = null;
                    //Destroy(rock);

                    var sc = rock.GetComponent<RockScript>();
                    if (sc != null)
                    {
                        if (!IsInBounds(x, y + 2))
                        {
                            RemoveRock(rock);
                            ScoreManager.instance.AddPoints(1);
                            return;
                        }
                        if (GameMap[x, y + 2] != null)
                        {
                            RemoveRock(rock);
                            ScoreManager.instance.AddPoints(1);
                            return;
                        }
                        sc.MoveRight();
                    }
                }
                break;
            case 3:
                {
                    if (x + 1 > 10)
                    {
                        return;
                    }
                    var rock = GameMap[x + 1, y];
                    if (rock == null)
                    {
                        return;
                    }
                    GameMap[x + 1, y] = null;
                    // Destroy(rock);

                    var sc = rock.GetComponent<RockScript>();
                    if (sc != null)
                    {
                        if (!IsInBounds(x + 2, y))
                        {
                            RemoveRock(rock);
                            ScoreManager.instance.AddPoints(1);
                            return;
                        }
                        if (GameMap[x + 2, y] != null)
                        {
                            RemoveRock(rock);
                            ScoreManager.instance.AddPoints(1);
                            return;
                        }
                        sc.MoveDown();
                    }
                }
                break;
            case 4:
                {
                    if (y - 1 < 0)
                    {
                        return;
                    }
                    var rock = GameMap[x, y - 1];
                    if (rock == null)
                    {
                        return;
                    }
                    GameMap[x, y - 1] = null;
                    // Destroy(rock);

                    var sc = rock.GetComponent<RockScript>();
                    if (sc != null)
                    {
                        if (!IsInBounds(x, y - 2))
                        {
                            RemoveRock(rock);
                            ScoreManager.instance.AddPoints(1);
                            return;
                        }
                        if (GameMap[x, y - 2] != null)
                        {
                            RemoveRock(rock);
                            ScoreManager.instance.AddPoints(1);
                            return;
                        }
                        sc.MoveLeft();
                    }
                }
                break;
        }
    }
}
