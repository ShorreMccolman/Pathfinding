using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	public NavMeshNode CurrentNode;
	public NavMeshNode TargetNode;

	Stack<NavMeshNode> path = new Stack<NavMeshNode>();
	bool moving = false;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space)) {
			UpdatePath ();
		}

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
		transform.position += (CurrentNode.transform.position - transform.position).normalized * 3f * Time.deltaTime;
	}

	void ReachDestination()
	{
		if (path.Count > 0)
			CurrentNode = path.Pop ();
		else
			moving = false;
	}

	void UpdatePath()
	{
		if (TargetNode == null)
			return;

		Queue<NavMeshNode> frontier = new Queue<NavMeshNode> ();
		frontier.Enqueue (CurrentNode);

		Dictionary<NavMeshNode, NavMeshNode> visitedDict = new Dictionary<NavMeshNode, NavMeshNode> ();
		visitedDict.Add (CurrentNode, null);

		bool found = false;

		while(frontier.Count > 0 && !found) {
			NavMeshNode current = frontier.Dequeue ();
			foreach(NavMeshNode neighbor in current.neighbors) {
				if(!visitedDict.ContainsKey(neighbor)) {
					frontier.Enqueue (neighbor);
					visitedDict.Add (neighbor, current);
				}

				if(neighbor == TargetNode) {
					found = true;
				}
			}
		}

		if(found) {
			path.Clear ();
			NavMeshNode next = TargetNode;
			while(next != null) {
				path.Push (next);
				next = visitedDict [next];
			}
			moving = true;
		}

	}
}
