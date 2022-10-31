using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnEnded;

    public int turnNumber;
    public bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one 'Turn System' active! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        OnTurnEnded?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn; 
    }
}
