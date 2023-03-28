using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionAtk : MonoBehaviour
{
    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private PlayerAttack PlayerAttack;

    public float fDamage = 10.0f;
    // ------------------------------------------------------------------

    // private bool isEnemyTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerAttack = PlayerObj.GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(PlayerAttack.IsAttacking)
        {
            // �C���^�[�t�F�C�X���擾
            var hit = collision.gameObject.GetComponent<IReceiveDamege>();

            // �G�ꂽ����̖��O��\��
            hit.ReceiveDamage(fDamage);

            collision.transform.root.GetComponent<TakeDamage>().Damage(collision);

            PlayerAttack.IsAttacking = false;
        }
    }

    public float GetAttackDamage()
    {
        return fDamage; 
    }
}
