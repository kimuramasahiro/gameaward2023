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

    // スキル関連 --------------------------------------------------------
    private GameObject ObstacleObj;                             // 障害物Obj

    private GameObject TargetObj = null;        // タップ対象のオブジェクト
    [SerializeField]
    private bool bSkill = false;
    // -------------------------------------------------------------------

    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();

        StageMake = GameObject.Find("StageMake");

        //リソース読み込み
        ReadResources();
    }

    void ReadResources()
    {
        ObstacleObj = (GameObject)Resources.Load("Prefabs/obstacle");
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

        // プレイヤーが動いていないとき かつ プレイヤーが４歩以上移動したときのみ発動可能
        if(!PlayerMovement.IsMoving && PlayerMovement.StepCount >= 4)
        { 
            bSkill = true;
        }
        else
        {
            bSkill = false;
        }

        if(bSkill)
        {
            // クリック検出処理
            if (Input.GetMouseButtonDown(0))
            {
                // 歩数リセット
                PlayerMovement.StepCount = 0;

                // クリックしたブロックを障害物に変更
                GetBlockTapPos();

                bSkill = false;
            }
        }
    }

    private void GetBlockTapPos()
    {
        // クリックした方向にカメラからRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            // Rayに当たる位置に存在するオブジェクトを取得
            TargetObj = hit.collider.gameObject;
        }

        // 対象オブジェクト(ブロック)が存在する場合の処理
        if (TargetObj != null)
        {
            // ブロック選択時処理
            ChangeBlock(TargetObj.GetComponent<MapBlock>());
        }
        else
        {
            Debug.Log("指定の場所には使えません");
        }
    }

    private void ChangeBlock(MapBlock targetBlock)
    {
        // 選択した地面を障害物に変更
        elementGenerator.Originalmap[(int)targetBlock.transform.position.x, (int)targetBlock.transform.position.z] = 2;

        // 地面を削除
        TargetObj.SetActive(false);

        // 障害物を生成
        GameObject obj;
        obj = Instantiate(ObstacleObj);
        obj.transform.position = new Vector3(targetBlock.transform.position.x, 2.0f, targetBlock.transform.position.z);
    }

    public int[,] GetUpdateMap()
    {
        return map;
    }
}
