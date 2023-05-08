using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerAttack : MonoBehaviour
{
    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private Vector3 CurrentPos = Vector3.zero;  // プレイヤー座標格納用

    public bool IsAttacking = false;            // プレイヤーが攻撃したか
    // ------------------------------------------------------------------

    // 2Dマップ生成スクリプト用 -----------------------------------------
    private GameObject StageMake;               // コンポーネントされているオブジェクト
    private ElementGenerator elementGenerator;  // ElementGeneratorクラス継承用
    private int[,] map;                         // マップ情報格納用
    private bool bMapLoading = false;           // マップがロードされたか
    // ------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
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

            // プレイヤー座標読み込み
            CurrentPos.x = elementGenerator.GetMapPlayer().x;
            CurrentPos.y = 1.5f;
            CurrentPos.z = elementGenerator.GetMapPlayer().y;

            bMapLoading = true;
        }
    }
}
