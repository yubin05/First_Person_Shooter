using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ReactionData", menuName = "Scriptable Object/ReactionData", order = int.MaxValue)]
public class ReactionData : ScriptableObject
{
    [SerializeField] private Vector3 reactVec;
    public Vector3 ReactVec => reactVec;
    [SerializeField] private float reactTime;
    public float ReactTime => reactTime;
}
