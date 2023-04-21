using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Robot : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private float _speed = .5f;
    private Vector3 _waypoint;

    private float _maxDistanceFromPlayer = 3f;
    private float _minDistanceFromPlayer = 2f;

    private float _marginToDestination = 1f;

    private RobotState _state;
    public enum RobotState
    {
        Appearing,
        Disappearing,
        Walking,
        Waiting,
        Idle
    }

    public UnityEvent OnWaypointReached;

    private void Update()
    {
        float distanceFromPlayer = RobotManager.Instance.GetDistanceFromPlayerToRobot();
        float distanceFromTarget = Vector3.Distance(transform.position, _waypoint);
        if (_state == RobotState.Walking)
        {
            RotateToPoint(_waypoint);

            float m = _speed / (_waypoint - transform.position).magnitude;
            transform.position = Vector3.Lerp(transform.position, _waypoint, m * Time.deltaTime);

            if (distanceFromPlayer >= _maxDistanceFromPlayer)
            {
                _state = RobotState.Waiting;
                _animator.SetTrigger("Wait");
            }
            if (distanceFromTarget <= _marginToDestination)
            {
                _state = RobotState.Idle;
                OnWaypointReached?.Invoke();
            }
        }
        else if (_state == RobotState.Waiting)
        {
            Vector3 playerPosition = RobotManager.Instance.GetPlayerPosition();
            RotateToPoint(playerPosition);

            if (distanceFromPlayer <= _minDistanceFromPlayer)
            {
                _state = RobotState.Walking;
                _animator.SetTrigger("Walk");
            }
        }
    }

    public void Appear(Vector3 startPosition)
    {
        _state = RobotState.Appearing;
        gameObject.transform.position = startPosition;

        _animator.SetTrigger("Appear");
    }

    public void Disappear()
    {
        _state = RobotState.Disappearing;
        _animator.SetTrigger("Disappear");
    }

    public void MoveToPoint(Vector3 position)
    {
        _waypoint = position;

        _state = RobotState.Walking;
        _animator.SetTrigger("Walk");
    }

    private void RotateToPoint(Vector3 position)
    {
        transform.LookAt(position);
    }
}
