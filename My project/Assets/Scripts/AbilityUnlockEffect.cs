using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/AbilityUnlock")]
public class AbilityUnlockEffect : CardEffect
{
    public string ScriptName;

    public override void Apply(PlayerStats stats, GameObject player)
    {
        Component ability = player.GetComponent(ScriptName);
        if (ability is MonoBehaviour mono)
        {
            mono.enabled = true;
            Debug.Log($"{ScriptName} Unlocked!");
        }
    }
}