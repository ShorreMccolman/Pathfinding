using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NavMesh : MonoBehaviour {
	[MenuItem("NavMesh/UpdateMesh")]
	public static void UpdateMesh()
	{
		NavMesh mesh = Selection.activeTransform.GetComponent<NavMesh> ();
		if(mesh != null) {
			GameObject[] nodes = GameObject.FindGameObjectsWithTag ("Node");
			mesh.nodes.Clear ();
			foreach(GameObject obj in nodes) {
				NavMeshNode node = obj.GetComponent<NavMeshNode> ();
				mesh.nodes.Add (node);
				node.neighbors.Clear ();

				foreach(GameObject other in nodes) {
					if(other != obj) {
						RaycastHit hit;
						Ray newRay = new Ray (obj.transform.position, (other.transform.position - obj.transform.position).normalized);
						Physics.Raycast (newRay, out hit, Vector3.Distance(obj.transform.position,other.transform.position) + 0.1f);
						if(hit.collider == null) {
							node.neighbors.Add (other.GetComponent<NavMeshNode> ());
						} else {
							Debug.LogError (obj.name + " hit a wall on its way to " + other.name);
						}
					}
				}

			}
		}
	}

	private static NavMesh instance;
	public static NavMesh Instance
	{
		get{ return instance; }
	}
	void Awake()
	{instance = this;}

	public List<NavMeshNode> nodes = new List<NavMeshNode>();


}
