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

    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();

        StageMake = GameObject.Find("StageMake");
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

        // �v���C���[�������Ă��Ȃ��Ƃ�
        if (!PlayerMovement.IsMoving)
        {

        }
    }
}
