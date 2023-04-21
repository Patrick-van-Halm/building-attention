using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Move")]
    private float _randomXPosition;
    private float _randomYPosition;
    private float _randomZPosition;
    private Vector3 _destination;
    private float _speed;

    [Header("Fade Out")]
    private readonly float _fadePerSecond = .03f;
    [SerializeField] private Color alphaColor;

    private void Start()
    {
        _randomXPosition = Random.Range(-.5f, .5f);
        _randomYPosition = Random.Range(-.5f, .5f);
        _randomZPosition = Random.Range(-.5f, .5f);

        _destination = new Vector3(_randomXPosition, _randomYPosition, _randomZPosition);

        _speed = Random.Range(.01f, .1f);
    }

    private void Update()
    {
        MoveToDestination();
        CheckDestination();
        FadeOut();
    }

    private void MoveToDestination()
    {
        transform.position = Vector3.Lerp(transform.position, _destination, Time.deltaTime * _speed);
    }
    private void CheckDestination()
    {
        if (Vector3.Distance(transform.position, _destination) <= .5f && gameObject.GetComponentInChildren<MeshRenderer>().material.color.a <= 0.2)
        {
            Destroy(gameObject);
        }
    }
    private void FadeOut()
    {
        gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(gameObject.GetComponentInChildren<MeshRenderer>().material.color, alphaColor, _fadePerSecond * Time.deltaTime);
    }
}
