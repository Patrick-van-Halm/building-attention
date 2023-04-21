using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections.Generic;

public class WaypointSharer : SingletonMonoBehaviour<WaypointSharer>
{
    [Tooltip("For debugging only. For testing with a local server.")]
    [SerializeField] private string ip = "iphere";
    [Tooltip("For debugging only. For testing with a local server.")]
    [SerializeField] private string port = "3000";

    [SerializeField] private GameObject mapWaypointPrefab;
    [SerializeField] private Transform worldMapTransform;
    [SerializeField] private RectTransform mapTransform;
    [SerializeField] private RectTransform waypointMenu;
    [SerializeField] private TextMeshProUGUI copiedText;

    private RectTransform currentWaypoint = null;


    ///<summary>Fires when a new path gets created.</summary>
    public UnityEvent<List<Vector3>> OnNavigationStart;

    private void Start()
    {
        Application.deepLinkActivated += OnDeepLinkActivated;

        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            OnDeepLinkActivated(Application.absoluteURL);
        }
    }

    public void WaypointSelected(RectTransform waypoint)
    {
        if (currentWaypoint == waypoint)
        {
            CloseMenu();
            currentWaypoint = null;
        }
        else
        {
            StopCoroutine("CopiedAnim");
            copiedText.color = new Color32(0, 0, 0, 0);

            Vector2 topOfWaypoint = waypoint.anchoredPosition + new Vector2(0, waypoint.rect.height);
            OpenMenu(topOfWaypoint);
            currentWaypoint = waypoint;
        }
    }

    public void CreateShareLink()
    {
        if (!currentWaypoint) return;

        string link = $"http://{ip}:{port}/?x={currentWaypoint.anchoredPosition.x}&y={currentWaypoint.anchoredPosition.y}";
        GUIUtility.systemCopyBuffer = link;

        StopCoroutine("CopiedAnim");
        StartCoroutine("CopiedAnim");
    }

    public void NavigateToWaypoint()
    {
        if (!currentWaypoint) return;

        //Vector3 worldPos = new Vector3(1f / mapTransform.sizeDelta.x * currentWaypoint.anchoredPosition.x * worldMapTransform.localScale.x, 1, 1f / mapTransform.sizeDelta.y * currentWaypoint.anchoredPosition.y * worldMapTransform.localScale.z);
        Waypoint waypoint = WaypointManager.Instance.GetWaypointByMapMarker(currentWaypoint.gameObject);
        List<Vector3> waypoints = PathfindingManager.Instance.StartNavigating(waypoint.WorldspaceWaypoint.transform.position);

        if (waypoints != null)
        {
            CloseMenu();
            OnNavigationStart?.Invoke(waypoints);
        }
    }

    private void OnDeepLinkActivated(string url)
    {
        string[] pos = url.Split("?")[1].Split("&");
        Vector2 waypointPos = new Vector2(float.Parse(pos[0]), float.Parse(pos[1]));

        GameObject wp = Instantiate(mapWaypointPrefab, mapTransform);
        wp.GetComponent<RectTransform>().anchoredPosition = waypointPos;
        WaypointManager.Instance.AddWaypoint(new() { MapWaypoint = wp });
    }

    #region Menu Handling
    private void OpenMenu(Vector2 point)
    {
        waypointMenu.anchoredPosition = point;

        StopCoroutine("MenuCloseAnim");
        StopCoroutine("MenuOpenAnim");
        StartCoroutine("MenuOpenAnim");
    }

    private void CloseMenu()
    {
        StopCoroutine("MenuOpenAnim");
        StopCoroutine("MenuCloseAnim");
        StartCoroutine("MenuCloseAnim");
    }

    private IEnumerator MenuOpenAnim()
    {
        waypointMenu.localScale = new Vector3(0, 0, 1);
        waypointMenu.gameObject.SetActive(true);

        for (float i = 0; i < 1f; i+=Time.deltaTime * 8f)
        {
            waypointMenu.localScale = new Vector3(i, i, 1);

            yield return null;
        }

        waypointMenu.localScale = new Vector3(1, 1, 1);
    }

    private IEnumerator MenuCloseAnim()
    {
        for (float i = 1f; i > 0f; i -= Time.deltaTime * 8f)
        {
            waypointMenu.localScale = new Vector3(i, i, 1);

            yield return null;
        }

        waypointMenu.localScale = new Vector3(0, 0, 1);
        waypointMenu.gameObject.SetActive(false);
    }

    private IEnumerator CopiedAnim()
    {
        for (float i = 355f; i > 0f; i-=Time.deltaTime*255f)
        {
            copiedText.color = new Color32(0, 0, 0, (byte)Mathf.Clamp(i, 0f, 255f));

            yield return null;
        }

        copiedText.color = new Color32(0, 0, 0, 0);
    }
    #endregion
}
