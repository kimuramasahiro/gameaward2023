using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ElementGenerator : MonoBehaviour
{
    // �v���C���[�֘A ---------------------------------------------------
    private Vector2 PlayerPos;              // �v���C���[���W�i�[�p
    // ------------------------------------------------------------------

    // �G�l�~�[�֘A -----------------------------------------------------
    // �����p�p�����[�^�[
    [SerializeField] int MaxEnemy;  // ���������̓G�̉�����
    [SerializeField] int MinEnemy;  // ���������̓G�̏����
    // ------------------------------------------------------------------

    // ���\�[�X�i�[�p (�K�w�܂����̃Z�[�u/���[�h�Ŏg�p) -----------------
    public Transform blockParent;                               // �eObj
    private GameObject blockPrefab_Grass;                       // �n��Obj
    private GameObject blockPrefab_Water;                       // ��Obj
    private GameObject PlayerObj;                               // �v���C���[Obj
    private GameObject EnemyObj;
    private GameObject[] EnemyObjList = new GameObject[1];      // �G���X�gObj
    private GameObject GoalObj;                                 // �S�[��Obj
    private GameObject ObstacleObj;                             // ��Q��Obj
    private GameObject[] objMapTipList = new GameObject[1];     // �}�b�v�`�b�v���X�g
    private Material material;                                  // �ǂ̃}�e���A��
    // ------------------------------------------------------------------

    // �}�b�v�֘A -------------------------------------------------------
    // 2D�}�b�v�����X�N���v�g�p
    private DungeonGenerator dungeonGenerator;
    private int[,] map;

    private int[,] Originalmap = { { 0, 0, 0, 0, 0, 0, 0},
                                   { 0, 1, 1, 1, 1, 1, 0},
                                   { 0, 1, 1, 1, 2, 1, 0},
                                   { 0, 1, 1, 3, 1, 1, 0},
                                   { 0, 1, 1, 1, 1, 1, 0},
                                   { 0, 1, 1, 1, 1, 1, 0},
                                   { 0, 0, 0, 0, 0, 0, 0}};


    // �p�X�ǂݍ��ݗp
    private GameObject objMap2D;                //Map2D

    // ���������}�b�v�`�b�v
    GameObject[,] objMapExist;                  //�t�B�[���h�p

    //�����T�����t���O
    public bool flgSearchMap;

    public List<GameObject> GrassList = new List<GameObject>();
    // ------------------------------------------------------------------

    void Start()
    {
        PlayerObj = GameObject.Find("Player");  // �v���C���[Obj��T��

        GenerateObj(Originalmap, PlayerObj);    // �v���C���[�𐶐�

        GenerateObj(Originalmap, EnemyObj);     // �G�𐶐�
    }

    void Awake()
    {
        //���\�[�X�ǂݍ���
        ReadResources();

        //�ǂ𐶐�
        GenerateWall();
    }

    //���\�[�X�ǂݍ���
    void ReadResources()
    {
        // �ǂ̃}�e���A��
        material = (Material)Resources.Load("Materials/Water002");

        // �S�[��
        GoalObj = (GameObject)Resources.Load("Prefabs/Goal");

        EnemyObj = (GameObject)Resources.Load("Prefabs/Enemies/Skeleton");

        ObstacleObj = (GameObject)Resources.Load("Prefabs/obstacle");

        // �G���X�g
        EnemyObjList = Resources.LoadAll<GameObject>("Prefabs/Enemies");

        // 2D�̃}�b�v�`�b�v�ǂݍ���
        objMapTipList[0] = (GameObject)Resources.Load("Prefabs/UI/MapTip");

        // �n�ʂ̃}�e���A��
        blockPrefab_Grass = (GameObject)Resources.Load("Prefabs/Grass");
        blockPrefab_Water = (GameObject)Resources.Load("Prefabs/Water");
    }

    //�ǂ𐶐�
    private void GenerateWall()
    {
        // �_���W�����}�b�v
        dungeonGenerator = GetComponent<DungeonGenerator>();

        // �ǂ̍���
        float blockHeight = 2.0f;

        // 2D�}�b�v����
        map = dungeonGenerator.Generate();

        // ��������ǂ̐e�ƂȂ�GameObject
        GameObject objWall = GameObject.Find("Wall");

        // �������������}�b�v��Cube��z�u
        for (int i = 0; i < Originalmap.GetLength(0); i++)
        {
            for (int j = 0; j < Originalmap.GetLength(1); j++)
            {
                //�ǂ�Cube�ō쐬
                if (Originalmap[i, j] == 0)
                {
                    for (int k = 0; k < blockHeight; k++)
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        //Wall�����ɊK�w���ړ�
                        cube.transform.parent = objWall.transform;
                        cube.GetComponent<Renderer>().material = material;
                        cube.transform.localScale = new Vector3(1, 1, 1);
                        cube.transform.position = new Vector3(i, 0.8f, j);
                    }
                }

                // �n�ʂ𐶐�
                if (Originalmap[i, j] == 1 || Originalmap[i, j] == 2)
                {
                    GameObject obj;
                    obj = Instantiate(blockPrefab_Grass, blockParent);
                    obj.transform.position = new Vector3(i, 1.0f, j);

                    GrassList.Add(obj);
                }

                // ��Q���𐶐�
                if (Originalmap[i, j] == 2)
                {
                    GameObject obj;
                    obj = Instantiate(ObstacleObj, blockParent);
                    obj.transform.position = new Vector3(i, 2.0f, j);
                }

                // �S�[���𐶐�
                if (Originalmap[i, j] == 3)
                {
                    GameObject obj;
                    obj = Instantiate(GoalObj, blockParent);
                    obj.transform.position = new Vector3(i, 1.0f, j);
                }

                // �ʘH�𐶐�
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

    //�I�u�W�F�N�g���� (�v���C���[�ȊO)
    private void GenerateObj(int[,] Originalmap, GameObject obj)
    {
        while (true)
        {
            // �����ɐ���
            //int Player_Pos_X = 1;
            int Player_Pos_X = Originalmap.GetLength(1) - 2;
            int Player_Pos_Y = 1;
            //int mapX = Random.Range(0, Originalmap.GetLength(0) - 1);
            //int mapY = Random.Range(0, Originalmap.GetLength(1) - 1);

            // �E��ɐ���
            //int Enemy_Pos_X = Originalmap.GetLength(0) - 2;
            int Enemy_Pos_X = 1;
            int Enemy_Pos_Y = Originalmap.GetLength(1) - 2;

            if (Originalmap[Player_Pos_X, Player_Pos_Y] == 1)
            {
                // �v���C���[�͐����ς݂̂��߈ړ�����
                if (obj.CompareTag("Player") == true)
                {
                    obj.transform.position = new Vector3(Player_Pos_X, 1.5f, Player_Pos_Y);

                    PlayerPos.x = Player_Pos_X;
                    PlayerPos.y = Player_Pos_Y;
                }
                // �G�l�~�[
                else if (obj.CompareTag("Enemy") == true)
                {
                    GameObject objInstant = Instantiate(obj, new Vector3(Enemy_Pos_X, 2.0f, Enemy_Pos_Y), Quaternion.Euler(0f, 0f, 0f));
                }
                //���̑��͐����ƈړ�
                //else
                //{
                //    GameObject objInstant = Instantiate(obj, new Vector3(mapX, 2.0f, mapY), Quaternion.Euler(0f, 0f, 0f));
                //}
                break;
            }
        }
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