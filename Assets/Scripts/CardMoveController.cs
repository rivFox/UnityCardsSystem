using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODO:
 * Dodać możliwosć zmiany prędkości w samej corutynie
 * INNE BŁĘDY: Przy DD podczas podnoszenia karty jeśli ruszamy nią, to są dziwne skoki.
 */

public class CardMoveController : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(0f, 0f, 0f);
    public Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);

    private bool _isMoving;
    private bool _isAutoMoving = false;

    private bool _dragging = false;
    private bool _cardActivated = false;

    private float _distance;

    public AnimationCurve curveMovementSpeed = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public float movementSpeed = 2f;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    public AnimationCurve curveRotationSpeed = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    private Quaternion _startRotation;
    private Quaternion _endRotation;

    private float _timeStartedMoving;
    private float _timeTakenDuringMove;

    private Vector3 oldCursorPosition;

    //Zmienne testowe
    public bool debugDDMode = true;
    private bool _debugDD = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartAutoMoving();
        }
        AutoMoving();
        DDMoving();
    }

    void OnMouseDown()
    {
        if (!_isAutoMoving)
        {
            _distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            _dragging = true;
            StartActive();
            //Cursor.visible = false;
        }
    }

    void OnMouseUp()
    {
        _dragging = false;
        _cardActivated = false;
        StopActive();
        Cursor.visible = true;
    }

    void DDMoving()
    {
        if (_dragging)
        {
            if(!_cardActivated)
            {
                StartCoroutine(Moving());
                if (!_isMoving)
                {
                    _cardActivated = true;
                }   
            }
            if(debugDDMode || (!debugDDMode && _cardActivated))
            {
                _distance = Vector3.Distance(new Vector3(transform.position.x, 1f, transform.position.z), Camera.main.transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 rayPoint = ray.GetPoint(_distance);
                transform.position = new Vector3(rayPoint.x, transform.position.y, rayPoint.z);
            }
        }
        else
        {
            if(_isMoving)
            {
                StartCoroutine(Moving());
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }

    void AutoMoving()
    {
        if (_isAutoMoving)
        {
            Debug.Log(_cardActivated);
            if (!_cardActivated)
            {
                StartCoroutine(Moving());
                if (!_isMoving)
                {
                    _cardActivated = true;
                    StartAutoMoving();
                }
            }
            else
            {
                StartCoroutine(Moving());
                if (!_isMoving)
                {
                    _isAutoMoving = false;
                    _cardActivated = false;
                }
            }
        }
    }

    void StartActive()
    {
        oldCursorPosition = Input.mousePosition;
        StartMoving(new Vector3(transform.position.x, 1f, transform.position.z));
    }

    void StopActive()
    {
        StartMoving(new Vector3(transform.position.x, 0f, transform.position.z));
    }

    void StartAutoMoving()
    {
        if (_isAutoMoving)
        { 
            if (_cardActivated){
                StartMoving(targetPosition);
            }
            else
            {
                StartActive();
            }
        }
        else
        {
            _isAutoMoving = true;
            _cardActivated = false;
            StartAutoMoving();
        }
    }

    void StartMoving(Vector3 target)
    {
        _isMoving = true;
        _timeStartedMoving = Time.time;

        _startPosition = transform.position;
        _endPosition = target;

        _startRotation = transform.rotation;
        _endRotation = targetRotation;

        _timeTakenDuringMove = Vector3.Magnitude(_startPosition - _endPosition);
    }

    IEnumerator Moving()
    {
        if (_isMoving)
        {
            float timeSinceStarted = Time.time - _timeStartedMoving;
            float percentageComplete = timeSinceStarted / _timeTakenDuringMove;
            transform.position = Vector3.Lerp(_startPosition, _endPosition, curveMovementSpeed.Evaluate(percentageComplete));
            transform.rotation = Quaternion.Lerp(_startRotation, _endRotation, curveRotationSpeed.Evaluate(percentageComplete));
            if (percentageComplete >= 1.0f)
            {
                _isMoving = false;
            }
        }
        yield return null;
    }
}
