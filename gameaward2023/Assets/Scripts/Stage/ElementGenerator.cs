﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ElementGenerator : MonoBehaviour
{
    // プレイヤー関連 ---------------------------------------------------
    private Vector2 PlayerPos;              // プレイヤー座標格納用
    // ------------------------------------------------------------------

    // エネミー関連 -----------------------------------------------------
    // 調整用パラメーター
    [SerializeField] int MaxEnemy;  // 初期生成の敵の下限数
    [SerializeField] int MinEnemy;  // 初期生成の敵の上限数
    // ------------------------------------------------------------------

    // リソース格納用 (階層またぎのセーブ/ロードで使用) -----------------
    public Transform blockParent;                               // 親Obj
    private GameObject blockPrefab_Grass;                       // 地面Obj
    private GameObject blockPrefab_Water;                       // 水Obj
    private GameObject PlayerObj;                               // プレイヤーObj
    private GameObject EnemyObj;
    private GameObject LateralMove_Enemy;
    private GameObject VerticalMove_Enemy;
    private GameObject[] EnemyObjList = new GameObject[1];      // 敵リストObj
    private GameObject GoalObj;                                 // ゴールObj
    private GameObject ObstacleObj;                             // 障害物Obj
    private GameObject useObj;
    private GameObject use1Obj;
    private GameObject use2Obj;
    private GameObject use3Obj;
    private GameObject use4Obj;
    private GameObject use5Obj;
    private GameObject use6Obj;
    private GameObject use7Obj;
    private GameObject use8Obj;
    private GameObject use9Obj;
    private GameObject use10Obj;
    private GameObject use11Obj;
    private GameObject use12Obj;
    private GameObject use13Obj;
    private GameObject use14Obj;
    private GameObject use15Obj;
    private GameObject use16Obj;
    private GameObject use17Obj;
    private GameObject use18Obj;
    private GameObject use19Obj;
    private GameObject[] objMapTipList = new GameObject[1];     // マップチップリスト
    private Material material;                                  // 壁のマテリアル
    // ------------------------------------------------------------------

    // マップ関連 -------------------------------------------------------
    // 2Dマップ生成スクリプト用
    private DungeonGenerator dungeonGenerator;
    private int[,] map;
    public int[,] Originalmap;
    //private int[,] Originalmap = { {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //                               {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
    //                               {0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0},
    //                               {0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
    //                               {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //                               {0, 1, 1, 1, 1, 3, 1, 1, 1, 1, 0},//上
    //                               {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //                               {0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
    //                               {0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0},
    //                               {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
    //                               {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
    //                                            //↓

    // パス読み込み用
    private GameObject objMap2D;                //Map2D

    // 生成したマップチップ
    GameObject[,] objMapExist;                  //フィールド用

    //部屋探索中フラグ
    public bool flgSearchMap;

    public List<GameObject> GrassList = new List<GameObject>();
    // ------------------------------------------------------------------

    // --- 敵データ ---
    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();
    public EnemyData GetEnemyData()
    {
        return enemyData;
    }
    void Start()
    {
        
        PlayerObj = GameObject.Find("Player");  // プレイヤーObjを探す

        GenerateObj(Originalmap, PlayerObj);    // プレイヤーを生成

        //GenerateObj(Originalmap, EnemyObj);     // 敵を生成

        //GenerateEnemy(Originalmap, EnemyObj, new Vector2(5.0f, 7.0f));

        //GenerateEnemy(Originalmap, LateralMove_Enemy, new Vector2(7.0f, 3.0f));
        //GenerateEnemy(Originalmap, LateralMove_Enemy, new Vector2(9.0f, 5.0f));
        //GenerateEnemy(Originalmap, LateralMove_Enemy, new Vector2(7.0f, 7.0f));
        //GenerateEnemy(Originalmap, VerticalMove_Enemy, new Vector2(3.0f, 7.0f));

        foreach(var data in enemyData.enemies)
        {
            GameObject enemy;
            enemy = (GameObject)Resources.Load(data.GetAddress());
            GameObject objInstant = Instantiate(enemy, new Vector3(data.GetPos().x, 2.0f, enemyData.GetMapSize()-1-data.GetPos().y), Quaternion.Euler(0f, 0f, 0f));
            objInstant.GetComponent<EnemyBase>().SetEnemy(data);
            enemies.Add(objInstant);
        }
    }

    void Awake()
    {
            Debug.Assert(enemyData != null, "StageMakeにマップ情報をセットしてください。");
        int start = enemyData.GetMapSize() * (enemyData.GetMapSize() -1);
        int minus = enemyData.GetMapSize();
        Originalmap = new int[enemyData.GetMapSize(), enemyData.GetMapSize()];
        for (int yy = 0; yy < enemyData.GetMapSize(); yy++)
            for (int xx = 0; xx < enemyData.GetMapSize(); xx++)
            {
                if(enemyData.GetMap()[(start + yy) - minus * xx]==-1)
                    Originalmap[yy, xx] =-1;
                else
                    Originalmap[yy, xx] = enemyData.GetMap()[(start + yy) - minus * xx] % 100;
            }
                
        //Originalmap[yy, xx] = enemyData.GetMap()[enemyData.GetMapSize() * enemyData.GetMapSize() - 1 - (xx * enemyData.GetMapSize() + yy)];
        //リソース読み込み
        ReadResources();

        //壁を生成
        GenerateWall();
    }

    //リソース読み込み
    void ReadResources()
    {
        // 壁のマテリアル
        material = (Material)Resources.Load("Materials/NewWall");

        // ゴール
        GoalObj = (GameObject)Resources.Load("Prefabs/Goal");

        useObj = (GameObject)Resources.Load("Prefabs/Temp_Grass");
        use1Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 1");
        use2Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 2");
        use3Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 3");
        use4Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 4");
        use5Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 5");
        use6Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 6");
        use7Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 7");
        use8Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 8");
        use9Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 9");
        use10Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 10");
        use11Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 11");
        use12Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 12");
        use13Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 13");
        use14Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 14");
        use15Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 15");
        use16Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 16");
        use17Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 17");
        use18Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 18");
        use19Obj = (GameObject)Resources.Load("Prefabs/Temp_Grass 19");

        //EnemyObj = (GameObject)Resources.Load("Prefabs/Enemies/Skeleton");

        //VerticalMove_Enemy = (GameObject)Resources.Load("Prefabs/Enemies/Enemy_02");
        //LateralMove_Enemy = (GameObject)Resources.Load("Prefabs/Enemies/Enemy_03");

        ObstacleObj = (GameObject)Resources.Load("Prefabs/obstacle");

        // 敵リスト
        EnemyObjList = Resources.LoadAll<GameObject>("Prefabs/Enemies");

        // 2Dのマップチップ読み込み
        objMapTipList[0] = (GameObject)Resources.Load("Prefabs/UI/MapTip");

        // 地面のマテリアル
        blockPrefab_Grass = (GameObject)Resources.Load("Prefabs/Grass");
        blockPrefab_Water = (GameObject)Resources.Load("Prefabs/Water");
    }

    //壁を生成
    private void GenerateWall()
    {
        // ダンジョンマップ
        dungeonGenerator = GetComponent<DungeonGenerator>();

        // 壁の高さ
        float blockHeight = 2.0f;

        // 2Dマップ生成
        map = dungeonGenerator.Generate();

        // 生成する壁の親となるGameObject
        GameObject objWall = GameObject.Find("Wall");

        // 自動生成したマップにCubeを配置
        for (int i = 0; i < Originalmap.GetLength(0); i++)
        {
            for (int j = 0; j < Originalmap.GetLength(1); j++)
            {
                //壁をCubeで作成
                if (Originalmap[i, j] == 0)
                {
                    for (int k = 0; k < blockHeight; k++)
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        //Wall直下に階層を移動
                        cube.transform.parent = objWall.transform;
                        cube.GetComponent<Renderer>().material = material;
                        cube.transform.localScale = new Vector3(1, 1.5f, 1);
                        cube.transform.position = new Vector3(i, 1.5f, j);
                    }
                }

                // 地面を生成
                if (Originalmap[i, j] == 1 || Originalmap[i, j] == 2)
                {
                    GameObject obj;
                    obj = Instantiate(blockPrefab_Grass, blockParent);
                    obj.transform.position = new Vector3(i, 1.0f, j);

                    GrassList.Add(obj);
                }

                // 障害物を生成
                if (Originalmap[i, j] == 2)
                {
                    GameObject obj;
                    obj = Instantiate(ObstacleObj, blockParent);
                    obj.transform.position = new Vector3(i, 2.0f, j);
                }

                // ゴールを生成
                if (Originalmap[i, j] == 3)
                {
                    GameObject obj;
                    obj = Instantiate(GoalObj, blockParent);
                    obj.transform.position = new Vector3(i, 1.0f, j);
                }

                // を生成
                if (Originalmap[i, j] == 4)
                {
                    GameObject obj;
                    obj = Instantiate(useObj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 5)
                {
                    GameObject obj;
                    obj = Instantiate(use1Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 6)
                {
                    GameObject obj;
                    obj = Instantiate(use2Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 7)
                {
                    GameObject obj;
                    obj = Instantiate(use3Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 8)
                {
                    GameObject obj;
                    obj = Instantiate(use4Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 9)
                {
                    GameObject obj;
                    obj = Instantiate(use5Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 10)
                {
                    GameObject obj;
                    obj = Instantiate(use6Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 11)
                {
                    GameObject obj;
                    obj = Instantiate(use7Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 12)
                {
                    GameObject obj;
                    obj = Instantiate(use8Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 13)
                {
                    GameObject obj;
                    obj = Instantiate(use9Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 14)
                {
                    GameObject obj;
                    obj = Instantiate(use10Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 15)
                {
                    GameObject obj;
                    obj = Instantiate(use11Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 16)
                {
                    GameObject obj;
                    obj = Instantiate(use12Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 17)
                {
                    GameObject obj;
                    obj = Instantiate(use13Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 18)
                {
                    GameObject obj;
                    obj = Instantiate(use14Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 19)
                {
                    GameObject obj;
                    obj = Instantiate(use15Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 20)
                {
                    GameObject obj;
                    obj = Instantiate(use16Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 21)
                {
                    GameObject obj;
                    obj = Instantiate(use17Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 22)
                {
                    GameObject obj;
                    obj = Instantiate(use18Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }
                if (Originalmap[i, j] == 23)
                {
                    GameObject obj;
                    obj = Instantiate(use19Obj, blockParent);
                    obj.transform.localScale = new Vector3(1, 1.5f, 1);
                    obj.transform.position = new Vector3(i, 1.5f, j);
                }

                // 通路を生成
                //if(map[i, j] == 2)
                //{
                //    GameObject obj;
                //    obj = Instantiate(blockPrefab_Water, blockParent);
                //    obj.transform.position = new Vector3(i, 1.0f, j);
                //}
            }
        }
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    //オブジェクト生成 (プレイヤー以外)
    private void GenerateObj(int[,] Originalmap, GameObject obj)
    {
        //while (true)
        //{
            // 左下に生成
            int Player_Pos_X = (int)enemyData.GetHeroPos().x;
            //int Player_Pos_X = Originalmap.GetLength(1) - 2;
            int Player_Pos_Y = enemyData.GetMapSize() - 1 - (int)enemyData.GetHeroPos().y;

            //int mapX = Random.Range(0, Originalmap.GetLength(0) - 1);
            //int mapY = Random.Range(0, Originalmap.GetLength(1) - 1);

            // 右上に生成
            //int Enemy_Pos_X = Originalmap.GetLength(0) - 2;
            //int Enemy_Pos_X = 3;
            //int Enemy_Pos_Y = 6;
            //int Enemy_Pos_Y = Originalmap.GetLength(1) - 2;

            if (Originalmap[Player_Pos_X, Player_Pos_Y] == 1)
            {
                // プレイヤーは生成済みのため移動だけ
                if (obj.CompareTag("Player") == true)
                {
                    obj.transform.position = new Vector3(Player_Pos_X, 2.0f, Player_Pos_Y);

                    PlayerPos.x = Player_Pos_X;
                    PlayerPos.y = Player_Pos_Y;
                }
                
                // エネミー
                //else if (obj.CompareTag("Enemy") == true)
                //{
                //    GameObject objInstant = Instantiate(obj, new Vector3(Enemy_Pos_X, 2.0f, Enemy_Pos_Y), Quaternion.Euler(0f, 0f, 0f));
                //}
                //その他は生成と移動
                //else
                //{
                //    GameObject objInstant = Instantiate(obj, new Vector3(mapX, 2.0f, mapY), Quaternion.Euler(0f, 0f, 0f));
                //}
                //break;
            }
            Debug.Assert(Originalmap[Player_Pos_X, Player_Pos_Y] == 1, "マップエディタ状のPlayerの位置を確認してください");
        //}
    }

    //オブジェクト生成
    private void GenerateEnemy(int[,] Originalmap, GameObject obj, Vector2 pos)
    {
       GameObject objInstant = Instantiate(obj, new Vector3(pos.x, 2.0f, pos.y), Quaternion.Euler(0f, 0f, 0f));
    }

    public int[,] GetMapGenerate()
    {
        return Originalmap;
    }

    public Vector2 GetMapPlayer()
    {
        return PlayerPos;
    }
}