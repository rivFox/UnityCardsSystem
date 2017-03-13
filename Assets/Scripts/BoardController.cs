using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour {

    public GameObject card;
    public GameObject[] stacks;

    private GameObject[] cards;

    int maxNumberOfCards = 100;

    float thicknessOfCard = 0.005f;
    int pointerToStack = 1;
    int pointerToCard;

    int couter = 0;

    // Use this for initialization
    void Start () {
        CreateDeck();
        pointerToCard = maxNumberOfCards-1;
    }
    
    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("space")  && (couter < maxNumberOfCards))
        {
            MoveCard(pointerToCard, pointerToStack);
            NextStackAndCard();
            couter++;
        }
    }
    
    void CreateDeck()
    {
        cards = new GameObject[maxNumberOfCards];
        Vector3 transformNewCard = new Vector3(0f, thicknessOfCard, 0f);

        for (int i = 0; i < maxNumberOfCards; i++)
        {
            card.GetComponent<CardController>().id = i + 1;
            cards[i] = Instantiate(card, stacks[0].transform.position + i * transformNewCard, Quaternion.Euler(0f, 0f, 180f));
        }
    }

    void MoveCard(int cardId, int stackId)
    {
        Vector3 addTransform = new Vector3(0f, stacks[stackId].GetComponent<StackController>().GetNumberOfCards() * thicknessOfCard, 0f);
        cards[cardId].GetComponent<CardController>().MoveTo(stacks[stackId].transform.position, addTransform);
        stacks[stackId].GetComponent<StackController>().AddCard();
    }

    void NextStackAndCard()
    {
        if (pointerToStack == stacks.Length-1)
        {
            pointerToStack = 1;
        }
        else
        {
            pointerToStack++;
        }
        if (pointerToCard > 0)
        {
            pointerToCard--;
        }
    }
}
