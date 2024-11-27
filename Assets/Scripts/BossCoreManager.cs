using UnityEngine;
using UnityEngine.UI;

public class BossCoreManager : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private float bossHpMax;
    private float bossHp;

    [Header("Gauge")]
    [SerializeField] private Slider hpGauge;

    // ダメージを受けられる状態かフラグ
    private bool canHit;

    void Start()
    {
        bossHp = bossHpMax;
    }

    void Update()
    {
        hpGauge.value = bossHp / bossHpMax;
    }

    // Setter
    public void Damage(float _damageValue)
    {
        bossHp -= _damageValue;
        bossHp = Mathf.Clamp(bossHp, 0f, bossHpMax);
    }
    public void SetCanHit(bool _canHit)
    {
        canHit = _canHit;
    }

    // Getter
    public float GetBossHp()
    {
        return bossHp;
    }
    public bool GetCanHit()
    {
        return canHit;
    }
}
