using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHealth : MonoBehaviour, IReceiveDamege
{
    public float Health = 50.0f;

    public bool IsReceiveDameged = false;
    private new BoxCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // HP��0�ȉ��̏ꍇ
        if(Health <= 0.0f)
        {
            collider.isTrigger = true;      // �����蔻����Ȃ���
            Health = 0.0f;                  // HP��0�ŌŒ肷��
            Destroy(gameObject, 2.0f);�@�@�@// �|�ꂽ��ɃV�[��������ł�����
        }
    }

    public void ReceiveDamage(float damage)
    {
        Health -= damage;

        IsReceiveDameged = true;
    }
}
