using UnityEngine;

public class EffectSlashManager : MonoBehaviour
{
    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem smokeSystem;

    public void SetColor()
    {
        // Particle Systems
        var smokeMain = smokeSystem.main;
        smokeMain.startColor = new ParticleSystem.MinMaxGradient(GlobalVariables.color1);
    }
}
