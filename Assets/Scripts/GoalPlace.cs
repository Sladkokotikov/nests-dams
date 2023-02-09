using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoalPlace : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GoalVisualizer goalVisualizer;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Image image;
    [SerializeField] private Vector3 offset;
    
    private Goal _goal;
    public void Init(Goal goal) => _goal = new Goal(goal.Tribe, goal.Positions().ToArray());


    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.white;
        goalVisualizer.gameObject.SetActive(true);
        goalVisualizer.VisualizeGoal(rect.position + offset, _goal);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.gray;
        goalVisualizer.Clear();
        goalVisualizer.gameObject.SetActive(false);
    }
}