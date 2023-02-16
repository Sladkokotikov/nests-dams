using UnityEngine;
using UnityEngine.UI;

public class AutoSize : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup group;
    [SerializeField] private RectTransform rect;
    [SerializeField] private float width;


    private void OnTransformChildrenChanged()
    {
        var y = Mathf.CeilToInt((float) transform.childCount / group.constraintCount);
        rect.sizeDelta = new Vector2(width, group.spacing.y + y *
            (group.cellSize.y + group.spacing.y));
    }
}