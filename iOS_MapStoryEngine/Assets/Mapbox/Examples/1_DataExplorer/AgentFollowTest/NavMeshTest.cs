using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Mapbox.Unity.MeshGeneration.Factories;

public class NavMeshTest : MonoBehaviour
{
	public NavMeshAgent agent;
    //public GameObject agent;

	public GameObject Target;

	[SerializeField]
    private GameObject Directions;

    //private bool myVar = false;

    [SerializeField]
    private GameObject GuidePlane;

    public bool setAgent = false;

    // Update is called once per frame
    void Update()
    {
    	
    	/*if(GameObject.Find("direction waypoint  entity") != null)
    	{
            Debug.Log(Directions.GetComponent<DirectionsWalking>()._waypoints.Length);
    		//agent.transform.position = Directions.GetComponent<DirectionsWalking>()._waypoints[1].position;
    	}*/
    	if(GameObject.Find("direction waypoint  entity") != null && GameObject.Find("direction waypoint  entity").GetComponent<NavMeshSurface>() == null && !GuidePlane.activeSelf)
    	//if(GameObject.Find("direction waypoint  entity") != null && myVar == false)
        {           
                    //myVar = true;
                    //Directions.GetComponent<DirectionsWalking>().myCount = false;
                    //playerMarker.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;

                    NavMeshSurface sc = GameObject.Find("direction waypoint  entity").AddComponent(typeof(NavMeshSurface)) as NavMeshSurface;
                    //NavMeshSurface sc = GameObject.Find("direction waypoint  entity").GetComponent<NavMeshSurface>() as NavMeshSurface;
                    sc.BuildNavMesh();

                    // Move to nearest part of surface
    		        NavMeshHit hit;
                    if(NavMesh.SamplePosition(agent.transform.position, out hit, 10.0f, NavMesh.AllAreas) && setAgent == false)
                    {
                        setAgent = true;
                        Vector3 result = hit.position;
                        result.y = 0;
                        agent.transform.position = result;
                    }

                    //plane.GetComponent<NavMeshSurface>().BuildNavMesh();

                    /* // Show mesh surface. Removed map and icon itself with mesh override. Need to add mesh filter and mesh renderer with material to work.
                    NavMeshTriangulation meshData = NavMesh.CalculateTriangulation();
                    Mesh mesh = new Mesh();
                    mesh.vertices = meshData.vertices;
                    mesh.triangles = meshData.indices;
                    GetComponent<MeshFilter>().mesh = mesh;*/

                    agent.SetDestination(Target.transform.position);



    	}
        else if(GameObject.Find("direction waypoint  entity") != null && GameObject.Find("direction waypoint  entity").GetComponent<NavMeshModifier>() == null && GuidePlane.activeSelf)
        {
            GameObject.Find("direction waypoint  entity").AddComponent<NavMeshModifier>().ignoreFromBuild = true;
        }
        // No directions guide for some reason
        //else
    	   //agent.SetDestination(Target.transform.position);

    }
}
