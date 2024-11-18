using UnityEngine;

public class EffectParent : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
