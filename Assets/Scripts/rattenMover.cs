using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 2f;
    public float leftBoundary = -10f;
    public float rightBoundary = 10f;
    private bool movingRight = true;
    private SpriteRenderer spriteRenderer;
    private bool isStopped = false;
    private bool isCollected = false;

    private ItemData itemData; // Variable zum Speichern der ItemData-Komponente

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemData = GetComponent<ItemData>(); // ItemData-Komponente abrufen

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
        if (!isStopped && !isCollected)
        {
            Move();
            CheckBoundaries();
        }
    }

    void Move()
    {
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            spriteRenderer.flipX = false;
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
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

    void OnMouseDown()
{
    Debug.Log("Mouse down on Mover!");
    if (!isCollected)
    {
        if (!isStopped)
        {
            isStopped = true;
        }
        else
        {
            isCollected = true;
            GameManager.Instance.CollectMouse(this);
            gameObject.SetActive(false); // Deaktiviere das GameObject, nachdem es eingesammelt wurde
        }
    }
    else
    {
        Debug.LogWarning("Mouse already collected!"); // Warnung, falls die Maus bereits gesammelt wurde
    }
}

}
