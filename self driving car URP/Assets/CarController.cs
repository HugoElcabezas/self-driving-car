using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Vector3 start_position, start_rotation;

    [Range(-1f, 1f)]
    public float acceleration, turning;

    public float time_since_start = 0f;

    [Header("Fitness")]
    public float overall_fitness;
    public float distance_multiplier = 1.4f;
    public float average_speed_multiplier = 0.2f;

    private Vector3 last_position;
    private float total_distance_travelled;
    private float average_speed;

    private float a_sensor, b_sensor, c_sensor;

    private void awake() {
        start_position = transform.position;
        start_rotation = transform.eulerAngles; 
    }

    private void reset() {
        time_since_start = 0f;
        total_distance_travelled = 0f;
        average_speed = 0f;
        last_position = start_position;
        transform.eulerAngles = start_rotation;
    }

    private Vector3 input;
    private void moveCar(float vertical, float horizontal) {
        // Position
        input = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, vertical * 11.4f), 0.02f);
        input = transform.TransformDirection(input);
        transform.position += input;
        // Rotation
        /*
         * horizontal * 90
         * This gives reference of a percentage for turning 90 degrees.
         * This value goes from -1 to +1, meaning turning left or right.
         * We don't do it instanteniously, we do it smoothly so it looks realistic.
         * So we use a small value for changes in time.
         */
        transform.eulerAngles += new Vector3(0, horizontal* 90 * 0.02f, 0);
    }
    private void onColissionEnter (Collision collission) {
        reset();
    }
}
