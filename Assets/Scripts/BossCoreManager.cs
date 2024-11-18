using UnityEngine;
using UnityEngine.UI;

public class BossCoreManager : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private float bossHpMax;
    private float bossHp;

    [Header("Gauge")]
    [SerializeField] private Slider hpGauge;

    void Start()
    {
        bossHp = bossHpMax;
    }

    void Update()
    {
        hpGauge.value = bossHp / bossHpMax;
    }

    public void Damage(float _damageValue)
    {
        bossHp -= _damageValue;
        bossHp = Mathf.Clamp(bossHp, 0f, bossHpMax);
    }
}
