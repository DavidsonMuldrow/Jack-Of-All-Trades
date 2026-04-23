using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/StatModifier")]
public class StatModifierEffect : CardEffect
{
    public float SpeedChange;
    public int JumpChange;
    public float TimePenalty = 7f;

    public override void Apply(PlayerStats stats, GameObject player)
    {
        int oldJumps = stats.NumberOfJumpsAllowed;
        float oldSpeed = stats.MoveSpeed;

        stats.NumberOfJumpsAllowed += JumpChange;
        stats.MoveSpeed += SpeedChange;

        if (HealthManager.Instance != null)
        {
            HealthManager.Instance.timeRemaining = LevelModifierManager.MaxRunTime;
        }

        Debug.Log($"<color=cyan>CARD APPLIED:</color> Jumps: {oldJumps}->{stats.NumberOfJumpsAllowed} | Speed: {oldSpeed}->{stats.MoveSpeed}");
    }
}