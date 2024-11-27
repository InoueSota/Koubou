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
    [SerializeField] private Animator vignetteAnim;

    [Header("Sounds")]
    [SerializeField] private AudioClip getDamageClip;
    private AudioSource audioSource;

    void Start()
    {
        hp = hpMax;

        damageCoolTimer = 0f;

        audioSource = GetComponent<AudioSource>();
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

            // 音を鳴らす
            audioSource.PlayOneShot(getDamageClip);

            // Vignetteのアニメーションを動かす
            vignetteAnim.SetTrigger("Start");
        }
    }
}
