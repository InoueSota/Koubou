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
        // �N�[���^�C���������Ă���Ȃ�_���[�W���󂯂�
        if (damageCoolTimer <= 0f)
        {
            // �_���[�W��H�炤
            hp--;
            hp = Mathf.Clamp(hp, 0, hpMax);
            // Hp��UI���\���ɂ���
            hpObjs[hp].SetActive(false);

            // HP�N�[���^�C���ݒ�
            damageCoolTimer = damageCoolTime;
        }
    }
}
