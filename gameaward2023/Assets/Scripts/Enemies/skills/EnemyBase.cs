using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int health;
    public int skillTurn;
    public SKILL skill;
    public Vector2 startPos;
    protected Vector3 CurrentPos = Vector3.zero;  // スケルトン座標格納用
    public string tag;
    public NAME name;
    public int skillStep;
    public bool chargedSkill = false;
    protected bool inSkill = false; // スキル発動中
    public float moveSpeed;
    protected bool onMove = false;// 通常移動中
    protected int SkeletonDir = 0;
    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private PlayerMovement PlayerMovement;
    // ------------------------------------------------------------------
    // 2Dマップ生成スクリプト用 -----------------------------------------
    private GameObject StageMake;               // 
    private ElementGenerator elementGenerator;  // 
    protected int[,] map;                         // マップ情報格納用
    private bool bMapLoading = false;           // マップがロードされたか
    // -------------------------------------------------------------------
    virtual protected void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
        skillStep = PlayerMovement.StepCount;
        // イベントの登録（プレイヤーが移動したら関数を呼ぶ）
        PlayerMovement.Walk += OnChargeSkill;

        StageMake = GameObject.Find("StageMake");
        // ダンジョンマップ読み込み
        elementGenerator = StageMake.GetComponent<ElementGenerator>();
        map = elementGenerator.GetMapGenerate();

        // スポーン時の座標を格納
        CurrentPos = this.gameObject.transform.position;

        bMapLoading = true;
        
    }
    //virtual protected void Update()
    //{
    //    // 一度だけ実行
    //    if (!bMapLoading)
    //    {
    //        // ダンジョンマップ読み込み
    //        elementGenerator = StageMake.GetComponent<ElementGenerator>();
    //        map = elementGenerator.GetMapGenerate();

    //        // スポーン時の座標を格納
    //        CurrentPos = this.gameObject.transform.position;

    //        bMapLoading = true;
    //    }
    //}
    // 生成時の設定用関数
    public void SetEnemy(Enemy data)
    {
        health = data.GetHealth();
        skillTurn = data.GetSkillTurn();
        skill = data.GetSkill();
        startPos = data.GetPos();
        name = data.GetName();
        tag = data.GetTag();
        moveSpeed = data.GetMoveSpeed();
    }

    // スキルチャージ関数
    private void OnChargeSkill()
    {
        skillStep++;
        skillStep %= skillTurn+1;

        chargedSkill = (skillStep == skillTurn) ? true : false;
        Debug.Log(name + " : " + tag + "<"+skillStep+"/"+skillTurn+">");
    }

    // --- スキル　---
    protected void SkillActivation()
    {
        inSkill = true;
        switch (skill)
        {
            case SKILL.down:
                {
                    WalkDownSkill();
                    break;
                }
            case SKILL.left:
                {
                    WalkLeftSkill();
                    break;
                }
            case SKILL.right:
                {
                    WalkRightSkill();
                    break;
                }
            case SKILL.up:
                {
                    WalkUpSkill();
                    break;
                }
            case SKILL.invisible:
                {
                    InvisibleSkill();
                    break;
                }
            case SKILL none:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    // 左に1マス進む
    protected void WalkLeftSkill()
    {
        // 壁があるか
        if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 0 ||
                map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2)
        {
            CurrentPos.x -= 0.0f;
        }
        else if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 1 ||
                map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 3)
        {
            CurrentPos.x -= 1.0f;
        }
        SkeletonDir = 2;
        // 向き調整
        //this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
        // 座標更新
        //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, CurrentPos, moveSpeed * Time.deltaTime);
    }

    // 右に1マス進む
    protected void WalkRightSkill()
    {
        // 壁があるか
        if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 0 ||
                map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 2)
        {
            CurrentPos.x += 0.0f;
        }
        else if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 1 ||
                map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 3)
        {
            CurrentPos.x += 1.0f;
        }
        SkeletonDir = 3;
        // 向き調整
        //this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        // 座標更新
        //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, CurrentPos, moveSpeed * Time.deltaTime);
    }

    // 上に1マス進む
    protected void WalkUpSkill()
    {
        // 壁があるか
        if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 0 ||
               map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 2)
        {
            CurrentPos.z += 0.0f;
        }
        else if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 1 ||
               map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 3)
        {
            CurrentPos.z += 1.0f;
        }
        SkeletonDir = 0;
        // 向き調整
        //this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        // 座標更新
        //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, CurrentPos, moveSpeed * Time.deltaTime);
    }

    // 下に1マス進む
    protected void WalkDownSkill()
    {
        // 壁があるか
        if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 0 ||
                map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 2)
        {
            CurrentPos.z -= 0.0f;
        }
        else if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 1 ||
                map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 3)
        {
            CurrentPos.z -= 1.0f;
        }

        SkeletonDir = 1;
        // 向き調整
        //this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        // 座標更新
        //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, CurrentPos, moveSpeed * Time.deltaTime);
    }

    //  透明化
    protected void InvisibleSkill()
    {
        SpriteRenderer body2D = GetComponent<SpriteRenderer>();
        if (body2D != null)
        {
            body2D.enabled = !body2D.enabled;
        }
        else
        {
            MeshRenderer body3D = GetComponent<MeshRenderer>();
            if(body3D != null)
                body3D.enabled = !body3D.enabled;
        }
    }
}
