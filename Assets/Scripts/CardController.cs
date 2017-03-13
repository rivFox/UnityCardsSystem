using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour {

    public int id;
    bool canMove = false;
    Vector3 targetToMove, addTransformToMove;

    // Use this for initialization
    void Start() {

        //Nadanie karcie numeru
        GetComponentInChildren<Text>().text += id.ToString();
    }

    private void Update()
    {
        if(canMove)
        {
            StartCoroutine(StartMove(targetToMove, addTransformToMove ));
        }
    }

    public void MoveTo(Vector3 target, Vector3 addTransform)
    {
        targetToMove = target;
        addTransformToMove = addTransform;
        canMove = true;
    }

    IEnumerator StartMove(Vector3 target, Vector3 addTransform)
    {
        transform.position = Vector3.Slerp(transform.position, target + addTransform, 0.05f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 0.05f);

        yield return null;
    }
}
