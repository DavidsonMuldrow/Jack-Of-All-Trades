using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/StatModifier")]
public class StatModifierEffect : CardEffect
{
    public float SpeedChange;
    public int JumpChange;

    public override void Apply(PlayerStats stats, GameObject player)
    {
        int oldJumps = stats.NumberOfJumpsAllowed;
        stats.NumberOfJumpsAllowed += JumpChange;

        Debug.Log($"<color=cyan>CARD APPLIED:</color> Jumps went from {oldJumps} to {stats.NumberOfJumpsAllowed}");
    }
}
