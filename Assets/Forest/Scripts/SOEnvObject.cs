using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


[CreateAssetMenu(menuName = "ScriptableObject/Environment/Object")]
public class SOEnvObject : ScriptableObject
{
    [Header("Objects")]
    [SerializeField] int _objectCount = 5;
    [SerializeField] float _objectGap = 10f;
    [SerializeField] float _densityVertical = 10f;
    [SerializeField] MapDirection _mapDirection;
    [Header("Shapes")]
    [SerializeField][MinMaxRange(-10, 10)] RangedFloat _generatePositionX;
    [SerializeField][MinMaxRange(-10, 10)] RangedFloat _generatePositionY;
    [SerializeField][MinMaxRange(0, 10)] RangedFloat _shapeDistance;
    [SerializeField][MinMaxRange(0.1f, 5)] RangedFloat _tangentHeight;
    [SerializeField][MinMaxRange(0, 5)] RangedFloat _tangentNoiseX;
    [SerializeField][MinMaxRange(-5, 5)] RangedFloat _tangentNoiseY;

    [SerializeField] List<SpriteShape> _sprites = new List<SpriteShape>();
    [Header("GameSetting")]
    [SerializeField] float _parallaxOffset = 1f;
    public RangedFloat ShapeDistance { get => _shapeDistance; }
    public RangedFloat Heigh { get => _tangentHeight; }
    public RangedFloat TangentNoiseX { get => _tangentNoiseX; }
    public RangedFloat TangentNoiseY { get => _tangentNoiseY; }

    public List<SpriteShape> Sprites { get => _sprites; }
    public float ParallaxOffset { get => _parallaxOffset; }
    public RangedFloat PositionY { get => _generatePositionY; }
    public RangedFloat PositionX { get => _generatePositionX; }
    public int ObjectCount { get => _objectCount; }
    public float ObjectGap { get => _objectGap; }
    public float DensityVertical { get => _densityVertical;}
    internal MapDirection MapDirection { get => _mapDirection;}
}