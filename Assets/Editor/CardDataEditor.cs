using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardData))]
public class CardDataEditor : Editor
{
    private int _selectedIndex;
    private string[] _possibles;
    private CardData _obj;

    private bool Valid => _obj.Tail == BytecodeBasis.Confirm || _obj.Tail == BytecodeBasis.ConfirmRandom ||
                          _obj.Tail == BytecodeBasis.ConfirmAuto;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        _obj = (CardData) target;
        _obj.InitHiddenCommands();


        _obj.id = EditorGUILayout.IntField("Card ID:", _obj.id);
        _obj.tribe = (Tribe) EditorGUILayout.EnumPopup("Tribe:", _obj.tribe);
        _obj.cardName = EditorGUILayout.TextField("Card Name:", _obj.cardName);
        _obj.abilityMask = EditorGUILayout.TextField("Ability Text:", _obj.abilityMask);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Ability:", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        foreach (var command in _obj.ShownCommands)
            EditorGUILayout.LabelField(command.ToString());
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        _possibles = GetPossibleContinuations(_obj.Tail);
        _selectedIndex = EditorGUILayout.Popup("Next:", _selectedIndex, _possibles);
        if (GUILayout.Button("Add"))
            AddCommand();

        EditorGUILayout.LabelField(Valid ? "Card is valid!" : "Card is invalid",
            new GUIStyle
            {
                normal =
                {
                    textColor = Valid ? Color.green : Color.red,
                }
            });

        if (GUILayout.Button("Clear"))
            _obj.Clear();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        _obj.image = (Sprite) EditorGUILayout.ObjectField("Full Image:", _obj.image, typeof(Sprite), false);
        _obj.icon = (Sprite) EditorGUILayout.ObjectField("Small Image:", _obj.icon, typeof(Sprite), false);
    }

    private void AddCommand()
    {
        var command = (BytecodeBasis) Enum.Parse(typeof(BytecodeBasis), _possibles[_selectedIndex]);
        _obj.AddCommand(command);
    }

    private string[] GetPossibleContinuations(BytecodeBasis objTail)
    {
        return objTail switch
        {
            BytecodeBasis.Confirm => ActionCommands,
            BytecodeBasis.ConfirmAuto => ActionCommands,
            BytecodeBasis.ConfirmRandom => ActionCommands,

            BytecodeBasis.Spawn => Join(TileCriteriaCommands, TribeCriteriaCommands),
            BytecodeBasis.Kill => Join(TileCriteriaCommands, TribeCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Push => Join(TileCriteriaCommands, TribeCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Pull => Join(TileCriteriaCommands, TribeCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Draw => Join(AutoCommand),
            BytecodeBasis.Lock => Join(TileCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Unlock => Join(TileCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Break => Join(TileCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Build => Join(TileCriteriaCommands, ConfirmationCommands),

            BytecodeBasis.Beaver => Join(ConfirmationCommands),
            BytecodeBasis.Magpie => Join(ConfirmationCommands),

            BytecodeBasis.Adjacent => Join(TileCriteriaCommands, TribeCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Surrounding => Join(TileCriteriaCommands, TribeCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Plus => Join(TileCriteriaCommands, TribeCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Edge => Join(TileCriteriaCommands, TribeCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Free => Join(TileCriteriaCommands, ConfirmationCommands),
            BytecodeBasis.Occupied => Join(TileCriteriaCommands, TribeCriteriaCommands, ConfirmationCommands),


            _ => Array.Empty<string>()
        };
    }

    private static string[] ActionCommands => new[]
    {
        BytecodeBasis.Spawn.ToString(),
        BytecodeBasis.Kill.ToString(),
        BytecodeBasis.Push.ToString(),
        BytecodeBasis.Pull.ToString(),
        BytecodeBasis.Draw.ToString(),
        BytecodeBasis.Lock.ToString(),
        BytecodeBasis.Unlock.ToString(),
        BytecodeBasis.Break.ToString(),
        BytecodeBasis.Build.ToString()
    };

    private string[] TribeCriteriaCommands => new[]
    {
        BytecodeBasis.Beaver.ToString(),
        BytecodeBasis.Magpie.ToString(),
    };

    private string[] TileCriteriaCommands => new[]
    {
        BytecodeBasis.Adjacent.ToString(),
        BytecodeBasis.Surrounding.ToString(),
        BytecodeBasis.Plus.ToString(),
        BytecodeBasis.Edge.ToString(),
        BytecodeBasis.Free.ToString(),
        BytecodeBasis.Occupied.ToString()
    };

    private string[] ConfirmationCommands => new[]
    {
        BytecodeBasis.Confirm.ToString(),
        BytecodeBasis.ConfirmRandom.ToString(),
    };

    private string[] AutoCommand => new[]
    {
        BytecodeBasis.ConfirmAuto.ToString(),
    };

    private string[] Join(params IEnumerable<string>[] collections)
        => collections.SelectMany(c => c).ToArray();
}