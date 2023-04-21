using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TempScript : MonoBehaviour
{
    [SerializeField] ARRaycastManager raycastManager;

    [SerializeField] GameObject showProjectObject;
    [SerializeField] GameObject scheduleObject;
    [SerializeField] GameObject designSpaceUI;

    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    GameObject spawnedObject;

    bool ableToShowProjects = false;
    bool ableToShowSchedule = false;

    void Start()
    {
        spawnedObject = null;
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        if (raycastManager.Raycast(Input.GetTouch(0).position, hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                SpawnPrefab(hits[0].pose.position);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
            {
                spawnedObject.transform.position = hits[0].pose.position;
            }
        }
    }
    private void SpawnPrefab(Vector3 spawnPosition)
    {
        if (ableToShowProjects)
        {
            Destroy(spawnedObject);
            spawnedObject = Instantiate(showProjectObject, spawnPosition, Quaternion.identity);
            SetShowProjects();
        }
        else if (ableToShowSchedule)
        {
            Destroy(spawnedObject);
            spawnedObject = Instantiate(scheduleObject, spawnPosition, Quaternion.identity);
            SetShowSchedule();
        }
        else
        {
            Destroy(spawnedObject);
        }
    }

    /// <summary>
    /// Enables users to tap in AR to spawn the show project prefab
    /// </summary>
    public void SetShowProjects()
    {
        if (!ableToShowSchedule)
        {
            ableToShowProjects = !ableToShowProjects;
            designSpaceUI.SetActive(ableToShowProjects);
        }
    }

    /// <summary>
    /// Enables users to tap in AR to spawn the room schedule
    /// </summary>
    public void SetShowSchedule() 
    { 
        if (!ableToShowProjects) ableToShowSchedule = !ableToShowSchedule;
    }
}
