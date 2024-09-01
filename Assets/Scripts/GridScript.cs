using Unity.VisualScripting;
using UnityEngine;

public class Grid2048 : MonoBehaviour
{
    public Row[] rows;

    public Cell[] cells;

    public int Size => cells.Length;

    public int Width => rows.Length;

    public int Height => cells.Length / rows.Length;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
        cells = GetComponentsInChildren<Cell>();
    }

    private void Start()
    {

        for (int i = 0; i < rows.Length; i++)
        {
            for (int j = 0; j < rows.Length; j++)
            {
                rows[i].cells[j].Coordinates = new Vector2Int(j, i);
            }
        }
    }

    public Cell GetEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startIndex = index;

        while (!cells[index].IsEmpty)
        {
            index++;

            if (index == cells.Length)
                index = 0;

            if (index == startIndex)
            {
                return null;
            }
        }
        return cells[index];
    }

}
