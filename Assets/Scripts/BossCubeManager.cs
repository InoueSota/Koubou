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
        // �v���C���[�̍��W�𒲐�����
        Vector3 playerRePosition = new(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z);

        // ��苗�����Ƀv���C���[��������_���[�W��^����
        if (Vector3.Distance(playerRePosition, transform.position) <= damageRange)
        {
            playerHpManager.Damage();
        }
    }
}
