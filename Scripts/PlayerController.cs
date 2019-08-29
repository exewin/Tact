using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	NavMeshAgent navMeshAgent;
	void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.updateRotation = false;
	}
	
	void LateUpdate()
	{
		if (navMeshAgent.velocity.sqrMagnitude > Mathf.Epsilon) //rotate immediately
		{
			transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
		}
	}
}
