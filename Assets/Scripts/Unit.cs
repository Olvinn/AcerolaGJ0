using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;

    public void Move(Vector3 dir)
    {
        if (dir.sqrMagnitude == 0)
            return;
        _agent.Move(dir * Time.deltaTime);
    }

    public void Look(Vector3 dir)
    {
        if (dir.sqrMagnitude == 0)
            return;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir),
            Time.deltaTime * GameConfigsContainer.instance.config.playerAngularSpeed);
    }

    public void Teleport(Vector3 pos)
    {
        _agent.Warp(pos);
    }

    private void Reset()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
}
