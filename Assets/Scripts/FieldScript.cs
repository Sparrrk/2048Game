using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour
{
    [SerializeField] GameGenerator gameGenerator;

    public TileState[] tileStates;

    public Tile tilePrefab;

    private Grid2048 grid;

    private List<Tile> tiles = new List<Tile>();

    private bool isCanInput = true;

    private void Awake()
    {
        grid = GetComponentInChildren<Grid2048>();
        tiles = new List<Tile>();
    }

    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0], 2);
        tile.SetPosition(grid.GetEmptyCell());
        tiles.Add(tile);
    }

    public void ClearField()
    {
        foreach (Tile tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        foreach (Cell cell in grid.cells)
        {
            cell.Tile = null;
        }

        tiles.Clear();
    }

    private void Update()
    {
        GetInputDirection();
    }

    private void GetInputDirection()
    {
        if (isCanInput)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Vector2Int.down, 0, 1, grid.Height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Vector2Int.right, grid.Width - 2, -1, 0, 1);
            }
        }
    }

    private void MoveTiles(Vector2Int direction, int startX, int stepX, int startY, int stepY)
    {
        bool changed = false;

        for (int x = startX; x < grid.Width && x >= 0; x += stepX)
        {
            for (int y = startY; y < grid.Height && y >= 0; y += stepY)
            {
                changed |= MoveTile(direction, grid.rows[y].cells[x]);
            }
        }

        if (changed)
        {
            StartCoroutine(WaitNextStep());
        }

    }

    private bool MoveTile(Vector2Int direction, Cell cell)
    {
        if (cell.IsEmpty) return false;

        Cell newCell = null;
        Cell neighbourCell = GetNeighbourCell(direction, cell);

        while (neighbourCell != null)
        {
            if (!neighbourCell.IsEmpty) 
            {
                if (IsCanMerge(cell, neighbourCell))
                {
                    Merge(cell.Tile, neighbourCell.Tile);
                    return true;
                }
                break;
            }

            newCell = neighbourCell;
            neighbourCell = GetNeighbourCell(direction, neighbourCell);
        }
        if (newCell != null)
        {
            cell.Tile.MoveTile(newCell);
            return true;
        }
        return false;
    }

    private bool IsCanMerge(Cell a, Cell b)
    {
        return a.Tile.Number == b.Tile.Number && !b.Tile.isLocked;
    }

    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.MergeTile(b.Cell);

        int index = Mathf.Clamp(GetIndexState(b) + 1, 0, tileStates.Length - 1);
        gameGenerator.UpdateScore(b.Number * 2);
        b.SetState(tileStates[index], b.Number * 2);
    }

    private int GetIndexState(Tile tile)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (tile.TileState == tileStates[i])
                return i;
        }

        return -1;
    }

    private Cell GetNeighbourCell(Vector2Int direction, Cell cell)
    {
        int newX = cell.Coordinates.x + direction.x;
        int newY = cell.Coordinates.y - direction.y;
        if (newX >= 0 && newX < grid.Width && newY >= 0 && newY < grid.Height)
        {
            return grid.rows[newY].cells[newX];
        }
        else 
        {
            return null; 
        }
    }

    private IEnumerator WaitNextStep()
    {
        isCanInput = false;

        for (int i = 0; i < tiles.Count; i++)
            tiles[i].isLocked = false;


        yield return new WaitForSeconds(0.1f);

        isCanInput = true;
        CreateTile();

        if (CheckForGameOver())
            gameGenerator.GameOver();
    }

    public bool CheckForGameOver()
    {
        if (tiles.Count < grid.Size) return false;

        foreach (Tile tile in tiles)
        {

            Cell up = GetNeighbourCell(Vector2Int.up, tile.Cell);
            Cell down = GetNeighbourCell(Vector2Int.down, tile.Cell);
            Cell left = GetNeighbourCell(Vector2Int.left, tile.Cell);
            Cell right = GetNeighbourCell(Vector2Int.right, tile.Cell);

            if (up != null && IsCanMerge(tile.Cell, up)) { return false; }
            if (down != null && IsCanMerge(tile.Cell, down)) { return false; }
            if (left != null && IsCanMerge(tile.Cell, left)) { return false; }
            if (right != null && IsCanMerge(tile.Cell, right)) { return false; }
        }
        return true;
    }


}
