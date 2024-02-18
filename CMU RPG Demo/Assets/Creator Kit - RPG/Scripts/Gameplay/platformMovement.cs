using UnityEngine;

public class platformMovement : MonoBehaviour
{
    public float moveDistance = 5f;
    public bool moveOnXAxis = true;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (moveOnXAxis)
        {
            MoveOnXAxis();
        }
        else
        {
            MoveOnYAxis();
        }
    }

    private void MoveOnXAxis()
    {
        float newX = initialPosition.x + Mathf.PingPong(Time.time * 2f, moveDistance * 2) - moveDistance;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void MoveOnYAxis()
    {
        float newY = initialPosition.y + Mathf.PingPong(Time.time * 2f, moveDistance * 2) - moveDistance;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
