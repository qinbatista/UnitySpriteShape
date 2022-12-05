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
    [SerializeField] SOEnvObject _soEnvObject;

    public SOEnvObject SoEnvObject { get => _soEnvObject;}

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
        _landDistance = Random.Range(SoEnvObject.MinDistance, SoEnvObject.MaxDistance);
        _spline.SetPosition(_leftSplineNode, new Vector3(-_landDistance, 0, 0));
        _spline.SetPosition(_rightSplineNode, new Vector3(_landDistance, 0, 0));
        _spline.SetRightTangent(_leftSplineNode, new Vector3(Random.Range(SoEnvObject.MinTangentNoiseX,  SoEnvObject.MaxTangentNoiseX), Random.Range(  SoEnvObject.MinTangentNoiseY,  SoEnvObject.MaxTangentNoiseY), 0));
        _spline.SetLeftTangent(_rightSplineNode, new Vector3(-Random.Range(SoEnvObject.MinTangentNoiseX,  SoEnvObject.MaxTangentNoiseX), Random.Range( SoEnvObject.MinTangentNoiseY, SoEnvObject.MaxTangentNoiseY), 0));
        _spline.SetHeight(_leftSplineNode, Random.Range(SoEnvObject.MinHeigh, SoEnvObject.MaxHeight));
        _spline.SetHeight(_rightSplineNode, Random.Range(SoEnvObject.MinHeigh, SoEnvObject.MaxHeight));
        transform.localScale = new Vector3(Random.Range(0, 2) == 0 ? -1 : 1, 1, 1);
        _spriteShapeController.RefreshSpriteShape();
        _spriteShapeController.spriteShape = SoEnvObject.Sprites[Random.Range(0, SoEnvObject.Sprites.Count)];
    }
}