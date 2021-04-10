using SafeSurfing.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SafeSurfing
{
    public class LaserController : MonoBehaviour
    {
        public GameObject LaserStartPrefab;
        public GameObject[] LaserBodyPrefabs;

        private GameObject _LaserStart;
        private List<GameObject> _LaserBodies;
        private bool _Shooting;

        private float _YMax;
        private int _LastBodyIndex = 0;
        public void ShootLaser(float time = 5f)
        {
            _LastBodyIndex = 0;
            StartCoroutine(Util.TimedAction(() => _Shooting = true, 
                () => 
                { 
                    _Shooting = false;
                    Destroy(_LaserStart);
                    _LaserStart = null;
                    _LaserBodies.ForEach(x => Destroy(x));
                    _LaserBodies.Clear();
                }, 
                time));
        }

        private void Start()
        {
            _LaserBodies = new List<GameObject>();
            var bounds = GameObject.FindGameObjectWithTag("Bounds");
            var collider = bounds.GetComponent<EdgeCollider2D>();

            _YMax = collider.points.Max(point => point.y);
        }

        void Update()
        {
            if (!_Shooting)
                return;

            Vector3 initialPosition = transform.position;
            // Create the laser start from the prefab
            if (_LaserStart == null)
            {
                _LaserStart = Instantiate(LaserStartPrefab, initialPosition, transform.rotation, transform);
            }

            // Define an "infinite" size, not too big but enough to go off screen
            var maxFill = _YMax - transform.parent.localPosition.y;

            var currentFill = 0f;
            var laserStartSpriteRender = _LaserStart.GetComponent<SpriteRenderer>();
            var laserStartHeight = laserStartSpriteRender.bounds.size.y;

            currentFill += laserStartHeight;

            //Check pre-existing fill
            if (_LaserBodies.Count > 0)
                _LaserBodies.ForEach(laserBody =>
                {
                    var laserBodySpriteRender = laserBody.GetComponent<SpriteRenderer>();
                    currentFill += laserBodySpriteRender.bounds.size.y;
                });

            while (currentFill > maxFill)
            {
                var destroyBody = _LaserBodies[_LaserBodies.Count - 1];

                var laserBodySpriteRender = destroyBody.GetComponent<SpriteRenderer>();
                currentFill -= laserBodySpriteRender.bounds.size.y;

                _LaserBodies.Remove(destroyBody);
                Destroy(destroyBody);
                _LastBodyIndex = _LastBodyIndex == 0 ? LaserBodyPrefabs.Length - 1 : _LastBodyIndex - 1;
            }

            while (currentFill < maxFill)
            {
                var laserBody = Instantiate(LaserBodyPrefabs[_LastBodyIndex],
                    initialPosition + new Vector3(0, currentFill, 0),
                    transform.rotation,
                    transform);

                _LaserBodies.Add(laserBody);

                var laserBodySpriteRender = laserBody.GetComponent<SpriteRenderer>();
                currentFill += laserBodySpriteRender.bounds.size.y;

                _LastBodyIndex = (_LastBodyIndex + 1) % LaserBodyPrefabs.Length;
            }
        }
    }
}