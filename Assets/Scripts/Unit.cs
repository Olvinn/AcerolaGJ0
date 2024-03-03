using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rigidbody;
    private Vector3 _cachedMovDir;

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void Move(Vector3 dir)
    {
        _agent.Move(_cachedMovDir * Time.deltaTime);
        _cachedMovDir = dir;
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
        _rigidbody = GetComponent<Rigidbody>();
    }
}
