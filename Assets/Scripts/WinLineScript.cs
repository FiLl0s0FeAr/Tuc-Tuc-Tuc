using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;

public class WinLineScript : MonoBehaviour
{
    public GameObject line;

    public void SpawnStraightLine(float coordinateX, float coordinateY, float coordinateZ = -7)
    {
        Instantiate(line, new Vector3(coordinateX, coordinateY, coordinateZ), transform.rotation);
    }

    public void SpawnHorizontalLine(float coordinateX, float coordinateY, float coordinateZ = -7)
    {
        Instantiate(line, new Vector3(coordinateX, coordinateY, coordinateZ), Quaternion.Euler(0, 0, 90));
    }

    public void SpawnDiagonalLine(float coordinateX, float coordinateY, bool startLeft = true, float coordinateZ = -7)
    {
        int rotation = startLeft ? 45 : 135;
        Instantiate(line, new Vector3(coordinateX, coordinateY, coordinateZ), Quaternion.Euler(0, 0, rotation));
    }
}
