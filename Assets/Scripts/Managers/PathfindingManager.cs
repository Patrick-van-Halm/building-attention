using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PathfindingManager : SingletonMonoBehaviour<PathfindingManager>
{
    [Tooltip("The AR camera of the player, used as starting point when creating a path.")]
    [SerializeField] private Transform player;
    [Tooltip("The world map transform, scale is used to convert the created path in world space to a path line on the map.")]
    [SerializeField] private Transform worldMapTransform;
    [SerializeField] private RectTransform mapTransform;

    [Space(12)]
    [Min(0.1f)]
    [Tooltip("The minimum distance between each waypoint.")]
    [SerializeField] private float waypointSpacing = 2f;
    [Tooltip("The object that will be visible on the floor and shows the path to the player.")]
    [SerializeField] private GameObject arrowPrefab;
    [Min(1)]
    [Tooltip("The total arrows visible in front of the player.")]
    [SerializeField] private int maxVisibleArrows = 3;
    [Tooltip("The distance at which a waypoint is deemed reached, and the max allowed distance from the player to the path before it gets regenerated.")]
    [SerializeField] private float reachDistance = 2f;

    [Space(12)]
    [SerializeField] private float updateInterval = 0.2f;
    private float currentInterval = 0f;

    private NavMeshPath currentPath;
    private UILineRenderer mapPathLine;

    private bool isNavigating = false;
    private Vector3 currentTarget;
    private List<Vector3> currentWaypoints;
    private int currentWaypointIndex = 0;

    ///<summary>Fires when the path changes.</summary>
    public UnityEvent<List<Vector3>> OnWaypointsChanged;

    ///<summary>Fires when the player transform reaches the path target.</summary>
    public UnityEvent OnDestinationReached;

    ///<summary>Fires when the navigation gets cancelled.</summary>
    public UnityEvent OnNavigationCancelled;

    private void Start()
    {
        currentPath = new NavMeshPath();
        mapPathLine = FindObjectOfType<UILineRenderer>();

        if (mapPathLine == null) Debug.LogWarning("No UILineRenderer component present in the scene.");
    }

    private void Update()
    {
        currentInterval -= Time.deltaTime;

        if (currentInterval <= 0f)
        {
            currentInterval = updateInterval;

            if (isNavigating && currentWaypointIndex < currentWaypoints.Count - 1)
            {
                float pathDistance = GetDistanceFromPath(new Vector2(player.position.x, player.position.z), currentWaypointIndex);

                // If the player has strayed too far from the path
                if (pathDistance > reachDistance)
                {
                    isNavigating = false;
                    // Try creating a new path towards the target
                    List<Vector3> waypoints = StartNavigating(currentTarget);

                    if (waypoints == null)
                    {
                        StopNavigating();
                        OnNavigationCancelled?.Invoke();
                    }
                    else
                    {
                        OnWaypointsChanged?.Invoke(waypoints);
                    }
                }
                // Player is still near the path
                else
                {
                    float waypointDistance = (new Vector3(currentWaypoints[currentWaypointIndex + 1].x, 0f, currentWaypoints[currentWaypointIndex + 1].z) - new Vector3(player.position.x, 0f, player.position.z)).magnitude;

                    // If the player has reached the next waypoint
                    if (waypointDistance < reachDistance)
                    {
                        currentWaypointIndex++;

                        // If the player has reached the target
                        if (currentWaypointIndex >= currentWaypoints.Count - 1)
                        {
                            OnDestinationReached.Invoke();
                            StopNavigating();
                            Debug.Log("Reached destination!");
                        }
                        // Player has not reached the target
                        else
                        {
                            // Update visuals
                            DrawArrows();
                            UpdateMapPath();
                        }
                    }
                }
            }
        }
    }

    ///<summary>Starts navigating the player towards the target. Returns true if a path was created, else returns false.</summary>
    public List<Vector3> StartNavigating(Vector3 target)
    {
        if (isNavigating)
        {
            if (target == currentTarget)
            {
                Debug.LogWarning("Already navigating to target.");
                return null;
            }
            else
            {
                StopNavigating();
            }
        }

        currentWaypoints = GeneratePath(player.position, target);

        if (currentWaypoints != null)
        {
            currentTarget = target;
            currentWaypointIndex = 0;

            DrawArrows();
            UpdateMapPath();

            isNavigating = true;
            return currentWaypoints;
        }
        else
        {
            Debug.LogWarning("Could not create a path.");
            return null;
        }
    }

    ///<summary>Stops navigating the player.</summary>
    public void StopNavigating()
    {
        isNavigating = false;
        currentWaypoints = null;

        DrawArrows();
        mapPathLine.SetPositions(new());
    }

    ///<summary>Generates a path starting from source and going to target, returning the path waypoins. Returns null if no path could be created.</summary>
    private List<Vector3> GeneratePath(Vector3 source, Vector3 target)
    {
        if (NavMesh.CalculatePath(source, target, NavMesh.AllAreas, currentPath))
        {
            List<Vector3> waypoints = new List<Vector3>() { currentPath.corners[0] };

            for (int i = 0; i < currentPath.corners.Length - 1; i++)
            {
                int count = Mathf.FloorToInt((currentPath.corners[i + 1] - currentPath.corners[i]).magnitude / waypointSpacing);

                for (int j = count - 1; j >= 0; j--)
                {
                    waypoints.Add(Vector3.Lerp(currentPath.corners[i + 1], currentPath.corners[i], 1f / count * j));
                }
            }

            return currentPath.corners.ToList();
        }
        else
        {
            return null;
        }
    }

    ///<summary>Returns the distance from source to the current path using the current node index. Returns -1 if no current path exists or if the index is invalid.</summary>
    private float GetDistanceFromPath(Vector2 source, int index)
    {
        if (currentPath.status != NavMeshPathStatus.PathInvalid && index >= 0 && index < currentWaypoints.Count - 1)
        {
            Vector2 start = new Vector2(currentWaypoints[index].x, currentWaypoints[index].z);
            Vector2 end = new Vector2(currentWaypoints[index + 1].x, currentWaypoints[index + 1].z);

            Vector2 heading = end - start;
            float magnitudeMax = heading.magnitude;
            heading.Normalize();

            Vector2 lhs = source - start;
            float dotP = Vector2.Dot(lhs, heading);
            dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);

            Vector2 closest = start + heading * dotP;
            return (closest - source).magnitude;
        }
        else
        {
            return -1f;
        }
    }

    private void DrawArrows()
    {
        // Destroy previous arrows
        foreach (Transform arrow in transform)
        {
            Destroy(arrow.gameObject);
        }

        if (currentWaypoints == null) return;

        // Create new arrows
        for (int i = 1; i < maxVisibleArrows + 1; i++)
        {
            if (currentWaypointIndex + i + 1 > currentWaypoints.Count - 1) break;

            GameObject arrow = Instantiate(arrowPrefab, currentWaypoints[currentWaypointIndex + i], Quaternion.identity, transform);
            arrow.transform.LookAt(currentWaypoints[currentWaypointIndex + i + 1], Vector3.up);
        }
    }

    private void UpdateMapPath()
    {
        List<Vector2> positions = new List<Vector2>();

        for (int i = currentWaypointIndex; i < currentWaypoints.Count; i++)
        {
            Vector3 localPos = worldMapTransform.InverseTransformPoint(currentWaypoints[i]);
            Vector2 mapPos = PositionConverter.WorldToMap(localPos);
            //float x = 0.1f * Mathf.Abs(currentWaypoints[i].x) * Mathf.Sign(currentWaypoints[i].x);
            //float y = 0.1f * Mathf.Abs(currentWaypoints[i].z) * Mathf.Sign(currentWaypoints[i].z);
            positions.Add(new(-mapPos.x, -mapPos.y));
        }

        mapPathLine.SetPositions(positions);
    }
}
