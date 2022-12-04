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
    const int _frondGroundsCount = 11;
    BGFrontJob _bgFrontJob;
    JobHandle _jobHandle;
    NativeArray<bool> _isActiveNativeArray;
    TransformAccessArray _transformAccessArray;
    void Awake()
    {
        _isActiveNativeArray = new NativeArray<bool>(_frondGroundsCount, Allocator.Persistent);
        _transformAccessArray = new TransformAccessArray(_frondGroundsCount);
        for (int i = 0; i < _frondGroundsCount; i++)
        {
            _frontGround = Instantiate(_frondGround, new Vector3(_player.transform.position.x - 25 + i * 5, 0, 0), Quaternion.identity, transform);
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
            // _transformAccessArrayJob = _transformAccessArray,
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
        public void Execute(int index, TransformAccess transform)
        {
            if (Mathf.Abs(_playerPositionJob.x - transform.position.x) > 10f) _isActiveNativeArrayJob[index] = false;
            else _isActiveNativeArrayJob[index] = true;
            if (Mathf.Abs(_playerPositionJob.x - transform.position.x) >= 30 && Mathf.Abs(_playerPositionJob.x - transform.position.x) < 35)
            {
                if (_playerPositionJob.x > transform.position.x)
                {
                    transform.position = new Vector3(transform.position.x + 55, 0, 0);
                    Debug.Log($"left move to right = {transform.position}");
                }
                else
                {
                    transform.position = new Vector3(transform.position.x - 55, 0, 0);
                    Debug.Log($"right move to left = {transform.position}");
                }
            }
        }
    }
}
