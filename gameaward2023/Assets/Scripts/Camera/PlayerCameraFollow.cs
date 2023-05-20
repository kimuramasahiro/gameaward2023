using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    private GameObject target;
    [SerializeField]
    private Vector3 offset = new Vector3(0.0f, 3.0f, -2.5f);
    [SerializeField]
    private Vector3 offset3rd = new Vector3(0.0f, 8.0f, 0.0f);
    private Quaternion Angle = Quaternion.Euler(50.0f, 0.0f, 0.0f);
    private int magnificationIdx = 0;
    [SerializeField]
    private int magnificationIdxMax = 3;
    [SerializeField]
    private float magnification =1.0f;

    void Start()
    {
        gameObject.transform.rotation = Angle;
        target = GameObject.Find("Player");
    }

    /// <summary>
    /// �v���C���[���ړ�������ɃJ�������ړ�����悤�ɂ��邽�߂�LateUpdate�ɂ���B
    /// </summary>
    void LateUpdate()
    {
        // �J�����̔{������
        if(Input.GetKeyDown(KeyCode.C))
        {
            gameObject.transform.rotation = Angle;
            magnificationIdx = (magnificationIdx + 1) % (magnificationIdxMax+1);
            magnification = 1 + magnificationIdx * 0.5f;
            // �ォ�王�_
            if(magnificationIdx == magnificationIdxMax)
            {
                magnification = 1 + magnificationIdxMax * 0.5f;
                gameObject.transform.rotation = Quaternion.Euler(90.0f,0.0f,0.0f);
            }
        }

        // �J�����̈ʒu���^�[�Q�b�g�̈ʒu�ɃI�t�Z�b�g�𑫂����ꏊ�ɂ���B
        if(magnificationIdx == magnificationIdxMax)
        {
            gameObject.transform.position = target.transform.position + offset3rd;
        }
        else
        {
            gameObject.transform.position = target.transform.position + (offset * magnification);
        }
    }
}
