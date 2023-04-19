using System.Collections;
using UnityEngine;

public class ProceduralAnimator : MonoBehaviour
{
    [System.Serializable]
    class ProceduralLimb
    {
        public Transform IKTarget;
        public Vector3 defaultPosition;
        public Vector3 lastPosition;
        public bool moving;
    }

    [SerializeField] private Transform[] _limbTargets; // Only use these to assign the location of the limbs when the game starts.
    [SerializeField] private ProceduralLimb[] _limbs;

    [SerializeField] private float _stepSize = 1;
    [SerializeField] private float _stepSpeed = 0.25f;
    [SerializeField] private float _stepHeight = 0.25f;
    [SerializeField] private float _stepPrediction = 1;

    [SerializeField] private LayerMask _groundLayerMask = default;
    [SerializeField] private float _raycastUpOffset = 2;

    [SerializeField] private float _resetTime = 0.2f;

    private Vector3 _lastBodyPosition;
    private Vector3 _velocity;
    private float _time = 0;
    private bool _hasMoved = false;
    private int _movingLegAmount;

    void Start()
    {
        _limbs = new ProceduralLimb[_limbTargets.Length];

        for (int i = 0; i < _limbTargets.Length; i++)
        {
            _limbs[i] = new ProceduralLimb()
            {
                IKTarget = _limbTargets[i],
                defaultPosition = _limbTargets[i].localPosition,
                lastPosition = _limbTargets[i].position,
                moving = false
            };
        }

        _lastBodyPosition = transform.position;
    }

    private void Update()
    {
        // Resets the legs after a certain amount of time.
        if (_velocity == Vector3.zero && _hasMoved == true)
        {
            _time += Time.deltaTime;

            if (_time >= _resetTime)
            {
               StartCoroutine(ResetLegPos());
                _time = 0;
            }
        }

        else
            _time = 0;
    }

    private void FixedUpdate()
    {
        // Calculate the velocity of the moving ob
        _velocity = transform.position - _lastBodyPosition;
        _lastBodyPosition = transform.position;

        Vector3[] desiredPositions = new Vector3[_limbs.Length];

        /* We are checking if the players body position is far enough
         from the limb to then move it. We also handle logic to make
         sure we dont lift all legs of the ground simultaneously */
        for (int i = 0; i < _limbs.Length; ++i)
        {
            if (_limbs[i].moving == true) continue; // if limb is moving skip it

            desiredPositions[i] = transform.TransformPoint(_limbs[i].defaultPosition);
            float distance = (desiredPositions[i] + _velocity - _limbs[i].lastPosition).magnitude;

            // Start taking a step
            if (distance > _stepSize && _movingLegAmount <= 2)
                StartCoroutine(MoveLeg(i, GroundRaycast(desiredPositions[i], transform.up, _raycastUpOffset)));

            // Keeps the limb in the old position of the limbs isnt moving
            if (_limbs[i].moving == false)
                _limbs[i].IKTarget.position = _limbs[i].lastPosition;
        }
    }

    /// <summary>
    /// Calculates the new position of the leg and will then move it towards this new location.
    /// </summary>
    /// <param name="limbToMove"></param>
    /// <param name="targetStartPos"></param>
    /// <returns></returns>
    private IEnumerator MoveLeg(int limbToMove, Vector3 targetStartPos)
    {
        _limbs[limbToMove].moving = true;
        Vector3 direction = _velocity.normalized;
        _hasMoved = true;
        _movingLegAmount++;

        Vector3 startPos = _limbs[limbToMove].IKTarget.position;
        Vector3 targetPoint = targetStartPos + (direction * _stepPrediction);

        float t = 0f;

        while (t < 1f)
        {
            // Calculate the vertical offset based on _time and step height
            float yOffset = Mathf.Sin(t * Mathf.PI) * _stepHeight;

            // Interpolate horizontally towards the target position
            t += Time.deltaTime / _stepSpeed;
            _limbs[limbToMove].IKTarget.position = Vector3.Lerp(startPos, targetPoint, t);

            // Apply the vertical offset
            _limbs[limbToMove].IKTarget.position += Vector3.up * yOffset;

            yield return null;
        }

        _limbs[limbToMove].IKTarget.position = targetPoint;
        _limbs[limbToMove].lastPosition = _limbs[limbToMove].IKTarget.position;
        _limbs[limbToMove].moving = false;
        _movingLegAmount--;
    }

    private Vector3 GroundRaycast(Vector3 position, Vector3 upDirection, float startingOffset)
    {
        Vector3 raycastOrigin = position + (startingOffset * upDirection);
        Ray ray = new Ray(raycastOrigin, -upDirection);

        if (Physics.Raycast(ray, out RaycastHit hit, startingOffset, _groundLayerMask))
            return hit.point;

        return position; // Resturns position if no ground is found
    }

    private IEnumerator ResetLegPos()
    {
        for (int i = 0; i < _limbs.Length; i++)
        {
            StartCoroutine(MoveLeg(i, transform.TransformPoint(_limbs[i].defaultPosition)));
            yield return new WaitForSeconds(0.05f);
            _hasMoved = false;
        }
    }
}

