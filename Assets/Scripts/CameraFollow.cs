using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Spieler Transform
    public Vector3 offset;   // Kameraversatz
    public Vector2 minPosition; // Minimale Kameraposition
    public Vector2 maxPosition; // Maximale Kameraposition
    public float edgeOffset = 1f; // Randbereichsversatz

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 targetPosition = transform.position;

            // Erhalten der Weltkoordinaten der Kamerasichtfeldgrenzen
            Vector3 screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -transform.position.z));
            Vector3 screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -transform.position.z));

            // Berechnen der Randposition des Kamerasichtfeldes
            float cameraLeftEdge = screenBottomLeft.x;
            float cameraRightEdge = screenTopRight.x;
            float cameraBottomEdge = screenBottomLeft.y;
            float cameraTopEdge = screenTopRight.y;

            // Überprüfen, ob sich der Spieler außerhalb des Randbereichs befindet
            bool shouldFollowX = player.position.x > cameraLeftEdge + edgeOffset && player.position.x < cameraRightEdge - edgeOffset;
            bool shouldFollowY = player.position.y > cameraBottomEdge + edgeOffset && player.position.y < cameraTopEdge - edgeOffset;

            // Wenn sich der Spieler außerhalb des Randbereichs befindet, beginnt das Folgen
            if (shouldFollowX)
            {
                targetPosition.x = player.position.x + offset.x;
            }

            if (shouldFollowY)
            {
                targetPosition.y = player.position.y + offset.y;
            }

            // Begrenzen der Kameraposition
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
}
