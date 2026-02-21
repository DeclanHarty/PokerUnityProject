using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand
{
    Card[] cards;
    public HandRanking ranking {get;}

    public Hand(Card[] cards, HandRanking ranking)
    {
        this.cards = cards;
        this.ranking = ranking;
    }
    /// <summary>
    /// Compares 2 poker hands to see which is the more powerful hand
    /// </summary> 
    /// <param name="hand1"></param>
    /// <param name="hand2"></param>
    /// <returns>An integer that is either negative, positve, or zero. A positive integar means hand1 is more powerful, a negative integer means hand2 is more powerful, and zero means they are equal.</returns>
    public static int CompareHands(Hand hand1, Hand hand2)
    {
        if(hand1.ranking != hand2.ranking)
        {
            return hand1.ranking - hand2.ranking;
        }

        string[] hand1Values = Card.getCardValues(hand1.cards);
        string[] hand2Values = Card.getCardValues(hand2.cards);

        int[] hand1Ranks = HandRankingFinder.GetRankIndices(hand1Values);
        int[] hand2Ranks = HandRankingFinder.GetRankIndices(hand2Values);

        const int ACE_RANK_INDEX = 12;
        const int FIVE_RANK_INDEX = 4;

        if(hand1.ranking == HandRanking.STRAIGHT || hand1.ranking == HandRanking.STRAIGHT_FLUSH)
        {
            int hand1HighCard = hand1Ranks[0];
            int hand2HighCard = hand2Ranks[0];

            if(hand1Ranks[0] == ACE_RANK_INDEX)
            {
                if(hand1Ranks[1] == FIVE_RANK_INDEX)
                {
                    hand1HighCard = FIVE_RANK_INDEX;
                }
            }

            if(hand2Ranks[0] == ACE_RANK_INDEX)
            {
                if(hand2Ranks[1] == FIVE_RANK_INDEX)
                {
                    hand2HighCard = FIVE_RANK_INDEX;
                }
            }

            return hand1HighCard - hand2HighCard;
        }

        if(hand1.ranking == HandRanking.FLUSH)
        {
            for(int i = 0; i < hand1Ranks.Length; i++){
                if(hand1Ranks[i] == hand2Ranks[i])
                {
                    continue;
                }

                return hand1Ranks[i] - hand2Ranks[i]; 
            }
            return 0;
        }

        Dictionary<char, int> hand1RankMap = HandRankingFinder.GetRankMap(hand1Values);
        Dictionary<char, int> hand2RankMap = HandRankingFinder.GetRankMap(hand2Values);


        if(hand1.ranking == HandRanking.FOUR_OF_A_KIND)
        {
            char hand1Top = hand1RankMap.First(x => x.Value == 4).Key;
            char hand2Top = hand2RankMap.First(x => x.Value == 4).Key;

            int hand1TopRank = HandRankingFinder.GetRankIndex(hand1Top.ToString());
            int hand2TopRank = HandRankingFinder.GetRankIndex(hand2Top.ToString());

            if(hand1TopRank != hand2TopRank)
            {
                return hand1TopRank - hand2TopRank;
            }

            char hand1Kicker = hand1RankMap.First(x => x.Key != hand1Top).Key;
            char hand2Kicker = hand2RankMap.First(x => x.Key != hand2Top).Key;
        }


    }
}
