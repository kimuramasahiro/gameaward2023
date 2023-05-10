using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int health;
    public int skillTurn;
    public SKILL skill;
    public Vector2 startPos;
    protected Vector3 CurrentPos = Vector3.zero;  // �X�P���g�����W�i�[�p
    public string tag;
    public NAME name;
    public int skillStep;
    public bool chargedSkill = false;
    protected bool inSkill = false; // �X�L��������
    public float moveSpeed;
    protected bool onMove = false;// �ʏ�ړ���
    protected int SkeletonDir = 0;
    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private PlayerMovement PlayerMovement;
    // ------------------------------------------------------------------
    // 2D�}�b�v�����X�N���v�g�p -----------------------------------------
    private GameObject StageMake;               // 
    private ElementGenerator elementGenerator;  // 
    protected int[,] map;                         // �}�b�v���i�[�p
    private bool bMapLoading = false;           // �}�b�v�����[�h���ꂽ��
    // -------------------------------------------------------------------
    virtual protected void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
        skillStep = PlayerMovement.StepCount;
        // �C�x���g�̓o�^�i�v���C���[���ړ�������֐����Ăԁj
        PlayerMovement.Walk += OnChargeSkill;

        StageMake = GameObject.Find("StageMake");
        // �_���W�����}�b�v�ǂݍ���
        elementGenerator = StageMake.GetComponent<ElementGenerator>();
        map = elementGenerator.GetMapGenerate();

        // �X�|�[�����̍��W���i�[
        CurrentPos = this.gameObject.transform.position;

        bMapLoading = true;
        
    }
    //virtual protected void Update()
    //{
    //    // ��x�������s
    //    if (!bMapLoading)
    //    {
    //        // �_���W�����}�b�v�ǂݍ���
    //        elementGenerator = StageMake.GetComponent<ElementGenerator>();
    //        map = elementGenerator.GetMapGenerate();

    //        // �X�|�[�����̍��W���i�[
    //        CurrentPos = this.gameObject.transform.position;

    //        bMapLoading = true;
    //    }
    //}
    // �������̐ݒ�p�֐�
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

    // �X�L���`���[�W�֐�
    private void OnChargeSkill()
    {
        skillStep++;
        skillStep %= skillTurn+1;

        chargedSkill = (skillStep == skillTurn) ? true : false;
        Debug.Log(name + " : " + tag + "<"+skillStep+"/"+skillTurn+">");
    }

    // --- �X�L���@---
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

    // ����1�}�X�i��
    protected void WalkLeftSkill()
    {
        // �ǂ����邩
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
        // ��������
        //this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
        // ���W�X�V
        //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, CurrentPos, moveSpeed * Time.deltaTime);
    }

    // �E��1�}�X�i��
    protected void WalkRightSkill()
    {
        // �ǂ����邩
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
        // ��������
        //this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        // ���W�X�V
        //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, CurrentPos, moveSpeed * Time.deltaTime);
    }

    // ���1�}�X�i��
    protected void WalkUpSkill()
    {
        // �ǂ����邩
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
        // ��������
        //this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        // ���W�X�V
        //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, CurrentPos, moveSpeed * Time.deltaTime);
    }

    // ����1�}�X�i��
    protected void WalkDownSkill()
    {
        // �ǂ����邩
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
        // ��������
        //this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        // ���W�X�V
        //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, CurrentPos, moveSpeed * Time.deltaTime);
    }

    //  ������
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
