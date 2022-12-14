using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class BackGroundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _environmentObject;
    [SerializeField] Transform _playerTransform;
    Vector3 _playerPositionPreviews;
    List<GameObject> _frondGroundsList = new List<GameObject>();
    GameObject _frontGround;
    BGFrontJob _bgFrontJob;
    JobHandle _jobHandle;
    NativeArray<bool> _isActiveNativeArray;
    TransformAccessArray _transformAccessArray;
    Vector2 _playerSpeed;
    SOEnvObject _soEnvObject => _environmentObject.GetComponent<SpriteShapeObject>().SoEnvObject;
    int _groundsCount => _soEnvObject.ObjectCount;
    float _groundsGap => _soEnvObject.ObjectGap;
    const int _horizontalIndex = 0;
    const int _verticalIndex = 1;
    int _vertical_Layer = 0;
    int _horizontal_Layer = 0;
    void Awake()
    {
        _isActiveNativeArray = new NativeArray<bool>(_groundsCount, Allocator.Persistent);
        _transformAccessArray = new TransformAccessArray(_groundsCount);
        for (int i = 0; i < _groundsCount; i++)
        {
            if ((int)_soEnvObject.MapDirection == _horizontalIndex)
            {
                _frontGround = Instantiate(_environmentObject,
                new Vector3(
                        _playerTransform.transform.position.x - (_groundsCount - 1) * _groundsGap / 2f + i * _groundsGap,
                        UnityEngine.Random.Range(_soEnvObject.PositionY.minValue, _soEnvObject.PositionY.maxValue),
                        0
                        ),
                        Quaternion.identity, transform);
            }
            else if ((int)_soEnvObject.MapDirection == _verticalIndex)
            {
                if (i % _soEnvObject.DensityVertical == 0) _vertical_Layer++;
                if ((i % _soEnvObject.DensityVertical) != 0)
                    _horizontal_Layer++;
                else
                    _horizontal_Layer = 0;
                _frontGround = Instantiate(_environmentObject,
                new Vector3(
                        _playerTransform.transform.position.x - _groundsGap * _soEnvObject.DensityVertical / 2f + _groundsGap * (_horizontal_Layer+1) + UnityEngine.Random.Range(_soEnvObject.PositionX.minValue, _soEnvObject.PositionX.maxValue),
                        _playerTransform.transform.position.y - _groundsGap*_vertical_Layer - UnityEngine.Random.Range(_soEnvObject.PositionY.minValue, _soEnvObject.PositionY.maxValue),
                        0
                        ),
                        Quaternion.identity, transform);
            }
            _frondGroundsList.Add(_frontGround);
            _transformAccessArray.Add(_frontGround.transform);
        }
        _playerPositionPreviews = _playerTransform.position;
    }
    void Update()
    {
        _playerSpeed = _playerTransform.position - _playerPositionPreviews;
        _bgFrontJob = new BGFrontJob
        {
            _playerPositionJob = _playerTransform.position,
            _isActiveNativeArrayJob = _isActiveNativeArray,
            _frondGroundsGapJob = _groundsGap,
            _frondGroundsCountJob = _groundsCount,
            _playerSpeedJob = _playerSpeed,
            _parallaxOffsetJob = _soEnvObject.ParallaxOffset,
            _isHorizontalJob = (int)_soEnvObject.MapDirection == _horizontalIndex ? true : false
        };
        _jobHandle = _bgFrontJob.Schedule(_transformAccessArray);
        _jobHandle.Complete();
        for (int i = 0; i < _groundsCount; i++)
        {
            _frondGroundsList[i].SetActive(_isActiveNativeArray[i]);
        }
        _playerPositionPreviews = _playerTransform.position;
    }
    void OnDestroy()
    {
        _isActiveNativeArray.Dispose();
        _transformAccessArray.Dispose();
    }

}
[BurstCompile]
public struct BGFrontJob : IJobParallelForTransform
{
    [ReadOnly] public Vector3 _playerPositionJob;
    public NativeArray<bool> _isActiveNativeArrayJob;
    [ReadOnly] public float _frondGroundsGapJob;
    [ReadOnly] public float _frondGroundsCountJob;
    [ReadOnly] public Vector3 _playerSpeedJob;
    [ReadOnly] public float _parallaxOffsetJob;
    [ReadOnly] public bool _isHorizontalJob;
    public void Execute(int index, TransformAccess transform)
    {
        if (_isHorizontalJob)
        {
            if (Mathf.Abs(_playerPositionJob.x - transform.position.x) > _frondGroundsGapJob * 4)
                _isActiveNativeArrayJob[index] = false;//left 2 grounds, right 2 grounds are visiable
            else
                _isActiveNativeArrayJob[index] = true;

            if (Mathf.Abs(_playerPositionJob.x - transform.position.x) > _frondGroundsGapJob * (_frondGroundsCountJob - 1) / 2 + _frondGroundsGapJob)// farther than half of all grounds+1 ground
            {
                if (_playerPositionJob.x > transform.position.x)
                {
                    transform.position = new Vector3(transform.position.x + _frondGroundsGapJob * _frondGroundsCountJob, transform.position.y, transform.position.z);
                    // Debug.Log($"left move to right = {transform.position}");
                }
                else
                {
                    transform.position = new Vector3(transform.position.x - _frondGroundsGapJob * _frondGroundsCountJob, transform.position.y, transform.position.z);
                    // Debug.Log($"right move to left = {transform.position}");
                }
            }
        }
        else
        {
            _isActiveNativeArrayJob[index] = true;
        }
        //parallax offset
        transform.position = new Vector3(transform.position.x + (-_playerSpeedJob.x * _parallaxOffsetJob), transform.position.y + -(_playerSpeedJob.y * _parallaxOffsetJob), transform.position.z);
    }
}