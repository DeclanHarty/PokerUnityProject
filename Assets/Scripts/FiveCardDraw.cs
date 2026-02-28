using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codice.Client.Common;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;

public class FiveCardDraw : MonoBehaviour
{
    int currentPlayerIndex = 0;
    int pot = 0;
    int ante;

    public GameObject holeCardPrefab;
    public GameObject discardPile;
    public List<Card> discardedCards;

    private FiveDrawGameState gameState;

    Player[] players;
    public Deck deck;

    private bool isDealing = false;

    public void Start()
    {
        InitGame(2, 500, 10);
    }

    public void InitGame(int numberOfPlayers, int buyIn, int ante)
    {
        if(numberOfPlayers <= 0 || numberOfPlayers > 5)
        {
            return;
        }

        players = new Player[numberOfPlayers];
        for(int i = 0; i < numberOfPlayers; i++)
        {
            HoleCards hole = Instantiate(holeCardPrefab, new Vector3(0,-3 + 6 * i), Quaternion.identity).GetComponent<HoleCards>();
            players[i] = new Player(i, buyIn, hole);
        }
        this.ante = ante;
        gameState = FiveDrawGameState.ROUND_ONE_DEAL;
    }

    public void UpdateState()
    {
        isDealing = false;
        StartCoroutine(players[0].hole.RevealCards());
        gameState++;
        Debug.Log(gameState);
    }

    public void Update()
    {
        switch (gameState)
        {
            case FiveDrawGameState.ROUND_ONE_DEAL:

                if (!isDealing)
                {
                    foreach(Player player in players)
                    {
                        pot += ante;
                        player.Bet(ante);
                    }

                    isDealing = true;
                    StartCoroutine(deck.Deal(players.Select(player => player.hole).ToArray(), 5, UpdateState));
                }

                break;
            
            case FiveDrawGameState.ROUND_ONE_BET:
                Debug.Log("Pot : " + pot);
                UpdateState();

                break;

            case FiveDrawGameState.DISCARD:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Card[] discardedCards0 = players[0].hole.DiscardCards(new int[]{0,4}, discardPile.transform.position);
                    Card[] discardedCards1 = players[1].hole.DiscardCards(new int[]{2}, discardPile.transform.position);
                    discardedCards.AddRange(discardedCards0);
                    discardedCards.AddRange(discardedCards1);
                    UpdateState();
                }

                break;

            case FiveDrawGameState.ROUND_TWO_DEAL:
                if (!isDealing)
                {
                    isDealing = true;
                    StartCoroutine(deck.UnevenDeal(
                        players.Where(player => player.playerState == PlayerState.PLAYING)
                        .Select(player => player.hole).ToArray(), 
                        new int[]{2,1}, UpdateState)
                    );
                }

                break;

            case FiveDrawGameState.ROUND_TWO_BET:
                UpdateState();

                break;

            case FiveDrawGameState.SHOWDOWN:
                Player[] playersStillIn = Player.GetNonFoldedPlayers(players);

                Hand[] hands = new Hand[playersStillIn.Length];

                for(int i = 0; i < playersStillIn.Length; i++)
                {
                    StartCoroutine(playersStillIn[i].hole.RevealCards());
                    hands[i] = new Hand(playersStillIn[i].hole.cards);
                    Debug.Log(hands[i].ranking);
                }

                HandRanking highestRanking = Hand.GetHighestHandRanking(hands);
                Hand[] handsOfHighestRanking = Hand.GetHandsOfSpecificRanking(hands, highestRanking);
                List<Hand> winningHands = Hand.GetBestHand(handsOfHighestRanking);

                List<Player> winningPlayers = new List<Player>();

                for(int i = 0; i < hands.Length; i++)
                {
                    if(winningHands.Contains(hands[i]))
                    {
                        playersStillIn[i].RecieveChips(pot / winningHands.Count);
                        winningPlayers.Add(playersStillIn[i]);
                        break;
                    }
                }

                if(winningHands.Count == 1)
                {
                    Player winningPlayer = winningPlayers[0];
                    Debug.Log("Player " + winningPlayer.seatNumber + " wins!");
                }

                foreach(Player player in players)
                {
                    Debug.Log("Player " + player.seatNumber + " Chips : " + player.chips);
                }

                UpdateState();

                break;
            
            default:
                break;
        } 
    }


    public enum FiveDrawGameState {
        RESET,
        ROUND_ONE_DEAL,
        ROUND_ONE_BET,
        DISCARD,
        ROUND_TWO_DEAL,
        ROUND_TWO_BET,
        SHOWDOWN,
        FINALE
    }

}
