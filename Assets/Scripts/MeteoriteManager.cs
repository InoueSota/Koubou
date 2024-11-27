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

    // ������\��
    private GameObject fallInfomation;

    // �v���C���[
    private Vector3 playerRePosition;

    public void Initialize(Vector3 _randomPosition, GameObject _fallInfomation, Transform _pillarParent, Vector3 _playerPosition)
    {
        // �n�ʂ�����̍����ɂ���
        transform.position = new(_randomPosition.x, startHeight, _randomPosition.z);

        // �ړ��J�n
        transform.DOMove(new(_randomPosition.x, 0.5f, _randomPosition.z), fallTime).SetEase(Ease.Linear).OnComplete(FinishFall);

        // �������\������I�u�W�F�N�g���擾
        fallInfomation = _fallInfomation;

        // ���̐e�g�����X�t�H�[�����擾
        pillarParent = _pillarParent;

        // �v���C���[�̒����������W���擾
        playerRePosition = new(_playerPosition.x, 0f, _playerPosition.z);
    }

    void FinishFall()
    {
        // �v���C���[����苗���ɂ�����_���[�W��^����
        if (Vector3.Distance(transform.position, playerRePosition) <= 0.8f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHpManager>().Damage();
        }

        // �����G�t�F�N�g
        Instantiate(explosion, transform.position, Quaternion.identity);

        // ��
        GameObject pillar = Instantiate(pillarPrefab, new(transform.position.x, 3f, transform.position.z), Quaternion.identity);
        pillar.transform.parent = pillarParent;

        // ������\������
        Destroy(fallInfomation);

        // ����
        Destroy(gameObject);
    }
}
