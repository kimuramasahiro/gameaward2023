using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement :EnemyBase
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

        //CheckPlayerWithToNear();

        // 進める状態か
        if (IsMovable)
        {
            MoveSkeleton();

            // プレイヤーが進んだら
            if (PlayerMovement.IsMoving)
            {
                
                //// プレイヤーが近くにいる場合
                //if (bPlayerNear)
                //{
                //    // プレイヤーに近づく処理
                //    LockOnMove();
                //}
                //// プレイヤーが近くにいない場合
                //else
                //{
                //    // ランダムに進む処理
                //    MoveRandomDir();
                //}

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

    private void MoveRandomDir()
    {
        // 進む方向を決める
        MoveDir = UnityEngine.Random.Range(1, 5);   // 1～4の値をランダムで抽出する

        // 進む先に壁があれば止まり、なければ１マス進む
        switch (MoveDir)
        {
            // 前に進む場合
            case 1:
                // 壁があるか
                if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 0)
                {
                    CurrentPos.z += 0.0f;
                }
                else if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 1 ||
                       map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 2)
                {
                    CurrentPos.z += 1.0f;
                }
                break;

            // 後ろに進む場合
            case 2:
                // 壁があるか
                if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 0)
                {
                    CurrentPos.z -= 0.0f;
                }
                else if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 1 ||
                        map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 2)
                {
                    CurrentPos.z -= 1.0f;
                }
                break;

            // 左に進む場合
            case 3:
                // 壁があるか
                if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 0)
                {
                    CurrentPos.x -= 0.0f;
                }
                else if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 1 ||
                        map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2)
                {
                    SkeletonDir = true;
                    CurrentPos.x -= 1.0f;
                }
                break;

            // 右に進む場合
            case 4:
                // 壁があるか
                if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 0)
                {
                    CurrentPos.x += 0.0f;
                }
                else if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 1 ||
                        map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 2)
                {
                    SkeletonDir = false;
                    CurrentPos.x += 1.0f;
                }
                break;
        }
    }

    private void CheckPlayerWithToNear()
    {
        // スケルトン座標格納
        CheckPos.x = CurrentPos.x;
        CheckPos.y = CurrentPos.z;

        // プレイヤー座標格納
        PlayerCurrentPos.x = PlayerObj.transform.position.x;
        PlayerCurrentPos.y = PlayerObj.transform.position.z;

        // プレイヤーが近くにいないか確認する
        // 
        if (PlayerCurrentPos.x >= CheckPos.x - SearchRange &&
            PlayerCurrentPos.x <= CheckPos.x + SearchRange)
        {
            if (PlayerCurrentPos.y >= CheckPos.y - SearchRange &&
                PlayerCurrentPos.y <= CheckPos.y + SearchRange)
            {
                bPlayerNear = true;
            }
            else
            {
                bPlayerNear = false;
            }
        }
        else
        {
            bPlayerNear = false;
        }
    }

    private void LockOnMove()
    {
        // 差分
        PosDiff.x = PlayerCurrentPos.x - CheckPos.x;
        PosDiff.y = PlayerCurrentPos.y - CheckPos.y;


        // 右
        if (PosDiff.y > 0)
        {
            // 壁があるか
            if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 0)
            {
                return;
            }
            else if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 1 ||
                    map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 2)
            {
                CurrentPos.z += 1.0f;
            }
        }
        // 左
        else if (PosDiff.y < 0)
        {
            if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 0)
            {
                return;
            }
            // 壁があるか
            else if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 1 ||
                    map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2)
            {
                CurrentPos.z -= 1.0f;
            }
        }

        // z座標の差分が0より大きかったら(プレイヤーより右にいる場合)
        // 右
        //if (PosDiff.x > 0)
        //{
        //    // 壁があるか
        //    if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 1 ||
        //        map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 2)
        //    {
        //        CurrentPos.x += 1.0f;
        //        SkeletonDir = false;
        //    }
        //}
        //// 左
        //else if(PosDiff.x < 0)
        //{
        //    // 壁があるか
        //    if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 1 ||
        //        map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2)
        //    {
        //        CurrentPos.x -= 1.0f;
        //        SkeletonDir = true;
        //    }
        //}
    }

    private void MoveSkeleton()
    {
        // プレイヤーが前に進んだら
        if (PlayerMovement.PressKey_W)
        {
            if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 0 || map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 2)
            {
                CurrentPos.z -= 0.0f;
            }
            else if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 1 || map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 3)
            {
                CurrentPos.z -= 1.0f;
            }

            onMove = true;
            //PlayerMovement.PressKey_W = false;
        }
        // プレイヤーが後ろに進んだら
        else if (PlayerMovement.PressKey_S)
        {
            // 壁があるか
            if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 0 || map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 2)
            {
                CurrentPos.z += 0.0f;
            }
            else if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 1 || map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 3)
            {
                CurrentPos.z += 1.0f;
            }

            onMove = true;
            //PlayerMovement.PressKey_S = false;
        }
        // プレイヤーが左に進んだら
        else if (PlayerMovement.PressKey_A)
        {
            // 壁があるか
            if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 2)
            {
                CurrentPos.x += 0.0f;
            }
            else if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 1 || map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 3)
            {
                SkeletonDir = false;
                CurrentPos.x += 1.0f;
            }

            onMove = true;
            //PlayerMovement.PressKey_A = false;
        }
        // プレイヤーが右に進んだら
        else if (PlayerMovement.PressKey_D)
        {
            if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2)
            {
                CurrentPos.x -= 0.0f;
            }
            else if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 1 || map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 3)
            {
                SkeletonDir = true;
                CurrentPos.x -= 1.0f;
            }

            onMove = true;
            //PlayerMovement.PressKey_D = false;
        }
    }
}
