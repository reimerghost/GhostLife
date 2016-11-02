using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public Transform parent;

    public GameObject player;
    public GameObject enemy;
    public int enemyAmount = 10;

    public GameObject[] tiles;
    public GameObject wall;

    public List<Vector3> createdTiles;

    public int tileAmount,cantidad=0;
    public int tileSize;
    public int seed;

    public float waitTime;

    public float chanceUp,chanceRight,chanceDown;

    //Wall Generation
    public float minY = 9999999;
    public float maxY = 0;
    public float minX = 9999999;
    public float maxX = 0;
    public float xAmount, yAmount;
    public float extraWallX, extraWallY;

    // Use this for initialization
    void Start()
    {
        parent = new GameObject().transform;
        parent.name = "Laberinto"; 
        StartCoroutine(GenerateLevel());
        //Random.InitState(seed);
    }

    IEnumerator GenerateLevel()
    {
        for (int i = 0; i < tileAmount; i++)
        {
            cantidad++;
            float dir = Random.Range(0f, 1f);
            int tile = Random.Range(0, tiles.Length);
            CreateTile(tile);
            CallMoveGen(dir);
            yield return new WaitForSeconds(waitTime);

            if (i == tileAmount-1)
            {
                Finish();
            }
        }
        yield return 0;
    }
    #region MoverGenerador
    void CallMoveGen(float ranDir)
    {
        if (ranDir<chanceUp)
        {
            MoveGen(0);
        } else if (ranDir < chanceRight)
        {
            MoveGen(1);
        } else if (ranDir < chanceDown)
        {
            MoveGen(2);
        }else
        {
            MoveGen(3);
        }
    }

    void MoveGen(int dir)
    {
        switch (dir)
        {
            case 0://ARRIBA
                transform.position = new Vector3(transform.position.x, transform.position.y + tileSize, 0);
                break;
            case 1://DERECHA
                transform.position = new Vector3(transform.position.x + tileSize, transform.position.y, 0);
                break;
            case 2://ABAJO
                transform.position = new Vector3(transform.position.x, transform.position.y - tileSize, 0);
                break;
            case 3:// IZQUIERDA
                transform.position = new Vector3(transform.position.x - tileSize, transform.position.y, 0);
                break;
        }
    }
    #endregion

    void CreateTile(int tileIndex)
    {
        if (!createdTiles.Contains(transform.position))
        {
            GameObject tileObject;
            tileObject = Instantiate(tiles[tileIndex], transform.position, transform.rotation) as GameObject;
            Debug.Log(tileIndex);
            createdTiles.Add(tileObject.transform.position);

            tileObject.transform.parent = parent;
        }
        else
        {
            tileAmount++;
        }         
    }

    void Finish()
    {
        CreateWallValues();
        CreateWalls();
        SpawnObjects();
    }
    #region Walls
    void CreateWallValues()
    {
        for (int i=0;i<createdTiles.Count;i++)
        {
            if (createdTiles[i].y<minY)
            {
                minY = createdTiles[i].y;
            }
            if (createdTiles[i].y > maxY)
            {
                maxY = createdTiles[i].y;
            }
            if (createdTiles[i].x < minX)
            {
                minX = createdTiles[i].x;
            }
            if (createdTiles[i].x > maxX)
            {
                maxX = createdTiles[i].x;
            }

            xAmount = ((maxX - minX) / tileSize) + extraWallX;
            yAmount = ((maxY - minY) / tileSize) + extraWallY;
        }
    }

    void CreateWalls()
    {
        for (int x = 0; x < xAmount; x++)
        {
            for (int y = 0; y < yAmount; y++)
            {
                Vector3 v3 = new Vector3((minX - (extraWallX * tileSize) / 2) + (x * tileSize), (minY - (extraWallY * tileSize) / 2) + (y * tileSize));
                if (!createdTiles.Contains(v3))
                {
                    GameObject wallObj = Instantiate(wall, v3,transform.rotation) as GameObject;
                    wallObj.transform.parent = parent;
                }
            }
        }
    }
    #endregion

    void SpawnObjects()
    {
        Instantiate(player,  transform.position, Quaternion.identity);
        GameObject Camara = GameObject.Find("Main Camera");
        Camara.transform.position = player.transform.position;
        Camara.GetComponent<SeguirPersonaje>().personaje = player;

        for (int i = 0; i<enemyAmount;i++)
        {
            Instantiate(enemy, createdTiles[Random.Range(0, createdTiles.Count)], Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}

