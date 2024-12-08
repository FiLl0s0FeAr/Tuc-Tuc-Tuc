using UnityEngine;

public class WinLineScript : MonoBehaviour
{
    public GameObject line;

    public void spawnLine(float coordinateX, float coordinateY)
    {
        Instantiate(line, new Vector2(coordinateX, coordinateY), transform.rotation);
    }
}
