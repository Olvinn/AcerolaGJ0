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
        Vector3 mov = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _agent.Move(mov);
    }
}
