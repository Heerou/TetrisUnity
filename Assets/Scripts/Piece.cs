using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board TheBoard { get; private set; }
    public TetrominoData Tetrodata { get; private set; }
    public Vector3Int ThePosition { get; private set; }
    public Vector3Int[] TheCells { get; private set; }
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.TheBoard = board;
        this.Tetrodata = data;
        this.ThePosition = position;

        if (this.TheCells == null)
        {
            this.TheCells = new Vector3Int[data.Cells.Length];
        }

        for (int i = 0; i < data.Cells.Length; i++)
        {
            this.TheCells[i] = (Vector3Int)data.Cells[i];
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
