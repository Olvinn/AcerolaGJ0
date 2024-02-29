using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move(Vector3 dir)
    {
        if (dir.sqrMagnitude == 0)
            return;
        _agent.Move(dir * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _agent.angularSpeed);
    }
}
