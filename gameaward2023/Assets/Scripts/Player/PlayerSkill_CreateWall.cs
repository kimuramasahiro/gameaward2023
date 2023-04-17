using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill_CreateWall : MonoBehaviour
{
    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private PlayerMovement PlayerMovement;
    // ------------------------------------------------------------------

    // 2D�}�b�v�����X�N���v�g�p -----------------------------------------
    private GameObject StageMake;               // 
    private ElementGenerator elementGenerator;  // 
    private int[,] map;                         // �}�b�v���i�[�p
    private bool bMapLoading = false;           // �}�b�v�����[�h���ꂽ��
                                                // -------------------------------------------------------------------

    // �X�L���֘A --------------------------------------------------------
    private GameObject ObstacleObj;                             // ��Q��Obj

    private GameObject TargetObj = null;        // �^�b�v�Ώۂ̃I�u�W�F�N�g
    [SerializeField]
    private bool bSkill = false;
    // -------------------------------------------------------------------

    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();

        StageMake = GameObject.Find("StageMake");

        //���\�[�X�ǂݍ���
        ReadResources();
    }

    void ReadResources()
    {
        ObstacleObj = (GameObject)Resources.Load("Prefabs/obstacle");
    }

    void Update()
    {
        // ��x�������s
        if (!bMapLoading)
        {
            // �_���W�����}�b�v�ǂݍ���
            elementGenerator = StageMake.GetComponent<ElementGenerator>();
            map = elementGenerator.GetMapGenerate();

            bMapLoading = true;
        }

        // �v���C���[�������Ă��Ȃ��Ƃ� ���� �v���C���[���S���ȏ�ړ������Ƃ��̂ݔ����\
        if(!PlayerMovement.IsMoving && PlayerMovement.StepCount >= 4)
        { 
            bSkill = true;
        }
        else
        {
            bSkill = false;
        }

        if(bSkill)
        {
            // �N���b�N���o����
            if (Input.GetMouseButtonDown(0))
            {
                // �������Z�b�g
                PlayerMovement.StepCount = 0;

                // �N���b�N�����u���b�N����Q���ɕύX
                GetBlockTapPos();

                bSkill = false;
            }
        }
    }

    private void GetBlockTapPos()
    {
        // �N���b�N���������ɃJ��������Ray���΂�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            // Ray�ɓ�����ʒu�ɑ��݂���I�u�W�F�N�g���擾
            TargetObj = hit.collider.gameObject;
        }

        // �ΏۃI�u�W�F�N�g(�u���b�N)�����݂���ꍇ�̏���
        if (TargetObj != null)
        {
            // �u���b�N�I��������
            ChangeBlock(TargetObj.GetComponent<MapBlock>());
        }
        else
        {
            Debug.Log("�w��̏ꏊ�ɂ͎g���܂���");
        }
    }

    private void ChangeBlock(MapBlock targetBlock)
    {
        // �I�������n�ʂ���Q���ɕύX
        elementGenerator.Originalmap[(int)targetBlock.transform.position.x, (int)targetBlock.transform.position.z] = 2;

        // �n�ʂ��폜
        TargetObj.SetActive(false);

        // ��Q���𐶐�
        GameObject obj;
        obj = Instantiate(ObstacleObj);
        obj.transform.position = new Vector3(targetBlock.transform.position.x, 2.0f, targetBlock.transform.position.z);
    }

    public int[,] GetUpdateMap()
    {
        return map;
    }
}
