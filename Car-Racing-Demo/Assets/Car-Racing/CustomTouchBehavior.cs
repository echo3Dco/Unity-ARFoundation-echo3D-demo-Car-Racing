using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/*
 * CustomTouchBehavior adds custom functionality when a plane is touched
 */
[RequireComponent(typeof(ARRaycastManager))]
public class CustomTouchBehavior : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    
    // The prefab to instantiate on touch.
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }
    
    // The object instantiated as a result of a successful raycast intersection with a plane.
    public GameObject spawnedObject { get; private set; }


    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }


    /*
     * Called every frame
     */
    void Update()
    {
        /*
         * Once an object has been spawned, do nothing.
         * This allows for only one spawn because there should only be one racetrack+racecar in the scene
         */
        if (spawnedObject != null)
            return;

        if (Input.touchCount <= 0) //If no screen touches
            return;

        else if (Input.touchCount >= 1) //If at least one screen touch
        {
            //If there is a touch on a plane...
            if (m_RaycastManager.Raycast(Input.GetTouch(0).position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hitPose = s_Hits[0].pose;

                //Instantiate the prefab
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

                //Disable plane manager and point cloud manager because after instantiating one instance, we will not allow any more.
                this.gameObject.GetComponent<ARPlaneManager>().enabled = false;
                this.gameObject.GetComponent<ARPointCloudManager>().enabled = false;
            }
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARRaycastManager m_RaycastManager;
}