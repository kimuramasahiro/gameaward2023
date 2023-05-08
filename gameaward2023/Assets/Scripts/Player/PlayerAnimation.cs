using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private PlayerMovement PlayerMovement;
    private PlayerAttack PlayerAttack;
    // ------------------------------------------------------------------

    // �A�j���[�V�����Ǘ� -----------------------------------------------
    private Animator m_animator;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;

    private bool m_noBlood = false;            // ���̕\��(true:���� false:�Ȃ�) 
    // ------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
        PlayerAttack = PlayerObj.GetComponent<PlayerAttack>();

        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // �A�j���[�V�����Đ�����
        m_timeSinceAttack += Time.deltaTime;

        // ��ɒn�ʂɂ��Ă�����
        m_animator.SetBool("Grounded", true);

        // ���S�A�j���[�V����
        if (Input.GetKeyDown("e"))
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }

        // �U���A�j���[�V����
        else if (Input.GetKeyDown("f") && m_timeSinceAttack > 0.5f)
        {
            PlayerAttack.IsAttacking = true;

            m_currentAttack = 1;

            // �A�j���[�V�����Đ����Ԃ�1�b������ƃR���{�I��
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // �Đ�����A�j���[�V������Trigger��ݒ�
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // �^�C�}�[���Z�b�g
            m_timeSinceAttack = 0.0f;
        }

        else if (PlayerMovement.IsMoving)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        // �A�j���[�V�������Đ����Ă��Ȃ��Ƃ���Idle��Ԃɂ���
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

        // �U�����ē������Ă��Ȃ�������false
        if (m_timeSinceAttack > 0.1f)
        {
            PlayerAttack.IsAttacking = false;
        }


        //else if (Input.GetKeyDown("f") && m_timeSinceAttack > 0.25f)
        //{
        //    // �U���A�j���[�V�������X�V����
        //    m_currentAttack++;

        //    // �U���A�j���[�V���������������n�߂ɖ߂�
        //    if (m_currentAttack > 3)
        //        m_currentAttack = 1;

        //    // Reset Attack combo if time since last attack is too large
        //    if (m_timeSinceAttack > 1.0f)
        //        m_currentAttack = 1;

        //    // �Đ�����A�j���[�V������Trigger��ݒ�
        //    m_animator.SetTrigger("Attack" + m_currentAttack);

        //    // �^�C�}�[���Z�b�g
        //    m_timeSinceAttack = 0.0f;
        //}
    }
}
