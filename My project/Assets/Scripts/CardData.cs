using UnityEngine;

[CreateAssetMenu(menuName = "Cards/New Card")]
public class CardData : ScriptableObject
{
    public string CardName;
    [TextArea] public string Description;
    public CardEffect Effect;
}