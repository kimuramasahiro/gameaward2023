using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_CreateWall : MonoBehaviour
{
    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private PlayerMovement PlayerMovement;
    // ------------------------------------------------------------------

    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // �v���C���[�������Ă��Ȃ��Ƃ�
        if(!PlayerMovement.IsMoving)
        {

        }
    }
}
