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
        [SerializeField] private AnimationController _animator;
        [SerializeField] private VFX _shootVFX;
        private Vector3 _cachedMovDir;
        private bool _cachedAim;
        private float _speed, _aimSpeed, _angularSpeed;
        private float _lastShotTime, _shotTimer;

        private void Update()
        {
            if (!_animator)
                return;
            Vector3 locMov;
            if (_agent.hasPath)
                locMov = transform.worldToLocalMatrix * _agent.velocity;
            else
                locMov = transform.worldToLocalMatrix * _cachedMovDir;
            _animator.SetFloat("Speed", locMov.magnitude);
            _animator.SetFloat("X", locMov.x);
            _animator.SetFloat("Y", locMov.z);
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

        public void SetUpMobility(float speed, float aimSpeed, float angularSpeed)
        {
            _agent.speed = _speed = speed;
            _agent.angularSpeed = _angularSpeed = angularSpeed;
            _aimSpeed = aimSpeed;
        }

        public void SetUpFirepower(float rateOfFire)
        {
            _lastShotTime = Time.time;
            _shotTimer = 1f / rateOfFire;
        }

        public void Move(Vector3 dir)
        {
            _cachedMovDir = Vector3.Lerp(_cachedMovDir, dir * (_cachedAim ? _aimSpeed : _speed), Time.deltaTime * _agent.acceleration);
            _agent.Move(_cachedMovDir * Time.deltaTime);
        }

        public void MoveTo(Vector3 pos)
        {
            _agent.speed = _cachedAim ? _aimSpeed : _speed;
            _agent.SetDestination(pos);
        }

        public void Look(Vector3 dir)
        {
            if (_cachedAim)
            {
                if (dir.sqrMagnitude == 0)
                    return;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir),
                    Time.deltaTime * _angularSpeed);
            }
            else
            {
                if (_cachedMovDir.sqrMagnitude == 0)
                    return;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_cachedMovDir),
                    Time.deltaTime * _angularSpeed);
            }
        }

        public void Aim(bool value)
        {
            if (_animator)
                _animator.SetBool("Aiming", value);
            _cachedAim = value;
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

        public bool Shoot(Damage damage)
        {
            if (!_cachedAim)
                return false;

            if (Time.time - _lastShotTime <= _shotTimer)
                return false;

            _lastShotTime = Time.time;

            if (_shootVFX)
                _shootVFX.StartEffect();
            if (_animator)
                    _animator.SetTrigger("Shoot");
            
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
                impact.transform.rotation = Quaternion.LookRotation(ray.direction);
                Unit unit;
                if (hit.collider.gameObject.TryGetComponent(out unit))
                    unit.TakeDamage(damage);
                else
                    break;
            }

            if (hits.Length > 0)
                VFXController.instance.GetEffect(EffectType.BulletTrail).StartEffect(new []{ray.origin, hits[0].point});
            else
                VFXController.instance.GetEffect(EffectType.BulletTrail).StartEffect(new []{ray.origin, 
                    ray.origin + ray.direction * GameConfigsAndSettings.instance.config.shootingDistance});

            return true;
        }

        public void TakeDamage(Damage damage)
        {
            onDamage?.Invoke(damage);
        }
    }
}
