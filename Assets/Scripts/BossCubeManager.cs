using UnityEngine;

public class BossCubeManager : MonoBehaviour
{
    [Header("Other Object")]
    [SerializeField] private GameObject playerObj;
    private PlayerHpManager playerHpManager;

    [Header("Parameter")]
    [SerializeField] private float damageRange;

    void Start()
    {
        playerHpManager = playerObj.GetComponent<PlayerHpManager>();
    }

    void Update()
    {
        // プレイヤーの座標を調整する
        Vector3 playerRePosition = new(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z);

        // 一定距離内にプレイヤーがいたらダメージを与える
        if (Vector3.Distance(playerRePosition, transform.position) <= damageRange)
        {
            playerHpManager.Damage();
        }
    }
}
