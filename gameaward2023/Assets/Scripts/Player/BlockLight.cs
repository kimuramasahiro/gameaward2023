using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLight : MonoBehaviour
{
    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private Vector3 CurrentPos = Vector3.zero;  // �v���C���[���W�i�[�p
    // ------------------------------------------------------------------

    // 2D�}�b�v�����X�N���v�g�p -----------------------------------------
    private GameObject StageMake;               // 
    private ElementGenerator elementGenerator;  // 
    private int[,] map;                         // �}�b�v���i�[�p
    private bool bMapLoading = false;           // �}�b�v�����[�h���ꂽ��
    // ------------------------------------------------------------------

    // ���点��I�u�W�F�N�g ---------------------------------------------
    private List<GameObject> ParentObj = new List<GameObject>();
    private List<GameObject> ChildObj = new List<GameObject>();
    private List<GameObject> LightObj = new List<GameObject>();
    // ------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        StageMake = GameObject.Find("StageMake");

        // �_���W�����}�b�v�ǂݍ���
        elementGenerator = StageMake.GetComponent<ElementGenerator>();
        map = elementGenerator.GetMapGenerate();

        ParentObj = elementGenerator.GrassList;

        for (int i = 0; i < ParentObj.Count; i++)
        {
            ChildObj.Add(ParentObj[i].transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < ParentObj.Count; i++)
        {
            LightObj.Add(ParentObj[i].transform.GetChild(1).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ��x�������s
        if (!bMapLoading)
        {
            //// �_���W�����}�b�v�ǂݍ���
            //elementGenerator = StageMake.GetComponent<ElementGenerator>();
            //map = elementGenerator.GetMapGenerate();

            //ParentObj = elementGenerator.GrassList;

            //for(int i = 0; i < ParentObj.Count; i++)
            //{
            //    ChildObj.Add(ParentObj[i].transform.GetChild(0).gameObject);
            //}

            //for (int i = 0; i < ParentObj.Count; i++)
            //{
            //   LightObj.Add(ParentObj[i].transform.GetChild(1).gameObject);
            //}

            bMapLoading = true;
        }

        // �v���C���[���W�ǂݍ���
        CurrentPos.x = PlayerObj.transform.position.x;
        CurrentPos.z = PlayerObj.transform.position.z;

        // �v���C���[�̕������Ƃ��\�ȃu���b�N��\������
        PlayerWalkMark();
    }

    private void PlayerWalkMark()
    {
        for (int i = 0; i < ParentObj.Count; i++)
        {
            if ((int)ParentObj[i].transform.position.x == CurrentPos.x && (int)ParentObj[i].transform.position.z == CurrentPos.z + 1)
            {
                ChildObj[i].SetActive(true);
                //LightObj[i].SetActive(true);
            }
            else if ((int)ParentObj[i].transform.position.x == CurrentPos.x && (int)ParentObj[i].transform.position.z == CurrentPos.z - 1)
            {
                ChildObj[i].SetActive(true);
                //LightObj[i].SetActive(true);
            }
            else if ((int)ParentObj[i].transform.position.x == CurrentPos.x - 1 && (int)ParentObj[i].transform.position.z == CurrentPos.z)
            {
                ChildObj[i].SetActive(true);
                //LightObj[i].SetActive(true);
            }
            else if ((int)ParentObj[i].transform.position.x == CurrentPos.x + 1 && (int)ParentObj[i].transform.position.z == CurrentPos.z)
            {
                ChildObj[i].SetActive(true);
                //LightObj[i].SetActive(true);
            }
            else
            {
                ChildObj[i].SetActive(false);
            }
        }
    }
}
