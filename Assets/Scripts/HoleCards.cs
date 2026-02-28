using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlasticGui.WorkspaceWindow;
using UnityEngine;

public class HoleCards : MonoBehaviour
{
    public List<Card> cards;

    public float timeBetweenCardFlips;

    public List<bool> cardIsFlipped;

    public void AddNewCard(Card card)
    {
        int insertIndex = Mathf.FloorToInt(cards.Count / 2);
        cards.Insert(insertIndex, card);
        cardIsFlipped.Insert(insertIndex, false);

        float cardXScale = card.gameObject.transform.localScale.x;

        for(int i = 0; i < cards.Count; i++)
        {
            if(i == insertIndex)
            {
                continue;
            }

            StartCoroutine(cards[i].SlideToPosition(new Vector3(transform.position.x - cardXScale * insertIndex + cardXScale * i, transform.position.y), cards[i].gameObject.transform.position));
        }

        StartCoroutine(card.DealToPosition(transform.position));
    }

    public Card[] DiscardCards(int[] cardIndices, Vector2 discardPosition)
    {
        cardIndices = cardIndices.OrderByDescending(index => index).ToArray();
        int numberOfCardsToDiscard = cardIndices.Length;
        Card[] cardsToDiscard = new Card[numberOfCardsToDiscard];

        for(int i = 0; i < numberOfCardsToDiscard; i++)
        {
            cardsToDiscard[i] = cards[cardIndices[i]];
            cards.RemoveAt(cardIndices[i]);
            cardIsFlipped.RemoveAt(cardIndices[i]);
        }

        foreach(Card card in cardsToDiscard)
        {
            card.gameObject.transform.localEulerAngles = new Vector3(0,180,0);
            StartCoroutine(card.SlideToPosition(discardPosition, card.gameObject.transform.position));
        }

        return cardsToDiscard;
    }

    public IEnumerator RevealCards()
    {
        for(int i = 0; i < cards.Count; i++)
        {
            if (!cardIsFlipped[i])
            {
                StartCoroutine(cards[i].FlipCard());
                yield return new WaitForSeconds(timeBetweenCardFlips);
            }
        }
    }

}
