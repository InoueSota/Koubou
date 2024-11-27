using UnityEngine;

public class PlayerHpManager : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private int hpMax;
    private int hp;
    [SerializeField] private float damageCoolTime;
    private float damageCoolTimer;

    [Header("UI")]
    [SerializeField] private GameObject[] hpObjs;

    void Start()
    {
        hp = hpMax;

        damageCoolTimer = 0f;
    }

    void Update()
    {
        damageCoolTimer -= Time.deltaTime;
    }

    public void Damage()
    {
        // クールタイムが消費されているならダメージを受ける
        if (damageCoolTimer <= 0f)
        {
            // ダメージを食らう
            hp--;
            hp = Mathf.Clamp(hp, 0, hpMax);
            // HpのUIを非表示にする
            hpObjs[hp].SetActive(false);

            // HPクールタイム設定
            damageCoolTimer = damageCoolTime;
        }
    }
}
