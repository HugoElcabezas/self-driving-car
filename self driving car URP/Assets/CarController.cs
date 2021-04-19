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
    public float sensor_multiplier = 0.1f;

    private Vector3 last_position;
    private float total_distance_travelled;
    private float average_speed;

    private float right_sensor, straight_sensor, left_sensor;

    private void Awake() {
        start_position = transform.position;
        start_rotation = transform.eulerAngles; 
    }

    private void Reset() {
        time_since_start = 0f;
        total_distance_travelled = 0f;
        average_speed = 0f;
        last_position = start_position;
        transform.eulerAngles = start_rotation;
    }
    private void OnColissionEnter(Collision collission)
    {
        // Trabajar
        Reset();
    }

    private void FixedUpdate() {
        InputSensors();
        last_position = transform.position;

        // Neuronal Network code here to implement

        MoveCar(acceleration, turning);

        time_since_start += Time.deltaTime;

        CalculateFitness();

        //acceleration = 0;
        //turning = 0;
    }

    private void CalculateFitness() {
        total_distance_travelled += Vector3.Distance(transform.position, last_position);
        average_speed = total_distance_travelled / time_since_start;
        /*
         * In here get averages and multiply them by their multipliers.
         */
        overall_fitness = (total_distance_travelled * distance_multiplier) + (average_speed * average_speed_multiplier) + (((right_sensor + straight_sensor + left_sensor)/3)*sensor_multiplier);
        /*
         * We make conditionals:
         * 1. Take into account if there haven't been changes with time.
         * 2. At least 3 laps.
         */
        if (time_since_start > 20 && overall_fitness < 40) {
            Reset();
        }
        if (overall_fitness >= 1000) {
            // Saves the network to a JSON file and the deserialize it and retrain it in a different course.
            Reset();

        }
    }

    private void InputSensors()
    {
        Vector3 right = (transform.forward + transform.right);
        Vector3 straight = (transform.forward);
        Vector3 left = (transform.forward - transform.right);

        Ray ray = new Ray(transform.position, right);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            right_sensor = hit.distance / 20;
            //print("Right Sensor :" + right_sensor);
            //Debug.Log("Right Sensor :" + right_sensor);
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }

        ray.direction = straight;

        if (Physics.Raycast(ray, out hit)) {
            straight_sensor = hit.distance / 20;
            //print("Straight Sensor :" + straight_sensor);
            //Debug.Log("Straight Sensor :" + straight_sensor);
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }

        ray.direction = left;

        if (Physics.Raycast(ray, out hit)) {
            left_sensor = hit.distance / 20;
            //print("Left Sensor :" + left_sensor);
            //Debug.Log("Left Sensor :" + left_sensor);
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }

    }

    private Vector3 input;
    private void MoveCar(float vertical, float horizontal) {
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
        transform.eulerAngles += new Vector3(0, (horizontal* 90) * 0.02f, 0);
    }

}