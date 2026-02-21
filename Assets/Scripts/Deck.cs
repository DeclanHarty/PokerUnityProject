using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Deck : MonoBehaviour
{
    public GameObject cardPrefab;

    private static string[] RANKS = {"A","2","3","4","5","6","7","8","9","T","J","Q","K"};
    private string[] SUITS = {"c", "d", "s", "h"};

    public string[] cardsInDeck;

    public int drawingIndex = 0;

    private void Start()
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
        Shuffle<string>(rng, cardsInDeck);
        drawingIndex = 0;
    }

    public Card Draw()
    {
        Card card = Instantiate(cardPrefab, transform.position, Quaternion.identity).GetComponent<Card>();
        card.SetCard(cardsInDeck[drawingIndex]);
        
        Debug.Log(card.ToString());

        drawingIndex++;
        return card;
    }
}
