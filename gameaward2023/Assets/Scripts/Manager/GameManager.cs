using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �^�b�v�Ώۂ̃I�u�W�F�N�g
    private GameObject TargetObj = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �^�b�v���o����
        if(Input.GetMouseButtonDown(0))
        {
            // �^�b�v�����u���b�N�̍��W��\��
            GetBlockTapPos();
        }
    }

    private void GetBlockTapPos()
    {
        // �^�b�v���������ɃJ��������Ray���΂�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(ray,out hit))
        {
            // Ray�ɓ�����ʒu�ɑ��݂���I�u�W�F�N�g���擾
            TargetObj = hit.collider.gameObject;
        }

        // �ΏۃI�u�W�F�N�g(�}�b�v�u���b�N)�����݂���ꍇ�̏���
        if(TargetObj != null)
        {
            // �u���b�N�I��������
            SelectBlock(TargetObj.GetComponent<MapBlock>());
        }
    }

    private void SelectBlock(MapBlock targetBlock)
    {
        Debug.Log("�u���b�N�̍��W : " + targetBlock.transform.position);
    }
}
