using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour {

    private int cards = 0;

    public int GetNumberOfCards()
    {
        return cards;
    }

    public void AddCard()
    {
        cards++;
    }
}
