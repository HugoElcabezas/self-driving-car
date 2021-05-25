using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;
using System;

public class NNet : MonoBehaviour
{
    public Matrix<float> input_layer = Matrix<float>.Build.Dense(1, 3);

    public List<Matrix<float>> hidden_layers = new List<Matrix<float>>();

    public Matrix<float> output_layer = Matrix<float>.Build.Dense(1, 2);

    public List<Matrix<float>> weights = new List<Matrix<float>>();

    public List<float> biases = new List<float>();
}
