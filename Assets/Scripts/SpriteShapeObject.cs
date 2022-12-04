using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] bool _isLeftLast;
    [SerializeField] bool _isRightLast;
    public bool IsLeftLast { get => _isLeftLast; set => _isLeftLast = value; }
    public bool IsRightLast { get => _isRightLast; set => _isRightLast = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _spriteShapeController = GetComponent<SpriteShapeController>();
        _spline = _spriteShapeController.spline;
        _spline.RemovePointAt(3);
        _spline.RemovePointAt(0);
        _initialLeftSplinePosition = _spline.GetPosition(_leftSplineNode);
        _initialRightSplinePosition = _spline.GetPosition(_rightSplineNode);
    }
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         _landDistance = Random.Range(5f,10f);
    //         _spline.SetPosition(_leftSplineNode, new Vector3(-_landDistance, 0, 0));
    //         _spline.SetPosition(_rightSplineNode, new Vector3(_landDistance, 0, 0));
    //         _spline.SetRightTangent(_leftSplineNode, new Vector3(Random.Range(0,_landDistance/2), Random.Range(-_landDistance/2,_landDistance/2), 0));
    //         _spline.SetLeftTangent(_rightSplineNode, new Vector3(-Random.Range(0,_landDistance/2), Random.Range(-_landDistance/2,_landDistance/2), 0));
    //         _spline.SetHeight(_leftSplineNode,Random.Range(0.1f,2f));
    //         _spline.SetHeight(_rightSplineNode,Random.Range(0.1f,2f));
    //         _spriteShapeController.RefreshSpriteShape();
    //     }
    // }
    void OnEnable()
    {
        _landDistance = Random.Range(5f, 10f);
        _spline.SetPosition(_leftSplineNode, new Vector3(-_landDistance, 0, 0));
        _spline.SetPosition(_rightSplineNode, new Vector3(_landDistance, 0, 0));
        _spline.SetRightTangent(_leftSplineNode, new Vector3(Random.Range(0, _landDistance / 2), Random.Range(0, _landDistance / 2), 0));
        _spline.SetLeftTangent(_rightSplineNode, new Vector3(-Random.Range(0, _landDistance / 2), Random.Range(0, _landDistance / 2), 0));
        _spline.SetHeight(_leftSplineNode, Random.Range(1f, 1.5f));
        _spline.SetHeight(_rightSplineNode, Random.Range(1f, 1.5f));
        transform.localScale = new Vector3(Random.Range(0, 2) == 0 ? -1 : 1, 1, 1);
        _spriteShapeController.RefreshSpriteShape();
    }
}
