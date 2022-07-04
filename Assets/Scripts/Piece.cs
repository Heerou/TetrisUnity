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
        TheBoard = board;
        Tetrodata = data;
        ThePosition = position;

        if (TheCells == null)
        {
            TheCells = new Vector3Int[data.Cells.Length];
        }

        for (int i = 0; i < data.Cells.Length; i++)
        {
            TheCells[i] = (Vector3Int)data.Cells[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        TheBoard.Clear(this);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        TheBoard.Set(this);
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPos = ThePosition;
        newPos.x += translation.x;
        newPos.y += translation.y;

        bool valid = TheBoard.ValidPos(this, newPos);

        if (valid)
        {
            ThePosition = newPos;
        }

        return valid;
    }
}
