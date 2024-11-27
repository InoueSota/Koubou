using DG.Tweening;
using UnityEngine;

public class MeteoriteManager : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private float fallTime;
    [SerializeField] private float startHeight;

    [Header("Effect")]
    [SerializeField] private GameObject explosion;

    [Header("Pillar")]
    [SerializeField] private GameObject pillarPrefab;
    private Transform pillarParent;

    // 落下先表示
    private GameObject fallInfomation;

    // プレイヤー
    private Vector3 playerRePosition;

    public void Initialize(Vector3 _randomPosition, GameObject _fallInfomation, Transform _pillarParent, Vector3 _playerPosition)
    {
        // 地面から一定の高さにする
        transform.position = new(_randomPosition.x, startHeight, _randomPosition.z);

        // 移動開始
        transform.DOMove(new(_randomPosition.x, 0.5f, _randomPosition.z), fallTime).SetEase(Ease.Linear).OnComplete(FinishFall);

        // 落下先を表示するオブジェクトを取得
        fallInfomation = _fallInfomation;

        // 柱の親トランスフォームを取得
        pillarParent = _pillarParent;

        // プレイヤーの調整した座標を取得
        playerRePosition = new(_playerPosition.x, 0f, _playerPosition.z);
    }

    void FinishFall()
    {
        // プレイヤーが一定距離にいたらダメージを与える
        if (Vector3.Distance(transform.position, playerRePosition) <= 0.8f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHpManager>().Damage();
        }

        // 爆発エフェクト
        Instantiate(explosion, transform.position, Quaternion.identity);

        // 柱
        GameObject pillar = Instantiate(pillarPrefab, new(transform.position.x, 3f, transform.position.z), Quaternion.identity);
        pillar.transform.parent = pillarParent;

        // 落下先表示消滅
        Destroy(fallInfomation);

        // 消滅
        Destroy(gameObject);
    }
}
