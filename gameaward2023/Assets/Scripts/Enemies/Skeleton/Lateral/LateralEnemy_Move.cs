using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralEnemy_Move : EnemyBase
{
    // �X�P���g���֘A ---------------------------------------------------
    private GameObject SkeletonObj = null;      // �X�P���g���I�u�W�F�N�g
    //private Vector3 CurrentPos = Vector3.zero;  // �X�P���g�����W�i�[�p
    private Vector2 CheckPos = Vector2.zero;    // 
    public float MoveSpeed = 3.0f;              // �X�P���g���̓�������
    private int MoveDir = 0;                    // �X�P���g���̐i�ޕ���
    private bool SkeletonDir = false;           // �X�P���g���̌���(false:�E true:��)
    public bool IsMovable = true;               // �X�P���g�����i�߂邩(true:�i�߂� false:�i�߂Ȃ�)

    private int SearchRange = 3;                // ���G�͈�(3�̏ꍇ,�������S����c��3�}�X)
    [SerializeField]
    private bool bPlayerNear = false;
    [SerializeField]
    private Vector2 PosDiff = Vector2.zero;     // �X�P���g���ƃv���C���[�̍��W�̍�

    private bool bTouch = false;
    private bool isTouched_A_Wall = false;
    private bool isTouched_D_Wall = false;
    // ------------------------------------------------------------------

    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private PlayerMovement PlayerMovement;

    private Vector2 PlayerCurrentPos = Vector2.zero;
    // ------------------------------------------------------------------

    // // 2D�}�b�v�����X�N���v�g�p -----------------------------------------
    // private GameObject StageMake;               // 
    // private ElementGenerator elementGenerator;  // 
    // private int[,] map;                         // �}�b�v���i�[�p
    // private bool bMapLoading = false;           // �}�b�v�����[�h���ꂽ��
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
        //// ��x�������s
        //if (!bMapLoading)
        //{
        //    // �_���W�����}�b�v�ǂݍ���
        //    elementGenerator = StageMake.GetComponent<ElementGenerator>();
        //    map = elementGenerator.GetMapGenerate();

        //    // �X�|�[�����̍��W���i�[
        //    CurrentPos = this.gameObject.transform.position;

        //    bMapLoading = true;
        //}

        // �i��ł��Ȃ��Ƃ�true�ɂ���
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

        // �i�߂��Ԃ�
        if (IsMovable)
        {
            MoveEnemy();
            

            // �v���C���[���i�񂾂�
            if (PlayerMovement.IsMoving)
            {
                // �i�񂾂�false
                IsMovable = false;
            }
        }

        // �i�ޕ����Ƀv���C���[����������
        if (!SkeletonDir)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        // ���W�X�V
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


        // �v���C���[���ړ�������
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
