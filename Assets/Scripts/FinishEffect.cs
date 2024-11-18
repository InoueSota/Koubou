using UnityEngine;

public class FinishEffect : MonoBehaviour
{
    [SerializeField] private string changeSceneName;

    public void ChangeScene()
    {
        Transition transition = GameObject.FindWithTag("Transition").GetComponent<Transition>();

        if (!transition.GetIsTransitionNow())
        {
            transition.SetTransition(changeSceneName);
        }
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
