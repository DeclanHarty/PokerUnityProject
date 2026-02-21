using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class HandRankingFinder
{
    private static char[] RANKS = {'2','3','4','5','6','7','8','9','T','J','Q','K','A'};
    private static char[] SUITS = {'c', 'd', 's', 'h'};

    /// <summary>
    /// Determines whether a set of 5 cards is a straight
    /// </summary>
    /// <param name="cardValues">List of the 5 values of cards</param>
    /// <returns></returns>
    private static bool IsStraight(string[] cardValues)
    {
        const int MAX_RANK_INDEX = 12;
        const int BOTTOM_ACE_TOP_INDEX = 4;

        string[] cardsSortedByRank = cardValues.OrderByDescending(card => GetRankIndex(card)).ToArray();

        int topCardIndex = Array.IndexOf(RANKS, cardsSortedByRank[0][0]);

        bool containsAce = topCardIndex == MAX_RANK_INDEX;


        for(int i = 1; i < 5; i++)
        {
            int rankIndex = topCardIndex - i;

            if(RANKS[rankIndex] != cardsSortedByRank[i][0])
            {
                // Checks if Ace should be considered the bottom card
                if(containsAce && cardsSortedByRank[i][0] == '5' && i == 1  )
                {
                    topCardIndex = BOTTOM_ACE_TOP_INDEX; 
                    continue;
                }

                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if whether a set of 5 cards are all the same suit
    /// </summary>
    /// <param name="cards">Array of 5 cards</param>
    /// <returns></returns>
    private static bool IsFlush(string[] cardValues)
    {
        const char UNDEFINED_SUIT = '-';
        char suit = UNDEFINED_SUIT;
        for(int i = 0; i < 5; i++)
        {
            if(suit == UNDEFINED_SUIT)
            {
                suit = cardValues[i][1];
            }

            if(suit != cardValues[i][1])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets a map of cards that represents the number of cards of a given rank in a set of 5
    /// </summary>
    /// <param name="cards">Array of 5 cards</param>
    /// <returns>A char, int dictionary where the char is the rank and the int is the number of a times there is a card of that rank</returns>
    public static Dictionary<char,int> GetRankMap(string[] cardValues)
    {
        Dictionary<char, int> rankMap = new Dictionary<char, int>();

        for(int i = 0; i < 5; i++)
        {
            char cardRank = cardValues[i][0];
            if (!rankMap.ContainsKey(cardRank))
            {
                rankMap.Add(cardRank, 1);
            }
            else
            {
                rankMap[cardRank]++;
            }
        }

        return rankMap;
    }
    /// <summary>
    /// Returns whether a set of 5 cards contains a 4 of a kind
    /// </summary>
    /// <param name="rankMap">A dictionary representing the number of cards of a given suit</param>
    /// <returns></returns>
    private static bool ContainsFourOfAKind(Dictionary<char,int> rankMap)
    {

        int[] counts = rankMap.Values.ToArray();
        return rankMap.Count == 2 && counts[0] == 1 || counts[0] == 4;
    }

    /// <summary>
    /// Determines whether a set of 5 cards contains a Three of a Kind
    /// </summary>
    /// <param name="rankMap"></param>
    /// <returns></returns>
    private static bool ContainsThreeOfAKind(Dictionary<char,int> rankMap)
    {
        int[] counts = rankMap.Values.ToArray();

        foreach(int count in counts)
        {
            if(count == 3)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines how many pairs a set of 5 cards contains
    /// </summary>
    /// <param name="rankMap"></param>
    /// <returns>Number of pairs</returns>
    private static int GetNumberOfPairs(Dictionary<char,int> rankMap)
    {
        int[] counts = rankMap.Values.ToArray();

        int numberOfPairs = 0;

        foreach(int count in counts)
        {
            if(count == 2)
            {
                numberOfPairs++;
            }
        }

        return numberOfPairs;
    }

    public static HandRanking DetermineHandFromSetOf5(string[] cardValues)
    {
        if(cardValues.Count() != 5)
        {
            throw new System.ArgumentException("Must be an array of 5 cards");
        }

        bool setIsStraight = IsStraight(cardValues);
        bool setIsFlush = IsFlush(cardValues);

        if(setIsFlush && setIsStraight)
        {
            return HandRanking.STRAIGHT_FLUSH;
        }

        Dictionary<char,int> rankMap = GetRankMap(cardValues);

        if (ContainsFourOfAKind(rankMap))
        {
            return HandRanking.FOUR_OF_A_KIND;
        }

        bool hasThreeOfAKind = ContainsThreeOfAKind(rankMap);
        int numberOfPairs = GetNumberOfPairs(rankMap);

        if(hasThreeOfAKind && numberOfPairs == 1)
        {
            return HandRanking.FULL_HOUSE;
        }

        if (setIsFlush)
        {
            return HandRanking.FLUSH;
        }

        if (setIsStraight)
        {
            return HandRanking.STRAIGHT;
        }

        if (hasThreeOfAKind)
        {
            return HandRanking.THREE_OF_A_KIND;
        }

        if (numberOfPairs == 2)
        {
            return HandRanking.TWO_PAIR;
        }

        if (numberOfPairs == 1)
        {
            return HandRanking.ONE_PAIR;
        }

        return HandRanking.HIGH_CARD;
    }

    public static int GetRankIndex(string cardValue)
    {
        return Array.IndexOf(RANKS, cardValue[0]);
    }

    public static string[] SortByRankDescending(string[] cardValues)
    {
        return cardValues.OrderByDescending(card => GetRankIndex(card)).ToArray();
    }

    public static int[] GetRankIndices(string[] cardValues)
    {
        int[] ranks = new int[cardValues.Length];

        for(int i = 0; i < cardValues.Length; i++)
        {
            ranks[i] = GetRankIndex(cardValues[i]);
        }

        ranks = ranks.OrderByDescending(rank => rank).ToArray();

        return ranks;
    }
}

public enum HandRanking
{
    HIGH_CARD,
    ONE_PAIR,
    TWO_PAIR,
    THREE_OF_A_KIND,
    STRAIGHT,
    FLUSH,
    FULL_HOUSE,
    FOUR_OF_A_KIND,
    STRAIGHT_FLUSH,
}
