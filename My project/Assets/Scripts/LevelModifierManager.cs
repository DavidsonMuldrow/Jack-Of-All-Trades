using System.Collections.Generic;

public static class LevelModifierManager
{
    public static List<CardData> ChosenCards = new List<CardData>();
    public static List<CardData> RemovedFromPool = new List<CardData>();

    public static void AddCard(CardData card)
    {
        if (card == null) return;

        ChosenCards.Add(card);

        RemovedFromPool.Add(card);
    }

    public static void ResetGameRun()
    {
        ChosenCards.Clear();
        RemovedFromPool.Clear();
    }
}