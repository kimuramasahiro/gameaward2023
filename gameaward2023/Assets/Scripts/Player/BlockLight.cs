using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLight : MonoBehaviour
{
    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private Vector3 CurrentPos = Vector3.zero;  // プレイヤー座標格納用
    // ------------------------------------------------------------------

    // 2Dマップ生成スクリプト用 -----------------------------------------
    private GameObject StageMake;               // 
    private ElementGenerator elementGenerator;  // 
    private int[,] map;                         // マップ情報格納用
    private bool bMapLoading = false;           // マップがロードされたか
    // ------------------------------------------------------------------

    // 光らせるオブジェクト ---------------------------------------------
    private List<GameObject> ParentObj = new List<GameObject>();
    private List<GameObject> ChildObj = new List<GameObject>();
    private List<GameObject> LightObj = new List<GameObject>();
    // ------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        StageMake = GameObject.Find("StageMake");

        // ダンジョンマップ読み込み
        elementGenerator = StageMake.GetComponent<ElementGenerator>();
        map = elementGenerator.GetMapGenerate();

        ParentObj = elementGenerator.GrassList;

        for (int i = 0; i < ParentObj.Count; i++)
        {
            ChildObj.Add(ParentObj[i].transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < ParentObj.Count; i++)
        {
            LightObj.Add(ParentObj[i].transform.GetChild(1).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 一度だけ実行
        if (!bMapLoading)
        {
            //// ダンジョンマップ読み込み
            //elementGenerator = StageMake.GetComponent<ElementGenerator>();
            //map = elementGenerator.GetMapGenerate();

            //ParentObj = elementGenerator.GrassList;

            //for(int i = 0; i < ParentObj.Count; i++)
            //{
            //    ChildObj.Add(ParentObj[i].transform.GetChild(0).gameObject);
            //}

            //for (int i = 0; i < ParentObj.Count; i++)
            //{
            //   LightObj.Add(ParentObj[i].transform.GetChild(1).gameObject);
            //}

            bMapLoading = true;
        }

        // プレイヤー座標読み込み
        CurrentPos.x = PlayerObj.transform.position.x;
        CurrentPos.z = PlayerObj.transform.position.z;

        // プレイヤーの歩くことが可能なブロックを表示する
        PlayerWalkMark();
    }

    private void PlayerWalkMark()
    {
        for (int i = 0; i < ParentObj.Count; i++)
        {
            if ((int)ParentObj[i].transform.position.x == CurrentPos.x && (int)ParentObj[i].transform.position.z == CurrentPos.z + 1)
            {
                ChildObj[i].SetActive(true);
                //LightObj[i].SetActive(true);
            }
            else if ((int)ParentObj[i].transform.position.x == CurrentPos.x && (int)ParentObj[i].transform.position.z == CurrentPos.z - 1)
            {
                ChildObj[i].SetActive(true);
                //LightObj[i].SetActive(true);
            }
            else if ((int)ParentObj[i].transform.position.x == CurrentPos.x - 1 && (int)ParentObj[i].transform.position.z == CurrentPos.z)
            {
                ChildObj[i].SetActive(true);
                //LightObj[i].SetActive(true);
            }
            else if ((int)ParentObj[i].transform.position.x == CurrentPos.x + 1 && (int)ParentObj[i].transform.position.z == CurrentPos.z)
            {
                ChildObj[i].SetActive(true);
                //LightObj[i].SetActive(true);
            }
            else
            {
                ChildObj[i].SetActive(false);
            }
        }
    }
}
