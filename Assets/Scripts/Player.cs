using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player
{
    public int seatNumber;
    public int chips;
    public HoleCards hole;
    public PlayerState playerState;

    public Player(int seatNumber, int chips, HoleCards hole)
    {
        this.seatNumber = seatNumber;
        this.chips = chips;
        this.hole = hole;
    }

    public void Bet(int amount)
    {
        chips -= amount;
    }

    public void RecieveChips(int amount)
    {
        chips += amount;
    }

    public void Fold()
    {
        playerState = PlayerState.FOLDED;
    }

    public static Player[] GetNonFoldedPlayers(Player[] players)
    {
        List<Player> stillInPlayers = new List<Player>();
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].playerState != PlayerState.FOLDED)
            {
                stillInPlayers.Add(players[i]);
            }
        }

        return stillInPlayers.ToArray();
    }
}

public enum PlayerState
{
    PLAYING,
    FOLDED
}

public enum PlayerMove
{
    CHECK_CALL,
    BET_RAISE,
    FOLD
}