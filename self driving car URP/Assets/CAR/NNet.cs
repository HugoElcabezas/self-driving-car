using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;
using System;

using Random = UnityEngine.Random;

public class NNet : MonoBehaviour
{
    public Matrix<float> input_layer = Matrix<float>.Build.Dense(1, 3);

    public List<Matrix<float>> hidden_layers = new List<Matrix<float>>();

    public Matrix<float> output_layer = Matrix<float>.Build.Dense(1, 2);

    public List<Matrix<float>> weights = new List<Matrix<float>>();

    public List<float> biases = new List<float>();

    public float fitness;

    public void Initialize (int hidden_layer_count, int hidden_neuron_count)
    {
        input_layer.Clear();
        hidden_layers.Clear();
        output_layer.Clear();
        weights.Clear();
        biases.Clear();

        for (int i = 0; i < hidden_layer_count + 1; i++)
        {
            Matrix<float> hidden_layer = Matrix<float>.Build.Dense(1, hidden_layer_count);

            hidden_layers.Add(hidden_layer);

            biases.Add(Random.Range(-1f, 1f));

            // WEIGHTS

            if (i == 0)
            {
                Matrix<float> input_to_H1 = Matrix<float>.Build.Dense(3, hidden_neuron_count);
                weights.Add(input_to_H1);
            }

            Matrix<float> hidden_to_hidden = Matrix<float>.Build.Dense(hidden_neuron_count, hidden_neuron_count);
            weights.Add(hidden_to_hidden);
        }

        Matrix<float> output_weight = Matrix<float>.Build.Dense(hidden_neuron_count, 2);
        weights.Add(output_weight);
        biases.Add(Random.Range(-1f, 1f));

        RandomizeWeights();

    }

    public void RandomizeWeights()
    {
        for (int i = 0; i < weights.Count; i++)
        {
            for (int x = 0; x < weights[i].RowCount; x++)
            {
                for (int y = 0; y < weights[i].ColumnCount; y++)
                {
                    weights[i][x, y] = Random.Range(-1f, 1f);
                }

            }
        }
        
    }

    public(float, float) RunNetwork ( float a, float b, float c)
    {
        input_layer[0, 0] = a;
        input_layer[0, 1] = b;
        input_layer[0, 2] = c;

        input_layer = input_layer.PointwiseTanh();

        hidden_layers[0] = ((input_layer * weights[0]) + biases[0]).PointwiseTanh();

        for (int i = 1; i < hidden_layers.Count; i++)
        {
            hidden_layers[i] = ((hidden_layers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        }

        output_layer = ((hidden_layers[hidden_layers.Count - 1] * weights[weights.Count - 1]) + biases[biases.Count - 1]).PointwiseTanh();

        // El primer output es aceleración, el segundo es girar.
        return (Sigmoid(output_layer[0,0]), (float)Math.Tanh(output_layer[0,1]));
    }

    private float Sigmoid (float s)
    {
        return (1 / (1 + Mathf.Exp(-s)));
    }

}
