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
    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private PlayerMovement PlayerMovement;
    // ------------------------------------------------------------------
    public int skillStep;
    public bool chargedSkill = false;
    virtual protected void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
        skillStep = PlayerMovement.StepCount;
        // �C�x���g�̓o�^�i�v���C���[���ړ�������֐����Ăԁj
        PlayerMovement.Walk += OnChargeSkill;
    }
    
    // �������̐ݒ�p�֐�
    public void SetEnemy(Enemy data)
    {
        health = data.GetHealth();
        skillTurn = data.GetSkillTurn();
        skill = data.GetSkill();
        startPos = data.GetPos();
        name = data.GetName();
        tag = data.GetTag();
    }

    // �X�L���`���[�W�֐�
    private void OnChargeSkill()
    {
        skillStep++;
        skillStep %= skillTurn+1;

        chargedSkill = (skillStep == skillTurn) ? true : false;
        Debug.Log(name + " : " + tag + "<"+skillStep+"/"+skillTurn+">");
    }
}
