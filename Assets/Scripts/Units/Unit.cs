using System;
using Controllers;
using Triggers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Units
{
    [RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Rigidbody))]
    public class Unit : MonoBehaviour
    {
        public Action<ContactPoint[]> onCollide;
        public Action<Damage> onDamage;
        public event Action<ExposedTrigger> onTriggerEnter, onTriggerExit;

        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LayerMask _shootingLayer;
        [SerializeField] private Animator _animator;
        private Vector3 _cachedMovDir;

        private void Update()
        {
            if (!_animator)
                return;
            _animator.SetFloat("Speed", _cachedMovDir.magnitude * _agent.speed);
            _animator.SetFloat("X", (transform.worldToLocalMatrix * (_cachedMovDir * _agent.speed)).x);
            _animator.SetFloat("Y", (transform.worldToLocalMatrix * (_cachedMovDir * _agent.speed)).z);
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void Reset()
        {
            _agent = GetComponent<NavMeshAgent>();
            _rigidbody = GetComponent<Rigidbody>();
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            onCollide?.Invoke(collision.contacts);
        }

        public void Move(Vector3 dir)
        {
            _cachedMovDir = Vector3.Lerp(_cachedMovDir, dir, Time.deltaTime * _agent.acceleration);
            _agent.Move(_cachedMovDir * (Time.deltaTime * _agent.speed));
        }

        public void MoveTo(Vector3 pos)
        {
            _agent.SetDestination(pos);
        }

        public void Look(Vector3 dir)
        {
            if (dir.sqrMagnitude != 0)
            {
                if (_animator)
                    _animator.SetBool("Aiming", true);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir),
                    Time.deltaTime * GameConfigsAndSettings.instance.config.playerAngularSpeed);
            }
            else if (_cachedMovDir.sqrMagnitude != 0)
            {
                if (_animator)
                    _animator.SetBool("False", true);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_cachedMovDir),
                    Time.deltaTime * GameConfigsAndSettings.instance.config.playerAngularSpeed);
            }
        }

        public void TriggerInter(ExposedTrigger trigger)
        {
            onTriggerEnter?.Invoke(trigger);
        }

        public void TriggerExit(ExposedTrigger trigger)
        {
            onTriggerExit?.Invoke(trigger);
        }

        public void Teleport(Vector3 pos, Quaternion rot)
        {
            _agent.Warp(pos);
            transform.rotation = rot;
        }

        public void Shoot(Damage damage)
        {
            Ray ray = new Ray(transform.position + Vector3.up, 
                Quaternion.Euler(Random.Range(-5f,5f),Random.Range(-5f,5f), 0) * transform.forward);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, GameConfigsAndSettings.instance.config.shootingDistance, _shootingLayer);
            Array.Sort(hits,
                (a, b) =>
                    (a.distance.CompareTo(b.distance)));
            foreach (var hit in hits)
            {
                VFX impact = VFXController.instance.GetEffect(EffectType.BulletImpact);
                impact.transform.position = hit.point;
                Unit unit;
                if (hit.collider.gameObject.TryGetComponent(out unit))
                    unit.TakeDamage(damage);
                else
                    break;
            }
        }

        public void TakeDamage(Damage damage)
        {
            onDamage?.Invoke(damage);
        }
    }
}
