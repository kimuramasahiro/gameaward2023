using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{ 
    private Text damageText;
    //　フェードアウトするスピード
    private float fadeOutSpeed = 1.5f;
    //　移動値
    [SerializeField]
    private float moveSpeed = 0.8f;

    private GameObject AttackCollider;
    private PlayerCollisionAtk PlayerCollisionAtk;

    void Start()
    {
        damageText = GetComponentInChildren<Text>();

        AttackCollider = GameObject.Find("AttackCollider");
        PlayerCollisionAtk = AttackCollider.GetComponent<PlayerCollisionAtk>();
    }

    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        damageText.text = "" + PlayerCollisionAtk.GetAttackDamage();
        damageText.color = Color.Lerp(damageText.color, new Color(1f, 0f, 0f, 0f), fadeOutSpeed * Time.deltaTime);

        if (damageText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
