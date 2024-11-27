using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackManager : MonoBehaviour
{
    // Manager取得
    private PlayerManager manager;
    private PlayerMoveManager moveManager;
    private PlayerPowerUpManager powerUpManager;

    [Header("Slash")]
    [SerializeField] private GameObject smallSlashPrefab;
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private float attackIntervalTime;
    private float attackIntervalTimer;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireIntervalTime;
    private float fireIntervalTimer;

    [Header("Hit")]
    [SerializeField] private float adjustDistance;

    [Header("UI")]
    [SerializeField] private Image attackGauge;
    [SerializeField] private Image fireGauge;

    [Header("Other Object")]
    [SerializeField] private Transform bossCoreTransform;

    void Start()
    {
        manager = GetComponent<PlayerManager>();
        moveManager = GetComponent<PlayerMoveManager>();
        powerUpManager = GetComponent<PlayerPowerUpManager>();
    }

    public void ManualUpdate(bool _isPowerUpFrame)
    {
        Slash(_isPowerUpFrame);
        Fire();
    }
    void Slash(bool _isPowerUpFrame)
    {
        // 攻撃のインターバル計測
        attackIntervalTimer -= Time.deltaTime;
        attackGauge.fillAmount = 1f - attackIntervalTimer / attackIntervalTime;

        // 攻撃を行う
        if (!_isPowerUpFrame && attackIntervalTimer <= 0f && manager.GetInputManager().IsTrgger(manager.GetInputManager().attack))
        {
            GameObject slash = null;

            // 強化状態かどうかによって生成するslashの種類を変える
            if (powerUpManager.GetIsPowerUp())
            {
                // Slashの生成
                slash = Instantiate(slashPrefab, transform.position, Quaternion.identity);
            } else {
                // SmallSlashの生成
                slash = Instantiate(smallSlashPrefab, transform.position, Quaternion.identity);
            }

            // 変数を与える
            slash.GetComponent<PlayerSlashManager>().Initialize(moveManager, bossCoreTransform, adjustDistance, powerUpManager.GetIsPowerUp());

            // インターバルの再設定
            attackIntervalTimer = attackIntervalTime;
        }
    }
    void Fire()
    {
        // 射撃のインターバル計測
        fireIntervalTimer -= Time.deltaTime;
        fireGauge.fillAmount = 1f - fireIntervalTimer / fireIntervalTime;

        // 射撃を行う
        if (fireIntervalTimer <= 0f && (manager.GetInputManager().IsPush(manager.GetInputManager().rTrigger) || manager.GetInputManager().IsPush(manager.GetInputManager().yButton)))
        {
            // Bulletの生成
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // 移動方向（初期値は進行方向）
            Vector3 moveVector = moveManager.GetSaveVector();

            // 狙いを定めていたら移動方向をボスに向ける
            if (manager.GetInputManager().IsPush(manager.GetInputManager().lTrigger))
            {
                moveVector = bossCoreTransform.position - transform.position;
            }

            // Bulletに移動方向を代入
            bullet.GetComponent<PlayerBulletManager>().Initialize(bossCoreTransform, moveVector, adjustDistance, powerUpManager.GetIsPowerUp());

            // インターバルの再設定
            fireIntervalTimer = fireIntervalTime;
        }
    }
}
