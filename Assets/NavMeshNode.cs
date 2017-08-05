using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NavMeshNode : MonoBehaviour {

	public List<NavMeshNode> neighbors = new List<NavMeshNode>();

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, 0.5f);

		foreach(NavMeshNode node in neighbors) {
			Gizmos.DrawLine (transform.position, node.transform.position);
		}
	}
}
