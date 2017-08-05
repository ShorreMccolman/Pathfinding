using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PriorityNode {
	public NavMeshNode node;
	public float priority;

	public PriorityNode(NavMeshNode node, float priority)
	{
		this.node = node;
		this.priority = priority;
	}
}

public class Agent : MonoBehaviour {

	NavMeshNode CurrentNode;
	NavMeshNode TargetNode;

	Stack<NavMeshNode> path = new Stack<NavMeshNode>();
	bool moving = false;

	void Update()
	{
		if(Input.GetMouseButtonDown(1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(Physics.Raycast(ray, out hit, 100f)) {
				if(hit.collider.tag == "Navigate") {
					CurrentNode = NavMesh.Instance.FindBestNode (transform.position);
					TargetNode = NavMesh.Instance.FindBestNode (hit.point);
					UpdatePath ();
				}
			}
		}


		if (!moving)
			return;
		
		if(Vector3.Distance(transform.position,CurrentNode.transform.position) > 0.1f) {
			MoveToNode ();
		} else {
			ReachDestination ();
		}
	}

	void MoveToNode()
	{
		transform.position += (CurrentNode.transform.position - transform.position).normalized * 5f * Time.deltaTime;
	}

	void ReachDestination()
	{
		if (path.Count > 0)
			CurrentNode = path.Pop ();
		else
			moving = false;
	}

	float CalculateCost(NavMeshNode current, NavMeshNode node) {
		return Vector3.Distance (current.transform.position, node.transform.position);
	}

	float DistToGoal(NavMeshNode node) {
		return Vector3.Distance (node.transform.position, TargetNode.transform.position);
	}

	void UpdatePath()
	{
		if (TargetNode == null)
			return;

		List<PriorityNode> frontier = new List<PriorityNode> ();
		frontier.Add (new PriorityNode(CurrentNode,0f));

		Dictionary<NavMeshNode, NavMeshNode> visitedDict = new Dictionary<NavMeshNode, NavMeshNode> ();
		visitedDict.Add (CurrentNode, null);

		Dictionary<NavMeshNode, float> costDict = new Dictionary<NavMeshNode, float> ();
		costDict.Add (CurrentNode, 0);

		bool found = false;

		while(frontier.Count > 0 && !found) {
			NavMeshNode current = frontier [0].node;
			frontier.RemoveAt (0);

			foreach(NavMeshNode neighbor in current.neighbors) {
				float newCost = costDict [current] + CalculateCost(current,neighbor);

				if(!costDict.ContainsKey(neighbor) || newCost < costDict[neighbor]) {
					costDict [neighbor] = newCost;

					frontier.Add (new PriorityNode(neighbor,newCost + DistToGoal(neighbor)));
					frontier.Sort (delegate(PriorityNode x, PriorityNode y) {
						return x.priority.CompareTo(y.priority);
					});

					if (visitedDict.ContainsKey (neighbor))
						visitedDict [neighbor] = current;
					else
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
