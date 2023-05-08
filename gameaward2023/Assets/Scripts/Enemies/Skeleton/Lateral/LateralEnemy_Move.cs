using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralEnemy_Move : EnemyBase
{
    // スケルトン関連 ---------------------------------------------------
    private GameObject SkeletonObj = null;      // スケルトンオブジェクト
    //private Vector3 CurrentPos = Vector3.zero;  // スケルトン座標格納用
    private Vector2 CheckPos = Vector2.zero;    // 
    public float MoveSpeed = 3.0f;              // スケルトンの動く速さ
    private int MoveDir = 0;                    // スケルトンの進む方向
    private bool SkeletonDir = false;           // スケルトンの向き(false:右 true:左)
    public bool IsMovable = true;               // スケルトンが進めるか(true:進める false:進めない)

    private int SearchRange = 3;                // 索敵範囲(3の場合,自分中心から縦横3マス)
    [SerializeField]
    private bool bPlayerNear = false;
    [SerializeField]
    private Vector2 PosDiff = Vector2.zero;     // スケルトンとプレイヤーの座標の差

    private bool bTouch = false;
    private bool isTouched_A_Wall = false;
    private bool isTouched_D_Wall = false;
    // ------------------------------------------------------------------

    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private PlayerMovement PlayerMovement;

    private Vector2 PlayerCurrentPos = Vector2.zero;
    // ------------------------------------------------------------------

    // // 2Dマップ生成スクリプト用 -----------------------------------------
    // private GameObject StageMake;               // 
    // private ElementGenerator elementGenerator;  // 
    // private int[,] map;                         // マップ情報格納用
    // private bool bMapLoading = false;           // マップがロードされたか
    //// -------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();

        //StageMake = GameObject.Find("StageMake");
    }

    // Update is called once per frame
    void Update()
    {
        //base.Update();
        //// 一度だけ実行
        //if (!bMapLoading)
        //{
        //    // ダンジョンマップ読み込み
        //    elementGenerator = StageMake.GetComponent<ElementGenerator>();
        //    map = elementGenerator.GetMapGenerate();

        //    // スポーン時の座標を格納
        //    CurrentPos = this.gameObject.transform.position;

        //    bMapLoading = true;
        //}

        // 進んでいないときtrueにする
        if(CurrentPos == this.gameObject.transform.position)
        {
            IsMovable = true;

            if (inSkill)
            {
                chargedSkill = false;
                inSkill = false;
            }
            if (onMove && chargedSkill)
            {
                SkillActivation();
            }
            onMove = false;
        }

        // 進める状態か
        if (IsMovable)
        {
            MoveEnemy();
            

            // プレイヤーが進んだら
            if (PlayerMovement.IsMoving)
            {
                // 進んだらfalse
                IsMovable = false;
            }
        }

        // 進む方向にプレイヤーを向かせる
        if (!SkeletonDir)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        // 座標更新
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, CurrentPos, MoveSpeed * Time.deltaTime);
    }

    private void MoveEnemy()
    {
        if((map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2)&&
            (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 2))
        {
            bTouch = true;
            isTouched_A_Wall = true;
            isTouched_D_Wall = true;
        }
        else if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2)
        {
            bTouch = true;
            isTouched_A_Wall = true;
            isTouched_D_Wall = false;
        }
        else if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 2)
        {
            bTouch = true;
            isTouched_A_Wall = false;
            isTouched_D_Wall = true;
        }
        else
        {
            bTouch = false;
            isTouched_A_Wall = false;
            isTouched_D_Wall = false;
        }


        // プレイヤーが移動したら
        if (PlayerMovement.PressKey_W || PlayerMovement.PressKey_A || PlayerMovement.PressKey_S || PlayerMovement.PressKey_D)
        {
            if(isTouched_A_Wall&&isTouched_D_Wall)
            {
                CurrentPos.x += 0.0f;
            }
            else if(isTouched_A_Wall)
            {
                SkeletonDir = false;
                CurrentPos.x += 1.0f;
            }
            else if(isTouched_D_Wall)
            {
                SkeletonDir = true;
                CurrentPos.x -= 1.0f;
            }
            else
            {
                if(SkeletonDir)
                {
                    CurrentPos.x -= 1.0f;
                }
                else
                {
                    CurrentPos.x += 1.0f;
                }
            }
            onMove = true;
        }
    }
}
