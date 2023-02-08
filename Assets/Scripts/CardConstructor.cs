using System;
using UnityEditor;
using UnityEngine;


public class CardConstructor : MonoBehaviour
{
    private int _counter;
    private void Start()
    {
        CreateCard(Tribe.Beaver, "Бобрёнок", "", new ConfirmationAbility());
        CreateCard(Tribe.Beaver, "Бобёр-забияка", "Убейте отряд", new ConfirmationAbility().Kill().Confirm());
        CreateCard(Tribe.Beaver, "Бобёр-учитель", "Возьмите карту", new ConfirmationAbility().Draw().ConfirmAuto());
    }

    private void CreateCard(Tribe tribe, string cardName, string mask, ConfirmationAbility ability)
    {
        var data = ScriptableObject.CreateInstance<CardData>();
        data.Load(_counter++, tribe, cardName, mask, ability);
        AssetDatabase.CreateAsset(data, $"Assets/CardAssets/{cardName}.asset");
        AssetDatabase.SaveAssets();
    }

    public static CardData GetCard(string cardName)
    {
        var data = AssetDatabase.LoadAssetAtPath<CardData>($"Assets/CardAssets/{cardName}.asset");
        if (data == null)
            throw new ArgumentException();
        return data;
    }
}