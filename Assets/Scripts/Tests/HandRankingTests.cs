using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HandRankingTests
{
    [Test]
    public void HandRankingStraightFlush()
    {
        string[] cardValues = {"Ac", "Kc", "Qc", "Jc", "Tc"};

        HandRanking ranking = HandRankingFinder.DetermineHandFromSetOf5(cardValues);

        Assert.AreEqual(ranking, HandRanking.STRAIGHT_FLUSH);
    }

    [Test]
    public void HandRankingFourOfAKind()
    {
        string[] cardValues = {"Kd", "Kc", "Kh", "Ks", "Tc"};

        HandRanking ranking = HandRankingFinder.DetermineHandFromSetOf5(cardValues);

        Assert.AreEqual(ranking, HandRanking.FOUR_OF_A_KIND);
    }

    [Test]
    public void HandRankingFullHouse()
    {
        string[] cardValues = {"Kd", "Kc", "Kh", "Ts", "Tc"};

        HandRanking ranking = HandRankingFinder.DetermineHandFromSetOf5(cardValues);

        Assert.AreEqual(ranking, HandRanking.FULL_HOUSE);
    }

    [Test]
    public void HandRankingFlush()
    {
        string[] cardValues = {"Kd", "6d", "2d", "Jd", "5d"};

        HandRanking ranking = HandRankingFinder.DetermineHandFromSetOf5(cardValues);

        Assert.AreEqual(ranking, HandRanking.FLUSH);
    }

    [Test]
    public void HandRankingBottomStraight()
    {
        string[] cardValues = {"Ad", "2c", "3h", "4s", "5c"};

        HandRanking ranking = HandRankingFinder.DetermineHandFromSetOf5(cardValues);

        Assert.AreEqual(ranking, HandRanking.STRAIGHT);
    }

    [Test]
    public void HandRankingThreeOfAKind()
    {
        string[] cardValues = {"Kd", "Kc", "Kh", "Ts", "Jc"};

        HandRanking ranking = HandRankingFinder.DetermineHandFromSetOf5(cardValues);

        Assert.AreEqual(ranking, HandRanking.THREE_OF_A_KIND);
    }

    [Test]
    public void HandRankingTwoPair()
    {
        string[] cardValues = {"Kd", "Kc", "Qh", "Ts", "Tc"};

        HandRanking ranking = HandRankingFinder.DetermineHandFromSetOf5(cardValues);

        Assert.AreEqual(ranking, HandRanking.TWO_PAIR);
    }

    [Test]
    public void HandRankingOnePair()
    {
        string[] cardValues = {"Kd", "Kc", "7h", "2s", "Tc"};

        HandRanking ranking = HandRankingFinder.DetermineHandFromSetOf5(cardValues);

        Assert.AreEqual(ranking, HandRanking.ONE_PAIR);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator HandRankingTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
