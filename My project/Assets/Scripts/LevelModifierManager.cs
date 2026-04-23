using System.Collections.Generic;

public static class LevelModifierManager
{
    public static List<CardData> ChosenCards = new List<CardData>();
    public static List<string> RemovedFromPool = new List<string>();
    public static Dictionary<string, bool> StickyObjectStates = new Dictionary<string, bool>();
    public static float MaxRunTime = 45f;

    public static void AddCard(CardData card)
    {
        ChosenCards.Add(card);
        if (!RemovedFromPool.Contains(card.CardName)) RemovedFromPool.Add(card.CardName);

        if (card.itemsToToggle != null)
        {
            foreach (var item in card.itemsToToggle)
            {
                if (!string.IsNullOrEmpty(item.ObjectID))
                    StickyObjectStates[item.ObjectID] = item.ShouldEnable;
            }
        }
    }
}