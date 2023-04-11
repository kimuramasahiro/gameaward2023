using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int health;
    public int skillTurn;
    public SKILL skill;
    public Vector2 startPos;
    public string tag;
    public NAME name;
    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private PlayerMovement PlayerMovement;
    // ------------------------------------------------------------------
    public int skillStep;
    public bool chargedSkill = false;
    virtual protected void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
        skillStep = PlayerMovement.StepCount;
        // イベントの登録（プレイヤーが移動したら関数を呼ぶ）
        PlayerMovement.Walk += OnChargeSkill;
    }
    
    // 生成時の設定用関数
    public void SetEnemy(Enemy data)
    {
        health = data.GetHealth();
        skillTurn = data.GetSkillTurn();
        skill = data.GetSkill();
        startPos = data.GetPos();
        name = data.GetName();
        tag = data.GetTag();
    }

    // スキルチャージ関数
    private void OnChargeSkill()
    {
        skillStep++;
        skillStep %= skillTurn+1;

        chargedSkill = (skillStep == skillTurn) ? true : false;
        Debug.Log(name + " : " + tag + "<"+skillStep+"/"+skillTurn+">");
    }
}
