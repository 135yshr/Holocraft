using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpatialRecognition : MonoBehaviour
{
    [Tooltip("When checked, the SurfaceObserver will stop running after a specified amount of time.")]
    public bool limitScanningByTime = true;

    [Tooltip("How much time (in seconds) that the SurfaceObserver will run after being started; used when 'Limit Scanning By Time' is checked.")]
    public float scanTime = 30.0f;

    [Tooltip("Material to use when rendering Spatial Mapping meshes while the observer is running.")]
    public Material defaultMaterial;

    [Tooltip("Minimum number of floor planes required in order to exit scanning/processing mode.")]
    public uint minimumFloors = 1;

    /// <summary>
    /// Indicates if processing of the surface meshes is complete.
    /// </summary>
    private bool meshesProcessed;

    // Use this for initialization
    void Start()
    {
        SpatialMappingManager.Instance.SetSurfaceMaterial(defaultMaterial);
        SurfaceMeshesToPlanes.Instance.MakePlanesComplete += SurfaceMeshesToPlanes_MakePlanesComplete;
    }

    // Update is called once per frame
    void Update()
    {
        if (!meshesProcessed && limitScanningByTime)
        {
            if (limitScanningByTime == false || (scanTime < (Time.time - SpatialMappingManager.Instance.StartTime)))
            {
                Debug.Log((Time.time - SpatialMappingManager.Instance.StartTime));
                if (SpatialMappingManager.Instance.IsObserverRunning())
                {
                    SpatialMappingManager.Instance.StopObserver();
                }
                CreatePlanes();
                meshesProcessed = true;
            }
        }
    }

    /// <summary>
    /// Creates planes from the spatial mapping surfaces.
    /// </summary>
    private void CreatePlanes()
    {
        // Generate planes based on the spatial map.
        SurfaceMeshesToPlanes surfaceToPlanes = SurfaceMeshesToPlanes.Instance;
        if (surfaceToPlanes != null && surfaceToPlanes.enabled)
        {
            surfaceToPlanes.MakePlanes();
        }
    }
    private void SurfaceMeshesToPlanes_MakePlanesComplete(object source, EventArgs args)
    {
        var floors = SurfaceMeshesToPlanes.Instance.GetActivePlanes(PlaneTypes.Floor);
        if (floors.Count <= minimumFloors)
        {
            SpatialMappingManager.Instance.StartObserver();
            meshesProcessed = false;
            return;
        }
        RemoveVertices(SurfaceMeshesToPlanes.Instance.ActivePlanes);
        foreach (var floor in floors)
        {
            Debug.Log("Name: " + floor.name);
            Debug.Log("position: " + floor.transform.position);
            Debug.Log("rotation: " + floor.transform.rotation);
            Debug.Log("scale: " + floor.transform.localScale);
        }
    }

    private void RemoveVertices(IEnumerable<GameObject> boundingObjects)
    {
        RemoveSurfaceVertices removeVerts = RemoveSurfaceVertices.Instance;
        if (removeVerts != null && removeVerts.enabled)
        {
            removeVerts.RemoveSurfaceVerticesWithinBounds(boundingObjects);
        }
    }

    /// <summary>
    /// Called when the GameObject is unloaded.
    /// </summary>
    private void OnDestroy()
    {
        if (SurfaceMeshesToPlanes.Instance != null)
        {
            SurfaceMeshesToPlanes.Instance.MakePlanesComplete -= SurfaceMeshesToPlanes_MakePlanesComplete;
        }
    }
}
