using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HandComparerTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void CompareHandsOfDifferentRanks()
    {
        string[] hand1Cards = {"Ac","Ad","Ah","As","Ks"};
        string[] hand2Cards = {"2c","5d","3h","9s","Js"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.FOUR_OF_A_KIND);
        Hand hand2 = new Hand(hand2Cards, HandRanking.HIGH_CARD);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void CompareStraights()
    {
        string[] hand1Cards = {"Ac","2d","3h","4s","5s"};
        string[] hand2Cards = {"2c","3d","4h","5s","6s"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.STRAIGHT);
        Hand hand2 = new Hand(hand2Cards, HandRanking.STRAIGHT);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) < 0);
    }

    [Test]
    public void CompareFlushes1()
    {
        string[] hand1Cards = {"Ac","Kc","Jc","7c","5c"};
        string[] hand2Cards = {"Ac","Kc","Qc","Jc","7c"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.FLUSH);
        Hand hand2 = new Hand(hand2Cards, HandRanking.FLUSH);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) < 0);
    }

    [Test]
    public void CompareFours1()
    {
        string[] hand1Cards = {"Ac","Ad","Ah","As","Ks"};
        string[] hand2Cards = {"Ac","Ad","Ah","As","Qs"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.FOUR_OF_A_KIND);
        Hand hand2 = new Hand(hand2Cards, HandRanking.FOUR_OF_A_KIND);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void CompareFours2()
    {
        string[] hand1Cards = {"Ac","Ad","Ah","As","Ks"};
        string[] hand2Cards = {"Kc","Kd","Kh","Ks","As"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.FOUR_OF_A_KIND);
        Hand hand2 = new Hand(hand2Cards, HandRanking.FOUR_OF_A_KIND);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void CompareFullHouses1()
    {
        string[] hand1Cards = {"Ac","Ad","Ah","Kd","Kc"};
        string[] hand2Cards = {"Kc","Kd","Kh","Ah","Ad"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.FULL_HOUSE);
        Hand hand2 = new Hand(hand2Cards, HandRanking.FULL_HOUSE);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void CompareFullHouses2()
    {
        string[] hand1Cards = {"Ac","Ad","Ah","Kd","Kc"};
        string[] hand2Cards = {"Ac","Ad","Ah","Qh","Qd"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.FULL_HOUSE);
        Hand hand2 = new Hand(hand2Cards, HandRanking.FULL_HOUSE);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void CompareThrees1()
    {
        string[] hand1Cards = {"Ac","Ad","Ah","Qd","Kc"};
        string[] hand2Cards = {"Kc","Kd","Kh","Qh","Ad"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.THREE_OF_A_KIND);
        Hand hand2 = new Hand(hand2Cards, HandRanking.THREE_OF_A_KIND);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void CompareThrees2()
    {
        string[] hand1Cards = {"Ac","Ad","Ah","Kd","Qc"};
        string[] hand2Cards = {"Ac","Ad","Ah","Qc","Jd"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.THREE_OF_A_KIND);
        Hand hand2 = new Hand(hand2Cards, HandRanking.THREE_OF_A_KIND);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void CompareTwoPairs1()
    {
        string[] hand1Cards = {"Ac","Ad","Kh","Kd","Qc"};
        string[] hand2Cards = {"Kc","Kd","Qh","Qc","Jd"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.TWO_PAIR);
        Hand hand2 = new Hand(hand2Cards, HandRanking.TWO_PAIR);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void CompareTwoPairs2()
    {
        string[] hand1Cards = {"Ac","Ad","Kh","Kd","Qc"};
        string[] hand2Cards = {"Ac","Ad","Qh","Qc","Jd"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.TWO_PAIR);
        Hand hand2 = new Hand(hand2Cards, HandRanking.TWO_PAIR);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void CompareTwoPairs3()
    {
        string[] hand1Cards = {"Ac","Ad","Kh","Kd","Qc"};
        string[] hand2Cards = {"Ac","Ad","Kh","Kc","Jd"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.TWO_PAIR);
        Hand hand2 = new Hand(hand2Cards, HandRanking.TWO_PAIR);

        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void ComparePairs1()
    {
        string[] hand1Cards = {"Ac","Ad","4h","7d","Qc"};
        string[] hand2Cards = {"Qc","Qd","2h","8c","4d"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.ONE_PAIR);
        Hand hand2 = new Hand(hand2Cards, HandRanking.ONE_PAIR);
        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void ComparePairs2()
    {
        string[] hand1Cards = {"Ac","Ad","4h","7d","Qc"};
        string[] hand2Cards = {"Ac","Ad","2h","8c","4d"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.ONE_PAIR);
        Hand hand2 = new Hand(hand2Cards, HandRanking.ONE_PAIR);
        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }

    [Test]
    public void ComparePairs3()
    {
        string[] hand1Cards = {"Ac","Ad","4h","Qd","Kc"};
        string[] hand2Cards = {"Ac","Ad","Kh","8c","4d"};

        Hand hand1 = new Hand(hand1Cards, HandRanking.ONE_PAIR);
        Hand hand2 = new Hand(hand2Cards, HandRanking.ONE_PAIR);
        Assert.IsTrue(Hand.CompareHands(hand1, hand2) > 0);
    }
}
