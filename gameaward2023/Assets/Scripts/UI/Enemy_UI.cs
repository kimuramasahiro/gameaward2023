using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_UI : MonoBehaviour
{
    private Transform SkeletonTrasform;
    private GameObject SkeletonObj;
    private SkeletonMovement SkeletonMovement;

    private GameObject[] UpUI = new GameObject[1];       // ��ړ�UI
    private GameObject[] DownUI = new GameObject[1];     // ���ړ�UI
    private GameObject[] LeftUI = new GameObject[1];     // ���ړ�UI
    private GameObject[] RightUI = new GameObject[1];    // �E�ړ�UI
    private GameObject[] InvisibleUI = new GameObject[1];    // ����UI���X�g
    private GameObject objUI;      // ��������Obj
    private int nSkillTurn = 0;    // �X�L���^�[��
    private bool bCreate = false;  // �������ꂽ��
    private int nChildCount = 0;   // �qObj�̐�
    private GameObject[] SkillTurnObj;  // 
    private SKILL skill;

    void Start()
    {
        SkeletonObj = this.gameObject;
        SkeletonTrasform = SkeletonObj.transform.GetChild(1).gameObject.transform;
        SkeletonMovement = SkeletonObj.GetComponent<SkeletonMovement>();

        // �X�L���^�[��
        nSkillTurn = SkeletonMovement.skillTurn;

        //���\�[�X�ǂݍ���
        ReadResources();
    }

    void ReadResources()
    {
        UpUI = Resources.LoadAll<GameObject>("Prefabs/UI_UpGage");          // ��ړ�UI���X�g��ǂݍ���
        DownUI = Resources.LoadAll<GameObject>("Prefabs/UI_DownGage");      // ���ړ�UI���X�g��ǂݍ���
        LeftUI = Resources.LoadAll<GameObject>("Prefabs/UI_LeftGage");      // ���ړ�UI���X�g��ǂݍ���
        RightUI = Resources.LoadAll<GameObject>("Prefabs/UI_RightGage");    // �E�ړ�UI���X�g��ǂݍ���
        InvisibleUI = Resources.LoadAll<GameObject>("Prefabs/UI_TomeiGage");    // ����UI���X�g��ǂݍ���
    }

    void Update()
    {
        if (!bCreate)
        {
            // �G�̃X�L���ɂ���ĕ\������UI��ύX����
            EnemySkillUI();

            // �����ݒ�
            objUI.transform.SetParent(SkeletonTrasform, false);
            objUI.transform.localPosition = Vector3.zero;
            //objUI.transform.localRotation = new Quaternion(20.0f, 0.0f, 0.0f, 0.0f);
            //objUI.transform.localScale = new Vector3(0.001f, 0.001f, 1.0f);

            // �qObj�̐����J�E���g
            nChildCount = objUI.transform.childCount;

            // SkillTurnObj�̃T�C�Y��ݒ�
            SkillTurnObj = new GameObject[nChildCount - 1];

            for (int i = 1; i < nChildCount; i++)
            {
                // �qobj��z��Ɋi�[
                Transform childTransform = objUI.transform.GetChild(i);
                SkillTurnObj[i - 1] = childTransform.gameObject;

                // �n�߂̓X�L�������܂��Ă��Ȃ����ߔ�\��
                SkillTurnObj[i - 1].SetActive(false);
            }

            bCreate = true;
        }

        // �X�L������������Q�[�W���\��
        if(SkeletonMovement.skillStep == SkillTurnObj.Length)
        {
            for (int i = 0; i < SkillTurnObj.Length; i++)
            {
                SkillTurnObj[i].SetActive(false);
            }
        }
        else
        {
            // �X�L���`���[�W���ɂ���ĕ\������
            SkillTurnObj[SkeletonMovement.skillStep].SetActive(true);
        }
    }

    //
    void EnemySkillUI()
    {
        // �G�̃X�L���ɂ���ĕ\������UI��ύX����
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
