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
        public Action onReloadComplete;
        public event Action<ExposedTrigger> onTriggerEnter, onTriggerExit;

        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LayerMask _shootingLayer;
        [SerializeField] private AnimationController _animator;
        [SerializeField] private VFX _shootVFX;
        private Vector3 _cachedMovDir, _currentVelocity;
        private bool _cachedAim;
        private float _speed, _aimSpeed, _angularSpeed;
        private float _lastShotTime, _shotTimer;

        private void Start()
        {
            _animator.onReloadComplete += onReloadComplete;
        }

        private void Update()
        {
            if (!_animator)
                return;
            Vector3 locMov;
            if (_agent.hasPath)
                locMov = transform.worldToLocalMatrix * _agent.velocity;
            else
                locMov = transform.worldToLocalMatrix * _currentVelocity;
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
            _animator = GetComponentInChildren<AnimationController>();
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
            _currentVelocity = Vector3.MoveTowards(_currentVelocity, dir * (_cachedAim ? _aimSpeed : _speed), Time.deltaTime * _agent.acceleration);
            _agent.Move(_currentVelocity * Time.deltaTime);
            if (_currentVelocity.sqrMagnitude != 0)
                _cachedMovDir = _currentVelocity;
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
                if (_cachedMovDir != transform.rotation.eulerAngles)
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

        public bool Shoot(Weapon weapon, UnitModel shooter)
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

            var angle = weapon.accuracy * .5f;
            Ray ray = new Ray(_shootVFX.transform.position, 
                Quaternion.Euler(Random.Range(-angle,angle),Random.Range(-angle,angle), 0) * transform.forward);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, GameConfigsAndSettings.Instance.config.shootingDistance, _shootingLayer);
            Array.Sort(hits,
                (a, b) =>
                    (a.distance.CompareTo(b.distance)));
            foreach (var hit in hits)
            {
                VFX impact = VFXController.Instance.GetEffect(EffectType.BulletImpact);
                impact.transform.position = hit.point;
                impact.transform.rotation = Quaternion.LookRotation(ray.direction);
                Unit unit;
                if (hit.collider.gameObject.TryGetComponent(out unit))
                    unit.TakeDamage(new Damage() { value = weapon.damage, from = shooter});
                else
                    break;
            }

            if (hits.Length > 0)
                VFXController.Instance.GetEffect(EffectType.BulletTrail).StartEffect(ray.origin, hits[0].point);
            else
                VFXController.Instance.GetEffect(EffectType.BulletTrail).StartEffect(ray.origin, 
                    ray.origin + ray.direction * GameConfigsAndSettings.Instance.config.shootingDistance);

            return true;
        }

        public void TakeDamage(Damage damage)
        {
            onDamage?.Invoke(damage);
        }

        public void Reload()
        {
            _animator.SetTrigger("Reloading");
        }
    }
}
