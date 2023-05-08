using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    // スケルトン関連 ---------------------------------------------------
    private GameObject SkeletonObj = null;      // スケルトンオブジェクト
    private Vector3 CurrentPos = Vector3.zero;  // スケルトン座標格納用
    // ------------------------------------------------------------------

    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private PlayerMovement PlayerMovement;
    // ------------------------------------------------------------------

    // 2Dマップ生成スクリプト用 -----------------------------------------
    private GameObject StageMake;               // 
    private ElementGenerator elementGenerator;  // 
    private int[,] map;                         // マップ情報格納用
    private bool bMapLoading = false;           // マップがロードされたか
    // ------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();

        StageMake = GameObject.Find("StageMake");
    }

    // Update is called once per frame
    void Update()
    {
        // 一度だけ実行
        if (!bMapLoading)
        {
            // ダンジョンマップ読み込み
            elementGenerator = StageMake.GetComponent<ElementGenerator>();
            map = elementGenerator.GetMapGenerate();

            // スポーン時の座標を格納
            CurrentPos = this.gameObject.transform.position;

            bMapLoading = true;
        }


    }
}
