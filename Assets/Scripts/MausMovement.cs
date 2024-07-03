using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 2f;
    public float leftBoundary = -10f; // Definiere die linke Grenze
    public float rightBoundary = 10f; // Definiere die rechte Grenze
    private bool movingRight = true;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Überprüfen, ob die Startposition innerhalb der Grenzen liegt
        if (transform.position.x < leftBoundary)
        {
            Debug.LogWarning("Startposition außerhalb der linken Grenze. Setze Objekt auf linke Grenze.");
            transform.position = new Vector3(leftBoundary, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > rightBoundary)
        {
            Debug.LogWarning("Startposition außerhalb der rechten Grenze. Setze Objekt auf rechte Grenze.");
            transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);
        }
    }

    void Update()
    {
        Move();
        CheckBoundaries();
    }

    void Move()
    {
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            // Nur wenn wir nach rechts gehen, soll der Sprite nicht geflippt sein
            spriteRenderer.flipX = false;
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            // Nur wenn wir nach links gehen, soll der Sprite geflippt sein
            spriteRenderer.flipX = true;
        }
    }

    void CheckBoundaries()
    {
        if (transform.position.x >= rightBoundary)
        {
            movingRight = false;
        }
        else if (transform.position.x <= leftBoundary)
        {
            movingRight = true;
        }
    }
}
