using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
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

    public float dealSpeed;
    public float flipSpeedInDegreesPerSecond;
    public float turnSpeedInDegreesPerSecond;


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

    public float EaseOutExpoDeal(float x){
        
        return -1 * Mathf.Pow(4, 10 * (x - 1.001f)) + 1;
    }

    public IEnumerator SlideToPosition(Vector3 position, Vector3 startPosition)
    {
        float t = 0;
        float distanceToTravel = Vector3.Distance(startPosition, position);
        while(transform.position != position)
        {
            t = Vector3.Distance(startPosition, transform.position) / distanceToTravel;

            float deltaPos = EaseOutExpoDeal(t) * dealSpeed * Time.deltaTime;

            Vector3 newPosition = Vector3.MoveTowards(transform.position, position, deltaPos);
            
            transform.position = newPosition;
            yield return null;
        }
    }

    public IEnumerator DealToPosition(Vector3 position, bool flipAfterDeal = false)
    {
        Vector3 startPosition = transform.position;

        Vector3 differenceVector = position - startPosition;

        float zAngle = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(differenceVector.normalized, Vector2.right)) + 90;

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zAngle);

        yield return StartCoroutine(SlideToPosition(position, startPosition));

        while(transform.localEulerAngles.z != 180)
        {
            float newZAngle = Mathf.MoveTowardsAngle(transform.localEulerAngles.z, 180, turnSpeedInDegreesPerSecond * Time.deltaTime);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, newZAngle);

            yield return null;
        }
    }

    public IEnumerator FlipCard()
    {
        while(transform.localEulerAngles.y != 0)
        {
            float newYAngle = Mathf.MoveTowards(transform.localEulerAngles.y, 0, flipSpeedInDegreesPerSecond * Time.deltaTime);
            transform.localEulerAngles = new Vector3(0, newYAngle, 0);

            yield return null;
        }
    }
}

public class CardComparer : IComparer<Card>
{
    private string[] RANKS = {"2","3","4","5","6","7","8","9","T","J","Q","K","A"};
    public int Compare(Card a, Card b)
    {
        int a_rank_index = Array.IndexOf(RANKS, a.value[0]);
        int b_rank_index = Array.IndexOf(RANKS, a.value[0]);

        return a_rank_index - b_rank_index;
    }
}