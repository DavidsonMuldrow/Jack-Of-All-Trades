using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ToggleItem
{
    public string ObjectID;
    public bool ShouldEnable; // (Enable = On Disable = Off)
}

[CreateAssetMenu(menuName = "Cards/New Card")]
public class CardData : ScriptableObject
{
    public string CardName;
    [TextArea] public string Description;
    public CardEffect Effect;

    [Header("Hierarchy Interaction")]
    public List<ToggleItem> itemsToToggle;
}