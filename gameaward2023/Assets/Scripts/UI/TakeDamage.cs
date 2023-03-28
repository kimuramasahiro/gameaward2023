using UnityEngine;
using System.Collections;

public class TakeDamage : MonoBehaviour
{
    //�@DamageUI�v���n�u
    private GameObject damageUI = null;

    private void Start()
    {
        damageUI = (GameObject)Resources.Load("Prefabs/UI/DamageUI");
    }

    public void Damage(Collision col)
    {
        //�@DamageUI���C���X�^���X���B�o��ʒu�͐ڐG�����R���C�_�̒��S����J�����̕����ɏ����񂹂��ʒu
        var obj = Instantiate<GameObject>(damageUI, col.collider.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity);
    }
}
