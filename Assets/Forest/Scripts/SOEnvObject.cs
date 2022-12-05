using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(menuName = "ScriptableObject/Environment/Object")]
public class SOEnvObject : ScriptableObject
{
    [Header("ObjectDistance")]
    [SerializeField] int _objectCount = 5;
    [SerializeField] float _ObjectGap = 10f;
    [Header("ShapeDistance")]
    [SerializeField] float _minDistance = 5f;
    [SerializeField] float _maxDistance = 10f;
    [Header("Position")]
    [SerializeField] float _minY = -6f;
    [SerializeField] float _maxY = -7f;
    [Header("Height")]
    [SerializeField] float _minHeigh = 0.5f;
    [SerializeField] float _maxHeight = 1.5f;
    [Header("Tangent")]
    [SerializeField] float _minTangentNoiseX = 0f;
    [SerializeField] float _maxTangentNoiseX = 2f;
    [SerializeField] float _minTangentNoiseY = 0f;
    [SerializeField] float _maxTangentNoiseY = 2f;
    [SerializeField] List<SpriteShape> _sprites = new List<SpriteShape>();
    [SerializeField] float _parallaxOffset = 1f;

    public float MinDistance { get => _minDistance; }
    public float MaxDistance { get => _maxDistance; }
    public float MinHeigh { get => _minHeigh; }
    public float MaxHeight { get => _maxHeight; }
    public float MinTangentNoiseX { get => _minTangentNoiseX; }
    public float MaxTangentNoiseX { get => _maxTangentNoiseX; }
    public float MinTangentNoiseY { get => _minTangentNoiseY; }
    public float MaxTangentNoiseY { get => _maxTangentNoiseY; }
    public List<SpriteShape> Sprites { get => _sprites; }
    public float ParallaxOffset { get => _parallaxOffset; }
    public float MinY { get => _minY;}
    public float MaxY { get => _maxY;}
    public int ObjectCount { get => _objectCount;}
    public float ObjectGap { get => _ObjectGap;}
}
