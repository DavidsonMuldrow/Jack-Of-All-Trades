using UnityEngine;

public class CardStateSyncer : MonoBehaviour
{
    void Start()
    {
        foreach (var entry in LevelModifierManager.StickyObjectStates)
        {
            GameObject obj = FindTarget(entry.Key);
            if (obj != null) obj.SetActive(entry.Value);
        }
    }

    private GameObject FindTarget(string id)
    {
        foreach (CardTarget t in Resources.FindObjectsOfTypeAll<CardTarget>())
            if (t.ObjectID == id) return t.gameObject;

        GameObject go = GameObject.Find(id);
        return go;
    }
}