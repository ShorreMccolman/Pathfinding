using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	public NavMeshNode CurrentNode;

	Queue<NavMeshNode> path = new Queue<NavMeshNode>();
	bool moving = false;

	void Update()
	{
		if (!moving)
			return;
		
		if(Vector3.Distance(transform.position,CurrentNode.transform.position) > 1f) {
			MoveToNode ();
		} else {
			ReachDestination ();
		}
	}

	void MoveToNode()
	{
		transform.position += (transform.position - CurrentNode.transform.position).normalized * Time.deltaTime;
	}

	void ReachDestination()
	{
		if (path.Count > 0)
			CurrentNode = path.Dequeue ();
		else
			moving = false;
	}

	void UpdatePath(NavMeshNode target)
	{
		moving = true;
		path.Clear ();


	}
}
