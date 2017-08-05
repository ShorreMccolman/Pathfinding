using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NavMeshNode : MonoBehaviour {

	public List<NavMeshNode> neighbors = new List<NavMeshNode>();

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, 0.5f);
		Gizmos.color = Color.yellow;
		foreach(NavMeshNode node in neighbors) {

			Vector3 cross1 = Vector3.Cross ((node.transform.position - transform.position).normalized, Vector3.up);

			Vector3 pos1 = transform.position + cross1 * 0.6f;
			Vector3 pos2 = node.transform.position + cross1 * 0.6f;

			Gizmos.DrawLine (pos1, pos2);
			//Gizmos.DrawLine (transform.position, node.transform.position);
		}
	}
}
