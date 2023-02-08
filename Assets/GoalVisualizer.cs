using System;
using UnityEngine;
using UnityEngine.UI;

public class GoalVisualizer : MonoBehaviour
{
    [SerializeField] private RectTransform smallTilePref;
    [SerializeField] private RectTransform rect;
    [SerializeField] private float tileWidth;
    [SerializeField] private float spacing;
    [SerializeField] private Color beaverColor;
    [SerializeField] private Color magpieColor;
    
    public void VisualizeGoal(Vector3 position, Goal goal)
    {
        rect.position = position;
        foreach (var pos in goal.Positions())
        {
            var smallTile = Instantiate(smallTilePref, rect);
            var offset = 0.5f * new Vector2(goal.RightUp.x % 2, goal.RightUp.y % 2);
            smallTile.anchoredPosition = (tileWidth + spacing) * (pos - goal.RightUp / 2 - offset);
            smallTile.sizeDelta = tileWidth * Vector2.one;
            smallTile.GetComponent<Image>().color = goal.Tribe switch
            {
                Tribe.Beaver => beaverColor,
                Tribe.Magpie => magpieColor,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public void Clear()
    {
        rect.Clear();
    }
}
