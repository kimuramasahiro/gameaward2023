using System.Collections;
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
    private GameObject[] objMapTipList = new GameObject[1];     // マップチップリスト
    private Material material;                                  // 壁のマテリアル
    // ------------------------------------------------------------------

    // マップ関連 -------------------------------------------------------
    // 2Dマップ生成スクリプト用
    private DungeonGenerator dungeonGenerator;
    private int[,] map;

    private int[,] Originalmap = { {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                   {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                                   {0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0},
                                   {0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
                                   {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                                   {0, 1, 1, 1, 1, 3, 1, 1, 1, 1, 0},//上
                                   {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                                   {0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
                                   {0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0},
                                   {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                                   {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
                                                //↓

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
            GameObject objInstant = Instantiate(enemy, new Vector3(data.GetPos().x, 2.0f, data.GetPos().y), Quaternion.Euler(0f, 0f, 0f));
            objInstant.GetComponent<EnemyBase>().SetEnemy(data);
            enemies.Add(objInstant);
        }
    }

    void Awake()
    {
        //リソース読み込み
        ReadResources();

        //壁を生成
        GenerateWall();
    }

    //リソース読み込み
    void ReadResources()
    {
        // 壁のマテリアル
        material = (Material)Resources.Load("Materials/Water002");

        // ゴール
        GoalObj = (GameObject)Resources.Load("Prefabs/Goal");

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
                        cube.transform.localScale = new Vector3(1, 1, 1);
                        cube.transform.position = new Vector3(i, 0.8f, j);
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
        while (true)
        {
            // 左下に生成
            int Player_Pos_X = 5;
            //int Player_Pos_X = Originalmap.GetLength(1) - 2;
            int Player_Pos_Y = 1;
            //int mapX = Random.Range(0, Originalmap.GetLength(0) - 1);
            //int mapY = Random.Range(0, Originalmap.GetLength(1) - 1);

            // 右上に生成
            //int Enemy_Pos_X = Originalmap.GetLength(0) - 2;
            int Enemy_Pos_X = 3;
            int Enemy_Pos_Y = 6;
            //int Enemy_Pos_Y = Originalmap.GetLength(1) - 2;

            if (Originalmap[Player_Pos_X, Player_Pos_Y] == 1)
            {
                // プレイヤーは生成済みのため移動だけ
                if (obj.CompareTag("Player") == true)
                {
                    obj.transform.position = new Vector3(Player_Pos_X, 1.5f, Player_Pos_Y);

                    PlayerPos.x = Player_Pos_X;
                    PlayerPos.y = Player_Pos_Y;
                }
                // エネミー
                else if (obj.CompareTag("Enemy") == true)
                {
                    GameObject objInstant = Instantiate(obj, new Vector3(Enemy_Pos_X, 2.0f, Enemy_Pos_Y), Quaternion.Euler(0f, 0f, 0f));
                }
                //その他は生成と移動
                //else
                //{
                //    GameObject objInstant = Instantiate(obj, new Vector3(mapX, 2.0f, mapY), Quaternion.Euler(0f, 0f, 0f));
                //}
                break;
            }
        }
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