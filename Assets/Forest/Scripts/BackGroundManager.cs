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
    int _frondGroundsCount => _soEnvObject.ObjectCount;
    float _frondGroundsGap => _soEnvObject.ObjectGap;
    void Awake()
    {
        _isActiveNativeArray = new NativeArray<bool>(_frondGroundsCount, Allocator.Persistent);
        _transformAccessArray = new TransformAccessArray(_frondGroundsCount);
        for (int i = 0; i < _frondGroundsCount; i++)
        {
            _frontGround = Instantiate(_environmentObject, new Vector3(_playerTransform.transform.position.x - (_frondGroundsCount - 1) * _frondGroundsGap / 2f + i * _frondGroundsGap, UnityEngine.Random.Range(_soEnvObject.MinY, _soEnvObject.MaxY), 0), Quaternion.identity, transform);
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
            _frondGroundsGapJob = _frondGroundsGap,
            _frondGroundsCountJob = _frondGroundsCount,
            _playerSpeedJob = _playerSpeed,
            _parallaxOffsetJob = _soEnvObject.ParallaxOffset
        };
        _jobHandle = _bgFrontJob.Schedule(_transformAccessArray);
        _jobHandle.Complete();
        for (int i = 0; i < _frondGroundsCount; i++)
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
    public void Execute(int index, TransformAccess transform)
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
        //parallax offset
        transform.position = new Vector3(transform.position.x + (-_playerSpeedJob.x * _parallaxOffsetJob), transform.position.y + -(_playerSpeedJob.y * _parallaxOffsetJob), transform.position.z);
    }
}