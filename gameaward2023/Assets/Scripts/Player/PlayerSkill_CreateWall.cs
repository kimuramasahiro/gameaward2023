using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill_CreateWall : MonoBehaviour
{
    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private PlayerMovement PlayerMovement;
    // ------------------------------------------------------------------

    // 2Dマップ生成スクリプト用 -----------------------------------------
    private GameObject StageMake;               // 
    private ElementGenerator elementGenerator;  // 
    private int[,] map;                         // マップ情報格納用
    private bool bMapLoading = false;           // マップがロードされたか
    // -------------------------------------------------------------------

    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();

        StageMake = GameObject.Find("StageMake");
    }

    void Update()
    {
        // 一度だけ実行
        if (!bMapLoading)
        {
            // ダンジョンマップ読み込み
            elementGenerator = StageMake.GetComponent<ElementGenerator>();
            map = elementGenerator.GetMapGenerate();

            bMapLoading = true;
        }

        // プレイヤーが動いていないとき
        if (!PlayerMovement.IsMoving)
        {

        }
    }
}
