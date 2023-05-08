using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement :EnemyBase
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

        //CheckPlayerWithToNear();

        // �i�߂��Ԃ�
        if (IsMovable)
        {
            MoveSkeleton();

            // �v���C���[���i�񂾂�
            if (PlayerMovement.IsMoving)
            {
                
                //// �v���C���[���߂��ɂ���ꍇ
                //if (bPlayerNear)
                //{
                //    // �v���C���[�ɋ߂Â�����
                //    LockOnMove();
                //}
                //// �v���C���[���߂��ɂ��Ȃ��ꍇ
                //else
                //{
                //    // �����_���ɐi�ޏ���
                //    MoveRandomDir();
                //}

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

    private void MoveRandomDir()
    {
        // �i�ޕ��������߂�
        MoveDir = UnityEngine.Random.Range(1, 5);   // 1�`4�̒l�������_���Œ��o����

        // �i�ސ�ɕǂ�����Ύ~�܂�A�Ȃ���΂P�}�X�i��
        switch (MoveDir)
        {
            // �O�ɐi�ޏꍇ
            case 1:
                // �ǂ����邩
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

            // ���ɐi�ޏꍇ
            case 2:
                // �ǂ����邩
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

            // ���ɐi�ޏꍇ
            case 3:
                // �ǂ����邩
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

            // �E�ɐi�ޏꍇ
            case 4:
                // �ǂ����邩
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
        // �X�P���g�����W�i�[
        CheckPos.x = CurrentPos.x;
        CheckPos.y = CurrentPos.z;

        // �v���C���[���W�i�[
        PlayerCurrentPos.x = PlayerObj.transform.position.x;
        PlayerCurrentPos.y = PlayerObj.transform.position.z;

        // �v���C���[���߂��ɂ��Ȃ����m�F����
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
        // ����
        PosDiff.x = PlayerCurrentPos.x - CheckPos.x;
        PosDiff.y = PlayerCurrentPos.y - CheckPos.y;


        // �E
        if (PosDiff.y > 0)
        {
            // �ǂ����邩
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
        // ��
        else if (PosDiff.y < 0)
        {
            if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 0)
            {
                return;
            }
            // �ǂ����邩
            else if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 1 ||
                    map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2)
            {
                CurrentPos.z -= 1.0f;
            }
        }

        // z���W�̍�����0���傫��������(�v���C���[���E�ɂ���ꍇ)
        // �E
        //if (PosDiff.x > 0)
        //{
        //    // �ǂ����邩
        //    if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 1 ||
        //        map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 2)
        //    {
        //        CurrentPos.x += 1.0f;
        //        SkeletonDir = false;
        //    }
        //}
        //// ��
        //else if(PosDiff.x < 0)
        //{
        //    // �ǂ����邩
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
        // �v���C���[���O�ɐi�񂾂�
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
        // �v���C���[�����ɐi�񂾂�
        else if (PlayerMovement.PressKey_S)
        {
            // �ǂ����邩
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
        // �v���C���[�����ɐi�񂾂�
        else if (PlayerMovement.PressKey_A)
        {
            // �ǂ����邩
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
        // �v���C���[���E�ɐi�񂾂�
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
