using System.Collections;
using Configuration;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private RectTransform hand;
    [SerializeField] private RectTransform cardSpawnPoint;
    [SerializeField] private CardMovement cardPrefab;
    [SerializeField] private MatchConfiguration matchConfig;

    [SerializeField] private float drawDuration;

    public IEnumerator ShowCard(RealPlayer player, CardData data)
    {
        var card = Instantiate(cardPrefab, transform)
            .With(c => c.Player = player)
            .With(c => c.Card.Data = data)
            .With(c => c.TileWidth = matchConfig.TileWidth)
            .With(c => c.hand = hand)
            .With(c => c.RectPosition = cardSpawnPoint.position);

        card.transform.SetParent(hand);

        yield return new WaitForSeconds(drawDuration);
    }
}