using UnityEngine;
using System.Collections;

public class TakeDamage : MonoBehaviour
{
    //　DamageUIプレハブ
    private GameObject damageUI = null;

    private void Start()
    {
        damageUI = (GameObject)Resources.Load("Prefabs/UI/DamageUI");
    }

    public void Damage(Collision col)
    {
        //　DamageUIをインスタンス化。登場位置は接触したコライダの中心からカメラの方向に少し寄せた位置
        var obj = Instantiate<GameObject>(damageUI, col.collider.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity);
    }
}
