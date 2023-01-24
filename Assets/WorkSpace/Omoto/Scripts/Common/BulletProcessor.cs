using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProcessor : MonoBehaviour
{
    [SerializeField] private float speed;       // 速度
    [SerializeField] private float damage;      // ダメージ

    // ダメージプロパティ
    public float Damage { get { return damage; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 自身の向きベクトル取得
        float angleDir = transform.eulerAngles.z * (Mathf.PI / 180.0f);
        Vector3 dir = new Vector3(Mathf.Cos(angleDir), Mathf.Sin(angleDir), 0.0f);

        // 自身の向きに移動
        transform.position += dir * speed * Time.deltaTime;

        //TODO: 自身を削除
    }
}
