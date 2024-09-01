using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int Coordinates { get; set; }

    public Tile Tile { get; set; }

    public bool IsEmpty { get { return Tile == null; }  }
}
