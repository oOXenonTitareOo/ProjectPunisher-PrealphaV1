using UnityEngine;
using UnityEngine.InputSystem;

public class DebugVisualizer : MonoBehaviour
{
    public static bool showDebug = true;
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            showDebug = !showDebug;

            Debug.Log("Mágikus Debug Vonalak: " + (showDebug ? "BEKAPCSOLVA" : "KIKAPCSOLVA"));
        }
    }

    public static void DrawArrow(Vector3 start, Vector3 direction, Color color, float arrowLenght = 0.3f, float arrowAngle = 25f)
    {
        if (!showDebug || direction == Vector3.zero) return;

        Debug.DrawRay(start, direction, color);

        // Draws the main debug line
        Vector3 endPosition = start + direction;
        Quaternion lookRot = Quaternion.LookRotation(direction);

        // Calculates the 3D arrowhead fins - so the arrowheads little additions
        Vector3 right = lookRot * Quaternion.Euler(0, 180 + arrowAngle, 0) * Vector3.forward;
        Vector3 left = lookRot * Quaternion.Euler(0, 180 - arrowAngle, 0) * Vector3.forward;
        Vector3 up = lookRot * Quaternion.Euler(180 + arrowAngle, 0, 0) * Vector3.forward;
        Vector3 down = lookRot * Quaternion.Euler(180 - arrowAngle, 0, 0) * Vector3.forward;
        
        // Draws the arrowhead
        Debug.DrawRay(endPosition, right * arrowLenght, color);
        Debug.DrawRay(endPosition, left * arrowLenght, color);
        Debug.DrawRay(endPosition, up * arrowLenght, color);
        Debug.DrawRay(endPosition, down * arrowLenght, color);
    }
}
