using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerAttack : MonoBehaviour
{
    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private Vector3 CurrentPos = Vector3.zero;  // �v���C���[���W�i�[�p

    public bool IsAttacking = false;            // �v���C���[���U��������
    // ------------------------------------------------------------------

    // 2D�}�b�v�����X�N���v�g�p -----------------------------------------
    private GameObject StageMake;               // �R���|�[�l���g����Ă���I�u�W�F�N�g
    private ElementGenerator elementGenerator;  // ElementGenerator�N���X�p���p
    private int[,] map;                         // �}�b�v���i�[�p
    private bool bMapLoading = false;           // �}�b�v�����[�h���ꂽ��
    // ------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        StageMake = GameObject.Find("StageMake");
    }

    // Update is called once per frame
    void Update()
    {
        // ��x�������s
        if (!bMapLoading)
        {
            // �_���W�����}�b�v�ǂݍ���
            elementGenerator = StageMake.GetComponent<ElementGenerator>();
            map = elementGenerator.GetMapGenerate();

            // �v���C���[���W�ǂݍ���
            CurrentPos.x = elementGenerator.GetMapPlayer().x;
            CurrentPos.y = 1.5f;
            CurrentPos.z = elementGenerator.GetMapPlayer().y;

            bMapLoading = true;
        }
    }
}
