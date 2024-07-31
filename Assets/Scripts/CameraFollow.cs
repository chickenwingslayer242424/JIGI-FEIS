using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Spieler Transform
    public Vector3 offset;   // Kamera Offset
    private Vector2 minPosition; // Minimale Kameraposition
    private Vector2 maxPosition; // Maximale Kameraposition

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;

            // Begrenzung der Kameraposition
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x + Camera.main.orthographicSize * Camera.main.aspect, maxPosition.x - Camera.main.orthographicSize * Camera.main.aspect);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y + Camera.main.orthographicSize, maxPosition.y - Camera.main.orthographicSize);

            transform.position = targetPosition;
        }
    }

    public void ResetCameraPosition()
    {
        if (player != null)
        {
            Vector3 resetPosition = player.position + offset;
            resetPosition.x = Mathf.Clamp(resetPosition.x, minPosition.x + Camera.main.orthographicSize * Camera.main.aspect, maxPosition.x - Camera.main.orthographicSize * Camera.main.aspect);
            resetPosition.y = Mathf.Clamp(resetPosition.y, minPosition.y + Camera.main.orthographicSize, maxPosition.y - Camera.main.orthographicSize);

            transform.position = resetPosition;
        }
    }

    // Methode, um die Grenzen der Szene festzulegen
    public void SetSceneBounds(Vector2 newMinPosition, Vector2 newMaxPosition)
    {
        minPosition = newMinPosition;
        maxPosition = newMaxPosition;
    }
}
