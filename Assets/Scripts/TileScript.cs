using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileState TileState {  get; private set; }

    public Cell Cell { get; set; }

    public int Number { get; private set; }

    private Image background;

    private TextMeshProUGUI text;

    public bool isLocked = false;

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetState(TileState state, int parNumber)
    {
        this.TileState = state;
        this.Number = parNumber;

        background.color = TileState.BackgroundColor;
        text.color = TileState.TextColor;
        text.text = Number.ToString();
    }

    public void SetPosition(Cell parCell)
    {
        if (Cell != null)
        {
            Cell.Tile = null;
        }

        Cell = parCell;
        Cell.Tile = this;

        transform.position = Cell.transform.position;
    }

    public void MoveTile(Cell parCell)
    {
        if (Cell != null)
        {
            Cell.Tile = null;
        }

        Cell = parCell;
        Cell.Tile = this;

        StartCoroutine(SlideTile(transform.position, parCell.transform.position));
    }


    public void MergeTile(Cell parCell)
    {
        if (Cell != null)
        {
            Cell.Tile = null;
        }

        StartCoroutine(SlideTile(transform.position, parCell.transform.position));
        parCell.Tile.isLocked = true;

        Destroy(gameObject);
    }

    private IEnumerator SlideTile(Vector3 from, Vector3 to)
    {
        float currentTime = 0f;
        float duration = 0.1f;

        while(currentTime < duration)
        {
            transform.position = Vector3.Lerp(from, to, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }

        transform.position = to;
    }


}
