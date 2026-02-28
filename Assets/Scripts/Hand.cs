using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand
{
    string[] cards;
    public HandRanking ranking {get;}

    public Hand(string[] cards, HandRanking ranking)
    {
        this.cards = cards;
        this.ranking = ranking;
    }

    public Hand(List<Card> cards)
    {
        this.cards = Card.getCardValues(cards.ToArray());
        ranking = HandRankingFinder.DetermineHandFromSetOf5(this.cards);
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

        int[] hand1Ranks = HandRankingFinder.GetRankIndices(hand1.cards);
        int[] hand2Ranks = HandRankingFinder.GetRankIndices(hand2.cards);

        const int ACE_RANK_INDEX = 12;
        const int FIVE_RANK_INDEX = 3;

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

        Dictionary<char, int> hand1RankMap = HandRankingFinder.GetRankMap(hand1.cards);
        Dictionary<char, int> hand2RankMap = HandRankingFinder.GetRankMap(hand2.cards);

        if(hand1.ranking == HandRanking.FOUR_OF_A_KIND || hand1.ranking == HandRanking.FULL_HOUSE)
        {
            char hand1Top = hand1RankMap.First(x => x.Value == 4 || x.Value == 3).Key;
            char hand2Top = hand2RankMap.First(x => x.Value == 4 || x.Value == 3).Key;

            int hand1TopRank = HandRankingFinder.GetRankIndex(hand1Top.ToString());
            int hand2TopRank = HandRankingFinder.GetRankIndex(hand2Top.ToString());

            if(hand1TopRank != hand2TopRank)
            {
                return hand1TopRank - hand2TopRank;
            }

            char hand1Kicker = hand1RankMap.First(x => x.Key != hand1Top).Key;
            char hand2Kicker = hand2RankMap.First(x => x.Key != hand2Top).Key;

            int hand1KickerRank = HandRankingFinder.GetRankIndex(hand1Kicker.ToString());
            int hand2KickerRank = HandRankingFinder.GetRankIndex(hand2Kicker.ToString());

            return hand1KickerRank - hand2KickerRank;
        }

        if(hand1.ranking == HandRanking.THREE_OF_A_KIND)
        {
            char hand1Top = hand1RankMap.First(x => x.Value == 3).Key;
            char hand2Top = hand2RankMap.First(x => x.Value == 3).Key;

            int hand1TopRank = HandRankingFinder.GetRankIndex(hand1Top.ToString());
            int hand2TopRank = HandRankingFinder.GetRankIndex(hand2Top.ToString());

            if(hand1TopRank != hand2TopRank)
            {
                return hand1TopRank - hand2TopRank;
            }

            for(int i = 0; i < 5; i++)
            {
                if(hand1Ranks[i] != hand2Ranks[i])
                {
                    return hand1Ranks[i] - hand2Ranks[i];
                }
            }
            
            return 0;
        }

        if(hand1.ranking == HandRanking.TWO_PAIR)
        {
            string[] hand1Pairs = hand1RankMap.Where(x => x.Value == 2).Select(x => x.Key.ToString()).ToArray();
            string[] hand2Pairs = hand2RankMap.Where(x => x.Value == 2).Select(x => x.Key.ToString()).ToArray();

            int[] hand1PairsRanks = HandRankingFinder.GetRankIndices(hand1Pairs);
            int[] hand2PairsRanks = HandRankingFinder.GetRankIndices(hand2Pairs);

            for(int i = 0; i < 2; i++)
            {
                if(hand1PairsRanks[i] != hand2PairsRanks[i])
                {
                    return hand1PairsRanks[i] - hand2PairsRanks[i];
                }
            }

            string hand1Kicker = hand1RankMap.First(x => x.Value == 1).Key.ToString();
            string hand2Kicker = hand2RankMap.First(x => x.Value == 1).Key.ToString(); 

            int hand1KickerRank = HandRankingFinder.GetRankIndex(hand1Kicker);
            int hand2KickerRank = HandRankingFinder.GetRankIndex(hand2Kicker);

            return hand1KickerRank - hand2KickerRank;
        }

        if(hand1.ranking == HandRanking.ONE_PAIR)
        {
            string hand1Pair = hand1RankMap.First(x => x.Value == 2).Key.ToString();
            string hand2Pair = hand2RankMap.First(x => x.Value == 2).Key.ToString();

            int hand1PairRank = HandRankingFinder.GetRankIndex(hand1Pair);
            int hand2PairRank = HandRankingFinder.GetRankIndex(hand2Pair); 

            if(hand1PairRank != hand2PairRank)
            {
                return hand1PairRank - hand2PairRank;
            }

            for(int i = 0; i < 5; i++){
                if(hand1Ranks[i] != hand2Ranks[i])
                {
                    return hand1Ranks[i] - hand2Ranks[i];
                }
            }

            return 0;
        }

        if(hand1.ranking == HandRanking.HIGH_CARD)
        {
            for(int i = 0; i < 5; i++)
            {
                if(hand1Ranks[i] != hand2Ranks[i])
                {
                    return hand1Ranks[i] - hand2Ranks[i];
                }
            }

            return 0;
        }

        return 0;
    }

    public static HandRanking GetHighestHandRanking(Hand[] hands)
    {
        HandRanking highestHandRanking = HandRanking.HIGH_CARD;

        for(int i = 0; i < hands.Length; i++)
        {
            if(hands[i].ranking > highestHandRanking)
            {
                highestHandRanking = hands[i].ranking;
            }
        }

        return highestHandRanking;
    }

    public static Hand[] GetHandsOfSpecificRanking(Hand[] hands, HandRanking handRanking)
    {
        List<Hand> handsOfRanking = new List<Hand>();

        foreach(Hand hand in hands)
        {
            if(hand.ranking == handRanking)
            {
                handsOfRanking.Add(hand);
            }
        }

        return handsOfRanking.ToArray();
    }

    public static List<Hand> GetBestHand(Hand[] hands)
    {
        List<Hand> winningHands = new List<Hand>();

        foreach(Hand hand in hands)
        {
            if(winningHands.Count == 0)
            {
                winningHands.Add(hand);
                continue;
            }

            int comparison = CompareHands(hand, winningHands[0]);
            if(comparison > 0)
            {
                winningHands.Clear();
                winningHands.Add(hand);
            }else if(comparison == 0)
            {
                winningHands.Add(hand);
            }
        }

        return winningHands;
    }
}
