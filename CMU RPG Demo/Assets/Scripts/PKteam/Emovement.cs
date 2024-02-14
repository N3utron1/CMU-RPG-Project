using UnityEngine;

public class Emovement : MonoBehaviour
{
    public float speed = 2f; // Speed of the wave
    public float frequency = 1f; // Frequency of the wave
    public float amplitude = 1f; // Amplitude of the wave
    public float range = 5f; // Range for the back-and-forth oscillation

    private Vector3 initialPosition;
    private float offsetX;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the horizontal movement using a sine wave
        offsetX += Time.deltaTime * speed;
        float offsetY = Mathf.Sin(offsetX * frequency) * amplitude;

        // Calculate back-and-forth oscillation along the X-axis
        float oscillation = Mathf.PingPong(offsetX, range) - range / 2f;

        // Update the object's position along the X and Y axes
        transform.position = new Vector3(initialPosition.x + oscillation, initialPosition.y + offsetY, initialPosition.z);
    }
}
