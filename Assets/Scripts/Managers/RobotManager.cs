using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : SingletonMonoBehaviour<RobotManager>
{
    [Tooltip("Robot to be enabled and disabled.")] [SerializeField] Robot _robot;

    [Tooltip("The AR camera of the player, used as starting point to calculate the distance to the robot.")]
    [SerializeField] private Transform _player;

    private List<Vector3> _waypoints = new();
    private int _waypointIndex;

    // Start is called before the first frame update
    void Start()
    {
       WaypointSharer _waypointSharer = FindObjectOfType<WaypointSharer>();
        _waypointSharer.OnNavigationStart.AddListener(ActivateRobot);

        PathfindingManager _pathfindingManager = FindObjectOfType<PathfindingManager>();
        _pathfindingManager.OnWaypointsChanged.AddListener(ChangeRoute);

        _pathfindingManager.OnNavigationCancelled.AddListener(DeactivateRobot);

        _robot.OnWaypointReached.AddListener(ReachedWaypoint);
    }

    private void ActivateRobot(List<Vector3> waypoints)
    {
        _waypoints = waypoints;
        _waypointIndex = 0;

        _robot.gameObject.SetActive(true);
        _robot.Appear(_waypoints[_waypointIndex]);
        _robot.MoveToPoint(_waypoints[_waypointIndex]);
    }

    private void ChangeRoute(List<Vector3> waypoints)
    {
        _waypoints = waypoints;
        _waypointIndex = 0;

        _robot.Appear(_waypoints[_waypointIndex]);
        _robot.MoveToPoint(_waypoints[_waypointIndex]);
    }

    private void DeactivateRobot()
    {
        _robot.Disappear();
        //_robot.gameObject.SetActive(false);
    }

    private void ReachedWaypoint()
    {
        _waypointIndex++;

        if (_waypointIndex == _waypoints.Count)
        {
            DeactivateRobot();
            return;
        }
        _robot.MoveToPoint(_waypoints[_waypointIndex]);
    }

    public float GetDistanceFromPlayerToRobot()
    {
        return Vector3.Distance(_robot.transform.position, _player.position);
    }

    public Vector3 GetPlayerPosition()
    {
        return _player.position;
    }
}
