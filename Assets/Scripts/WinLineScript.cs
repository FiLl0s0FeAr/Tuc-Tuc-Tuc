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

    public void SpawnDiagonalLine(float coordinateX, float coordinateY, float coordinateZ = -7)
    {
        Instantiate(line, new Vector3(coordinateX, coordinateY, coordinateZ), Quaternion.Euler(0, 0, 45));
    }
}
