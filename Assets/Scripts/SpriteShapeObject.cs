using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteShapeObject : MonoBehaviour
{
    SpriteShapeController _spriteShapeController;
    Spline _spline;
    Vector3 _initialLeftSplinePosition;
    Vector3 _initialRightSplinePosition;
    float _landDistance;
    const int _leftSplineNode = 0;
    const int _rightSplineNode = 1;
    [Header("Distance")]
    [SerializeField] float _minDistance = 5f;
    [SerializeField] float _maxDistance = 10f;
    [Header("Height")]
    [SerializeField] float _minHeigh = 0.5f;
    [SerializeField] float _maxHeight = 1.5f;
    [Header("LeftTangent")]
    [SerializeField] float _minLeftTangentNoiseX = 0f;
    [SerializeField] float _maxLeftTangentNoiseX = 2f;
    [SerializeField] float _minLeftTangentNoiseY = 5f;
    [SerializeField] float _maxLeftTangentNoiseY = 2f;
    void Awake()
    {
        _spriteShapeController = GetComponent<SpriteShapeController>();
        _spline = _spriteShapeController.spline;
        _spline.RemovePointAt(3);
        _spline.RemovePointAt(0);
        _initialLeftSplinePosition = _spline.GetPosition(_leftSplineNode);
        _initialRightSplinePosition = _spline.GetPosition(_rightSplineNode);
    }
    void OnEnable()
    {
        _landDistance = Random.Range(_minDistance, _maxDistance);
        _spline.SetPosition(_leftSplineNode, new Vector3(-_landDistance, 0, 0));
        _spline.SetPosition(_rightSplineNode, new Vector3(_landDistance, 0, 0));
        _spline.SetLeftTangent(_rightSplineNode, new Vector3(-Random.Range(_minLeftTangentNoiseX, _landDistance / _maxLeftTangentNoiseX), Random.Range(_landDistance / _minLeftTangentNoiseY, _landDistance / _maxLeftTangentNoiseY), 0));
        _spline.SetRightTangent(_leftSplineNode, new Vector3(Random.Range(_minLeftTangentNoiseX, _landDistance / _maxLeftTangentNoiseX), Random.Range(_landDistance / _minLeftTangentNoiseY, _landDistance / _maxLeftTangentNoiseY), 0));
        _spline.SetHeight(_leftSplineNode, Random.Range(_minHeigh, _maxHeight));
        _spline.SetHeight(_rightSplineNode, Random.Range(_minHeigh, _maxHeight));
        transform.localScale = new Vector3(Random.Range(0, 2) == 0 ? -1 : 1, 1, 1);
        _spriteShapeController.RefreshSpriteShape();
    }
}
