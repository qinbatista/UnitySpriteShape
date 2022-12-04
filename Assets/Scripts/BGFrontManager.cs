using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class BGFrontManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _frondGround;
    [SerializeField] Transform _player;
    List<GameObject> _frondGroundsList = new List<GameObject>();
    GameObject _frontGround;
    BGFrontJob _bgFrontJob;
    JobHandle _jobHandle;
    NativeArray<bool> _isActiveNativeArray;
    TransformAccessArray _transformAccessArray;
    const int _frondGroundsCount = 11;
    const int _frondGroundsGap =7;
    void Awake()
    {
        _isActiveNativeArray = new NativeArray<bool>(_frondGroundsCount, Allocator.Persistent);
        _transformAccessArray = new TransformAccessArray(_frondGroundsCount);
        for (int i = 0; i < _frondGroundsCount; i++)
        {
            _frontGround = Instantiate(_frondGround, new Vector3(_player.transform.position.x - (_frondGroundsCount - 1) * _frondGroundsGap / 2 + i * _frondGroundsGap, UnityEngine.Random.Range(-6f,-7f), 0), Quaternion.identity, transform);
            _frondGroundsList.Add(_frontGround);
            _transformAccessArray.Add(_frontGround.transform);
        }
    }
    void Update()
    {
        _bgFrontJob = new BGFrontJob
        {
            _playerPositionJob = _player.position,
            _isActiveNativeArrayJob = _isActiveNativeArray,
            _frondGroundsGapJob = _frondGroundsGap,
            _frondGroundsCountJob = _frondGroundsCount
        };
        _jobHandle = _bgFrontJob.Schedule(_transformAccessArray);
        _jobHandle.Complete();
        for (int i = 0; i < _frondGroundsCount; i++)
        {
            _frondGroundsList[i].SetActive(_isActiveNativeArray[i]);
        }
    }
    void OnDestroy()
    {
        _isActiveNativeArray.Dispose();
        _transformAccessArray.Dispose();
    }
    [BurstCompile]
    public struct BGFrontJob : IJobParallelForTransform
    {
        [ReadOnly] public Vector3 _playerPositionJob;
        public NativeArray<bool> _isActiveNativeArrayJob;
        [ReadOnly] public float _frondGroundsGapJob;
        [ReadOnly] public float _frondGroundsCountJob;
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
        }
    }
}
