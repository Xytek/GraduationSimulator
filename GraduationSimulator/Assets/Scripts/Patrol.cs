using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] Transform[] _checkpoints = default;    // An array holding the checkpoints the teacher will go to
    private int _nextCheckpoint;                            // Holds the next checkpoint the teacher should go to
    private NavMeshAgent _agent;                            // Used for AI commands and initiated in Start()

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Ensures you only change destination when you're not already on a path, and you're close to your goal
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            GoToNextCheckpoint();
        }
    }

    private void GoToNextCheckpoint()
    {
        // If no checkpoints have been added to the array it will exit the function
        if (_checkpoints.Length == 0)
        {
            Debug.Log("No checkpoints");
            return;
        }

        // Set the agent destination to the next checkpoint in the array
        _agent.destination = _checkpoints[_nextCheckpoint].position;

        // After the last checkpoint in the array comes the first one, so make sure they're close together.
        _nextCheckpoint = (_nextCheckpoint + 1) % _checkpoints.Length;
    }
}
