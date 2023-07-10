using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private Controller _gameInputs;    //gamepad
    private bool bSwitch = true;
    private CanvasGroup canvasGroup;

    private bool bTouch = false;

    void Start()
    {
        SkeletonObj = this.gameObject;
        SkeletonTrasform = SkeletonObj.transform.GetChild(1).gameObject.transform;
        SkeletonMovement = SkeletonObj.GetComponent<SkeletonMovement>();

        // �X�L���^�[��
        nSkillTurn = SkeletonMovement.skillTurn;

        //���\�[�X�ǂݍ���
        ReadResources();

        // �R���g���[���[����
        //Input Action�C���X�^���X����
        _gameInputs = new Controller();

        //Action�C�x���g�o�^
        _gameInputs.UI.OnOff.started += UI_OnOff;
        //_gameInputs.UI.OnOff.performed += UI_OnOff;

        _gameInputs.Enable();
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

            // objUI��CanvasGroup�R���|�[�l���g���擾����
            canvasGroup = objUI.GetComponent<CanvasGroup>();

            bCreate = true;
        }

        // UI����ɐ��ʂ������Ă���悤�ɂ���
        objUI.transform.eulerAngles = new Vector3(50.0f, 0.0f, 0.0f);

        // �X�L������������Q�[�W���\��
        if (SkeletonMovement.skillStep == SkillTurnObj.Length)
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

        // UI�̕\��.��\���̐؂�ւ�
        if(bSwitch)
        {
            canvasGroup.alpha = 1.0f;
        }
        else
        {
            canvasGroup.alpha = 0.0f;
        }
    }

    // ����̃{�^������������UI�̕\��������
    private void UI_OnOff(InputAction.CallbackContext context)
    {
        // bool�̐؂�ւ�
        bSwitch = !bSwitch;
    }

    // �G�̃X�L���ɂ���ĕ\������UI��ύX����
    private void EnemySkillUI()
    {
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
