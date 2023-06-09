using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_UI : MonoBehaviour
{
    private Transform SkeletonTrasform;
    private GameObject SkeletonObj;
    private SkeletonMovement SkeletonMovement;

    private GameObject[] UpUI = new GameObject[1];       // 上移動UI
    private GameObject[] DownUI = new GameObject[1];     // 下移動UI
    private GameObject[] LeftUI = new GameObject[1];     // 左移動UI
    private GameObject[] RightUI = new GameObject[1];    // 右移動UI
    private GameObject[] InvisibleUI = new GameObject[1];    // 透明UIリスト
    private GameObject objUI;      // 生成するObj
    private int nSkillTurn = 0;    // スキルターン
    private bool bCreate = false;  // 生成されたか
    private int nChildCount = 0;   // 子Objの数
    private GameObject[] SkillTurnObj;  // 
    private SKILL skill;

    void Start()
    {
        SkeletonObj = this.gameObject;
        SkeletonTrasform = SkeletonObj.transform.GetChild(1).gameObject.transform;
        SkeletonMovement = SkeletonObj.GetComponent<SkeletonMovement>();

        // スキルターン
        nSkillTurn = SkeletonMovement.skillTurn;

        //リソース読み込み
        ReadResources();
    }

    void ReadResources()
    {
        UpUI = Resources.LoadAll<GameObject>("Prefabs/UI_UpGage");          // 上移動UIリストを読み込む
        DownUI = Resources.LoadAll<GameObject>("Prefabs/UI_DownGage");      // 下移動UIリストを読み込む
        LeftUI = Resources.LoadAll<GameObject>("Prefabs/UI_LeftGage");      // 左移動UIリストを読み込む
        RightUI = Resources.LoadAll<GameObject>("Prefabs/UI_RightGage");    // 右移動UIリストを読み込む
        InvisibleUI = Resources.LoadAll<GameObject>("Prefabs/UI_TomeiGage");    // 透明UIリストを読み込む
    }

    void Update()
    {
        if (!bCreate)
        {
            // 敵のスキルによって表示するUIを変更する
            EnemySkillUI();

            // 初期設定
            objUI.transform.SetParent(SkeletonTrasform, false);
            objUI.transform.localPosition = Vector3.zero;
            //objUI.transform.localRotation = new Quaternion(20.0f, 0.0f, 0.0f, 0.0f);
            //objUI.transform.localScale = new Vector3(0.001f, 0.001f, 1.0f);

            // 子Objの数をカウント
            nChildCount = objUI.transform.childCount;

            // SkillTurnObjのサイズを設定
            SkillTurnObj = new GameObject[nChildCount - 1];

            for (int i = 1; i < nChildCount; i++)
            {
                // 子objを配列に格納
                Transform childTransform = objUI.transform.GetChild(i);
                SkillTurnObj[i - 1] = childTransform.gameObject;

                // 始めはスキルが溜まっていないため非表示
                SkillTurnObj[i - 1].SetActive(false);
            }

            bCreate = true;
        }

        // スキル発動したらゲージを非表示
        if(SkeletonMovement.skillStep == SkillTurnObj.Length)
        {
            for (int i = 0; i < SkillTurnObj.Length; i++)
            {
                SkillTurnObj[i].SetActive(false);
            }
        }
        else
        {
            // スキルチャージ数によって表示する
            SkillTurnObj[SkeletonMovement.skillStep].SetActive(true);
        }
    }

    //
    void EnemySkillUI()
    {
        // 敵のスキルによって表示するUIを変更する
        switch(SkeletonMovement.skill)
        {
            case SKILL.up:
                {
                    objUI = Instantiate(UpUI[nSkillTurn - 1]);
                    break;
                }
            case SKILL.down:
                {
                    objUI = Instantiate(DownUI[nSkillTurn - 1]);
                    break;
                }
            case SKILL.left:
                {
                    objUI = Instantiate(LeftUI[nSkillTurn - 1]);
                    break;
                }
            case SKILL.right:
                {
                    objUI = Instantiate(RightUI[nSkillTurn - 1]);
                    break;
                }
            case SKILL.invisible:
                {
                    objUI = Instantiate(InvisibleUI[nSkillTurn - 1]);
                    break;
                }
            case SKILL none:
                {
                    break;
                }
        }
    }
      
}
