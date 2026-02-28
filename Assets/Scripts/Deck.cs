using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Deck : MonoBehaviour
{
    public GameObject cardPrefab;
    public HoleCards holeCards;

    private static string[] RANKS = {"A","2","3","4","5","6","7","8","9","T","J","Q","K"};
    private string[] SUITS = {"c", "d", "s", "h"};

    public string[] cardsInDeck;

    public int drawingIndex = 0;

    public float timeBetweenDeals;

    private void Awake()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    private void InitializeDeck()
    {
        cardsInDeck = new string[52];
  
        for(int suitIndex = 0; suitIndex < 4; suitIndex++)
        {
            for(int rankIndex = 0; rankIndex < 13; rankIndex++)
            {
                cardsInDeck[suitIndex * 13 + rankIndex] = RANKS[rankIndex] + SUITS[suitIndex];
            }
        }

        drawingIndex = 0;
    }
    public static void Shuffle<T> (System.Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    private void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        Shuffle(rng, cardsInDeck);
        drawingIndex = 0;
    }

    public Card Draw()
    {
        Card card = Instantiate(cardPrefab, transform.position, Quaternion.identity).GetComponent<Card>();
        card.transform.localScale = transform.localScale;
        card.transform.localRotation = Quaternion.Euler(new Vector3(0,180));
        card.SetCard(cardsInDeck[drawingIndex]);

        drawingIndex++;
        return card;
    }

    public IEnumerator Deal(HoleCards[] holes, int numberOfCards, Action onFinishDeal)
    {
        for(int i = 0; i < numberOfCards; i++)
        {
            foreach(HoleCards hole in holes)
            {
                Card card = Draw();
                hole.AddNewCard(card);
                yield return new WaitForSeconds(timeBetweenDeals);
            }
            
        }

        onFinishDeal?.Invoke();
    }

    public IEnumerator UnevenDeal(HoleCards[] holes, int[] cardsToDealEach, Action onFinishDeal)
    {
        int maxNumberOfCards = cardsToDealEach.Max();

        for(int numberOfCardsDealt = 0; numberOfCardsDealt < maxNumberOfCards; numberOfCardsDealt++)
        {
            for(int holeIndex = 0; holeIndex < holes.Length; holeIndex++)
            {
                if(cardsToDealEach[holeIndex] > 0)
                {
                    cardsToDealEach[holeIndex]--;
                    Card card = Draw();
                    holes[holeIndex].AddNewCard(card);
                    yield return new WaitForSeconds(timeBetweenDeals);
                }
            }
        }

        onFinishDeal?.Invoke();
    }
}
