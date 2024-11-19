using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackManager : MonoBehaviour
{
    // Manager取得
    private PlayerManager manager;
    private PlayerMoveManager moveManager;

    [Header("Parameter")]
    [SerializeField] private float damageValue;
    [SerializeField] private float attackIntervalTime;
    private float attackIntervalTimer;

    [Header("Hit")]
    [SerializeField] private float adjustDistance;

    [Header("Slash")]
    [SerializeField] private float slashRange;

    [Header("EffectPrefabs")]
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject slashPrefab;

    [Header("UI")]
    [SerializeField] private Image attackGauge;

    [Header("Other Object")]
    [SerializeField] private Transform bossCoreTransform;
    private BossCoreManager bossCoreManager;

    void Start()
    {
        manager = GetComponent<PlayerManager>();
        moveManager = GetComponent<PlayerMoveManager>();

        bossCoreManager = bossCoreTransform.GetComponent<BossCoreManager>();
    }

    public void ManualUpdate()
    {
        Attack();
    }
    void Attack()
    {
        // 攻撃のインターバル計測
        attackIntervalTimer -= Time.deltaTime;
        attackGauge.fillAmount = 1f - attackIntervalTimer / attackIntervalTime;

        // 攻撃を行う
        if (attackIntervalTimer <= 0f && manager.GetInputManager().IsTrgger(manager.GetInputManager().attack))
        {
            // Pillarに攻撃
            foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
            {
                Vector3 pillarRePosition = pillar.transform.position;

                if (IsHitObject(ref pillarRePosition))
                {
                    Vector3 toPlayer = Vector3.Normalize(transform.position - pillarRePosition);
                    Vector3 diffVector = toPlayer * adjustDistance;

                    // HitEffect作成
                    Instantiate(hitPrefab, pillarRePosition + diffVector, Quaternion.identity);

                    // 反動
                    moveManager.Reaction(toPlayer);
                }
            }

            // BossCoreに攻撃
            Vector3 bossCoreRePosition = bossCoreTransform.position;

            if (IsHitObject(ref bossCoreRePosition))
            {
                Vector3 toPlayer = Vector3.Normalize(transform.position - bossCoreRePosition);
                Vector3 diffVector = toPlayer * adjustDistance;

                // HitEffect作成
                Instantiate(hitPrefab, bossCoreRePosition + diffVector, Quaternion.identity);

                // Damageを与える
                bossCoreManager.Damage(damageValue);
            }

            // SlashEffect作成
            Instantiate(slashPrefab, transform.position, Quaternion.identity);

            attackIntervalTimer = attackIntervalTime;
        }
    }
    bool IsHitObject(ref Vector3 _objectPosition)
    {
        // 高さをPlayerに合わせた新座標
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // 距離を取得
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // SlashRange内のPillarを攻撃する
        if (distance < slashRange)
        {
            return true;
        }
        return false;
    }
}
