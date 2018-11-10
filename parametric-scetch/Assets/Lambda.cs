using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lambda : Coordinate
{
    public override float Value => (1f - _lambda) * _parent0.Value + _lambda * _parent1.Value;
    public Lambda(Coordinate parent0, Coordinate parent1, float lambda)
    {
        _parent0 = parent0;
        _parent1 = parent1;
        _lambda = lambda;
    }

    private Coordinate _parent0;
    private Coordinate _parent1;
    private float _lambda;
}