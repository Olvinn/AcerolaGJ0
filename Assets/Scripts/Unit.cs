using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Vector3 _cachedMovDir;

    public void Move(Vector3 dir)
    {
        if (dir.sqrMagnitude == 0)
            return;
        _cachedMovDir = dir;
        _agent.Move(dir * Time.deltaTime);
    }

    public void Look(Vector3 dir)
    {
        if (dir.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir),
                Time.deltaTime * GameConfigsContainer.instance.config.playerAngularSpeed);
        }
        else if (_cachedMovDir.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_cachedMovDir),
                Time.deltaTime * GameConfigsContainer.instance.config.playerAngularSpeed);
        }
    }

    public void Teleport(Vector3 pos, Quaternion rot)
    {
        _agent.Warp(pos);
        transform.rotation = rot;
    }

    private void Reset()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
}
