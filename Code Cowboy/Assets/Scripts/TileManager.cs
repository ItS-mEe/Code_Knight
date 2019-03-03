using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public GameObject[,] map;
    public GameObject[,] entities;
    public int mapW, mapH, tileW, tileH, mountainRangeNum;
    public int adjust;

    public GameObject mountainPrefab1, mountainPrefab2, mountainPrefab3, mountainPrefab4, grassPrefab, treePrefab, playerPrefab, enemyPrefab;

    public GameObject[] enemies;
    public int numEnemies;

    // Start is called before the first frame update
    public void Awake()
    {
        map = new GameObject[mapW, mapH];
        entities = new GameObject[mapW, mapH];
        enemies = new GameObject[numEnemies];

        // initialize map

        // fill with grass prefabs
        for(int i = 0; i<map.GetLength(0); i++){
            for(int j = 0; j<map.GetLength(1); j++){
                map[i,j] = grassPrefab;
            }
        }

        //randomly tree up
        for(int i = 2; i<map.GetLength(0) - 2; i++){
            for(int j = 2; j<map.GetLength(1) - 2; j++){
                if(Random.value < 0.5) map[i,j] = treePrefab;
            }
        }
        map[4,3] = mountainPrefab1;
        map[4,4] = mountainPrefab2;
        map[3,3] = mountainPrefab3;
        map[3,4] = mountainPrefab4;

        // make mountainRangeNum mountain range
        // for(int n = 0; n<mountainRangeNum; n++){
        //     Vector2 startpos = new Vector2 ((int) (Random.value * (mapW - 2) + 1), (int) (Random.value * (mapH - 2) + 1));
        //     Vector2 endpos = new Vector2 ((int) (Random.value * (mapW - 2) + 1), (int) (Random.value * (mapH - 2) + 1));
        //     if(startpos.y == endpos.y){
        //         for (int i = (int) Mathf.Min(startpos.x, endpos.x); i<=Mathf.Max(startpos.x, endpos.x); i++){
        //             map[i, (int) startpos.y] = mountainPrefab;
        //         }
        //     } else if (startpos.x == endpos.x){
        //         for (int j = (int) Mathf.Max(startpos.y, endpos.y); j>=Mathf.Min(startpos.y, endpos.y); j--){
        //             map[(int) startpos.x, j] = mountainPrefab;
        //         }
        //     } else {

        //         float slope = (startpos.y - endpos.y) / (startpos.x - endpos.x);

        //         if(Mathf.Abs(slope) > 1){
        //             int i;
        //             for (int j = (int) Mathf.Max(startpos.y, endpos.y); j>=Mathf.Min(startpos.y, endpos.y); j--){
        //                 i = (int) ((j - startpos.y) * Mathf.Pow(slope, -1) + startpos.x);
        //                 map[i, j] = mountainPrefab;
        //             }
        //         } else{
        //             int j;
        //             for (int i = (int) Mathf.Min(startpos.x, endpos.x); i<=Mathf.Max(startpos.x, endpos.x); i++){
        //                 j = (int) ((i - startpos.x) * slope + startpos.y);
        //                 map[i, j] = mountainPrefab;
        //             }
        //         }
        //     }
        // }
        // instantiate all prefabs and set their positions
        for(int i = 0; i<map.GetLength(0); i++){
            for(int j = 0; j<map.GetLength(1); j++){
                map[i,j] = Instantiate(map[i,j]);
                map[i,j].transform.parent = this.transform;
                map[i,j].transform.position = this.transform.position + new Vector3((2 * i + adjust) * tileW / 2.0f, (2 * j + adjust) * tileH / 2.0f, 0);
            }
        }
        entities[0,0] = GameObject.Instantiate(playerPrefab);
        for(int i = 0; i<numEnemies; i++){
            enemies[i] = Instantiate(enemyPrefab);
            entities[mapH-1 - i/mapH, mapH-1-(i % mapH)] = enemies[i]; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<map.GetLength(0); i++){
            for(int j = 0; j<map.GetLength(1); j++){
                map[i,j].transform.position = this.transform.position + new Vector3((2 * i + adjust) * tileW / 2.0f, (2 * j + adjust) * tileH / 2.0f, 0);
            }
        }

        for(int i = 0; i<map.GetLength(0); i++){
            for(int j = 0; j<map.GetLength(1); j++){
                if(entities[i,j] == null) continue;
                entities[i,j].transform.position = this.transform.position + new Vector3((2 * i + adjust) * tileW / 2.0f, (2 * j + adjust) * tileH / 2.0f, 0);
            }
        }
    }

    public void turnLeft(){
        GameObject[,] newmap = new GameObject[mapW, mapH];
        GameObject[,] newentities = new GameObject[mapW, mapH];

        for(int i = 0; i<map.GetLength(0); i++){
            for(int j = 0; j<map.GetLength(1); j++){
                if((i == 3 || i== 4) && (j == 3 || j==4)) {
                    print("im herer...");
                    newmap[i,j] = map[i,j];
                } else {
                    newmap[i,j] = map[mapW-1-j, i];
                    newentities[i,j] = entities[mapW-1-j, i];
                }
            }
        }
        map = newmap;
        entities = newentities;
    }

    public void turnRight(){
        print("getting called");
        GameObject[,] newmap = new GameObject[mapW, mapH];
        GameObject[,] newentities = new GameObject[mapW, mapH];

        for(int i = 0; i<map.GetLength(0); i++){
            for(int j = 0; j<map.GetLength(1); j++){
                print("i " + i + ", j " + j);
                if((i == 3 || i== 4) && (j == 3 || j==4)) {
                                        print("im herer...");

                    newmap[i,j] = map[i,j];
                                        continue;

                } else {
                    newmap[i,j] = map[j, mapW-1-i];
                    newentities[i,j] = entities[j, mapW-1-i];
                }
            }
        }
        map = newmap;
        entities = newentities;
    }

    public Vector2Int find(GameObject entity){
        for(int i = 0; i<map.GetLength(0); i++){
            for(int j = 0; j<map.GetLength(1); j++){
                if(entities[i,j] == entity){
                    return new Vector2Int(i,j);
                }
            }
        }
        return Vector2Int.zero;
    }

    public void cleanup(){
        for(int i = 0; i<map.GetLength(0); i++){
            for(int j = 0; j<map.GetLength(1); j++){
                Destroy(map[i, j]);
                if(entities[i, j] != null)
                    Destroy(entities[i, j]);
            }
        }
    }
}
