using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCtrlCommon : MonoBehaviour
{
    //�}�b�v�T���p
    ElementGenerator elementGenerator;                      //�I�u�W�F�N�g�����p
    [SerializeField] GameObject objMapTip;                  //�}�b�v�`�b�v���\�[�X
    GameObject objMapTipFinal;                              //���������}�b�v�`�b�v

    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private PlayerMovement PlayerMovement;

    private Vector2 PlayerCurrentPos = Vector2.zero;
    // ------------------------------------------------------------------

    void Awake()
    {
        //�}�b�v�T���p
        elementGenerator = GameObject.Find("StageMake").GetComponent<ElementGenerator>();

        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
    }

    void Start()
    {
        ////�}�b�v�`�b�v����
        //objMapTipFinal = elementGenerator.GenerateObjMap2D(gameObject, objMapTip);

        ////�v���C���[�ȊO�͏����͔�\��
        //if (CompareTag("Player") == false)
        //{
        //    Color color = objMapTipFinal.GetComponent<Image>().color;
        //    color.a = 0;
        //    objMapTipFinal.GetComponent<Image>().color = color;
        //}
    }

    void Update()
    {
        ////�}�b�v�`�b�v�X�V
        //elementGenerator.UpdateMap2D(gameObject, objMapTipFinal);

        ////�v���C���[�̓}�b�v�T������
        //if (CompareTag("Player") == true)
        //{
        //    // �v���C���[���W�i�[
        //    PlayerCurrentPos.x = PlayerObj.transform.position.x;
        //    PlayerCurrentPos.y = PlayerObj.transform.position.z;

        //    //elementGenerator.SearchMap((int)(PlayerCurrentPos.x + 0.5f), (int)(PlayerCurrentPos.y + 0.5f));
        //}
    }
}