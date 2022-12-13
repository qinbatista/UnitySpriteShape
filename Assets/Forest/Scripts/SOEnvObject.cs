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
    [SerializeField][MinMaxRange(0, 10)] RangedFloat _shapeDistance;
    [SerializeField][MinMaxRange(-10, 10)] RangedFloat _positionY;
    [SerializeField][MinMaxRange(-5, 5)] RangedFloat _height;
    [Header("Tangent")]
    [SerializeField][MinMaxRange(-5, 5)] RangedFloat _tangentNoiseX;
    [SerializeField][MinMaxRange(-5, 5)] RangedFloat _tangentNoiseY;
    // [SerializeField] float _maxTangentNoiseX = 2f;
    // [SerializeField] float _minTangentNoiseY = 0f;
    // [SerializeField] float _maxTangentNoiseY = 2f;
    [SerializeField] List<SpriteShape> _sprites = new List<SpriteShape>();
    [Header("GameSetting")]
    [SerializeField] float _parallaxOffset = 1f;
    public RangedFloat ShapeDistance { get => _shapeDistance; }
    public RangedFloat Heigh { get => _height; }
    public RangedFloat TangentNoiseX { get => _tangentNoiseX; }
    public RangedFloat TangentNoiseY { get => _tangentNoiseY; }

    public List<SpriteShape> Sprites { get => _sprites; }
    public float ParallaxOffset { get => _parallaxOffset; }
    public RangedFloat PositionY { get => _positionY; }
    public int ObjectCount { get => _objectCount; }
    public float ObjectGap { get => _ObjectGap; }
}
