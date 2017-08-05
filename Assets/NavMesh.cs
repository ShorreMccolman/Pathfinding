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
					if(other != obj && Vector3.Distance(node.transform.position,other.transform.position) < 6.0f) {
						bool obstructed = false;

						RaycastHit hit;
						Ray newRay = new Ray (obj.transform.position, (other.transform.position - obj.transform.position).normalized);
						Physics.Raycast (newRay, out hit, Vector3.Distance(obj.transform.position,other.transform.position) + 0.1f);
						if(hit.collider != null) {
							Debug.LogWarning (obj.name + " hit a wall on its way to " + other.name);
							obstructed = true;
						}

						Vector3 cross1 = Vector3.Cross ((other.transform.position - obj.transform.position).normalized, Vector3.up);
						Vector3 cross2 = Vector3.Cross ((obj.transform.position - other.transform.position).normalized, Vector3.up);
						Vector3[] crosses = new Vector3[]{ cross1, cross2 };
						foreach(Vector3 cross in crosses) {
							Vector3 pos1 = obj.transform.position + cross * 0.6f;
							Vector3 pos2 = other.transform.position + cross * 0.6f;

							RaycastHit hit2;
							Ray newRay2 = new Ray (pos1, (pos2 - pos1).normalized);
							Physics.Raycast (newRay2, out hit2, Vector3.Distance(pos1,pos2) + 0.1f);
							if(hit2.collider != null) {
								Debug.LogWarning (obj.name + " hit a wall on its way to " + other.name);
								obstructed = true;
							}
						}

						if(!obstructed) {
							node.neighbors.Add (other.GetComponent<NavMeshNode> ());
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

	public NavMeshNode FindBestNode(Vector3 point) {
		float bestDistance = 1000f;
		NavMeshNode best = nodes[0];
		foreach(NavMeshNode node in nodes) {
			float distance = Vector3.Distance(node.transform.position,point);
			if(distance < bestDistance) {
				best = node;
				bestDistance = distance;
			}
		}

		return best;
	}
}
