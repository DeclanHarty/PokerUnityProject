using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private GameObject cardFront;

    [SerializeField]
    private string cardMaterialDirectoryPath;
    public Material cardFrontMaterial;

    public string value;

    // Sets a cards rank and suit and updates the card's face material
    public void SetCard(string new_value)
    {
        value = new_value;
        Material frontCardMaterial;

        
        string frontCardMaterialPath = cardMaterialDirectoryPath + "/card_" + getSuitString() + "_" + getRankString() + ".mat";

        frontCardMaterial = AssetDatabase.LoadAssetAtPath<Material>(frontCardMaterialPath);

        cardFront.GetComponent<Renderer>().material = frontCardMaterial;
    }

    public override string ToString()
    {
        string suit = getSuitString();
        suit = Char.ToUpper(suit[0]) + suit.Substring(1);
        return value[0] + " of " + suit;
    }

    public static string[] getCardValues(Card[] cards)
    {
        string[] cardValues = new string[cards.Length];

        for(int i = 0; i < cards.Length; i++)
        {
            cardValues[i] = cards[i].value;
        }

        return cardValues;
    }

    public string getSuitString()
    {
        switch (value[1])
        {
            case 'c':
                return "clubs";
            case 'd':
                return "diamonds";
            case 'h':
                return "hearts";
            case 's':
                return "spades";
            default:
                return "";
        }
    }

    public string getRankString()
    {
        char rank = value[0];

        switch (rank)
        {
            case 'K' or 'Q' or 'J' or 'A':
                return rank.ToString();
            case 'T':
                return "10";
            default:
                return "0" + rank.ToString();

        }
    }
}

public class CardComparer : IComparer<Card>
{
    private string[] RANKS = {"A","2","3","4","5","6","7","8","9","T","J","Q","K"};
    public int Compare(Card a, Card b)
    {
        int a_rank_index = Array.IndexOf(RANKS, a.value[0]);
        int b_rank_index = Array.IndexOf(RANKS, a.value[0]);

        return a_rank_index - b_rank_index;
    }
}