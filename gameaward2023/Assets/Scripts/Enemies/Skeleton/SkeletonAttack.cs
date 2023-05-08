using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    // �X�P���g���֘A ---------------------------------------------------
    private GameObject SkeletonObj = null;      // �X�P���g���I�u�W�F�N�g
    private Vector3 CurrentPos = Vector3.zero;  // �X�P���g�����W�i�[�p
    // ------------------------------------------------------------------

    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private PlayerMovement PlayerMovement;
    // ------------------------------------------------------------------

    // 2D�}�b�v�����X�N���v�g�p -----------------------------------------
    private GameObject StageMake;               // 
    private ElementGenerator elementGenerator;  // 
    private int[,] map;                         // �}�b�v���i�[�p
    private bool bMapLoading = false;           // �}�b�v�����[�h���ꂽ��
    // ------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();

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

            // �X�|�[�����̍��W���i�[
            CurrentPos = this.gameObject.transform.position;

            bMapLoading = true;
        }


    }
}
