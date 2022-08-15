using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board TheBoard { get; private set; }
    public TetrominoData Tetrodata { get; private set; }
    public Vector3Int ThePosition { get; private set; }
    public Vector3Int[] TheCells { get; private set; }
    public int RotationIndex { get; private set; }
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        TheBoard = board;
        Tetrodata = data;
        ThePosition = position;
        RotationIndex = 0;

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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

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

    void Rotate(int dir)
    {
        int orginalRotation = RotationIndex;
        RotationIndex += Wrap(RotationIndex + dir, 0, 4);

        ApplyRotation(dir);

        if (!TestWallKicks(RotationIndex, dir))
        {
            RotationIndex = orginalRotation;
            ApplyRotation(-dir);
        }
    }

    void ApplyRotation (int dir)
    {
        for (int i = 0; i < TheCells.Length; i++)
        {
            Vector3 cell = TheCells[i];

            int x, y;

            switch (Tetrodata.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * dir) + (cell.y * Data.RotationMatrix[1] * dir));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * dir) + (cell.y * Data.RotationMatrix[3] * dir));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * dir) + (cell.y * Data.RotationMatrix[1] * dir));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * dir) + (cell.y * Data.RotationMatrix[3] * dir));
                    break;
            }

            TheCells[i] = new Vector3Int(x, y, 0);
        }
    }

    bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickId = GetWallKickId(rotationIndex, rotationDirection);

        for (int i = 0; i < Tetrodata.WallKicks.GetLength(1); i++)
        {
            Vector2Int trans = Tetrodata.WallKicks[wallKickId, i];

            if (Move(trans))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickId(int rotationIndex, int rotationDirection)
    {
        int wallKickId = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickId--;
        }

        return Wrap(wallKickId, 0, Tetrodata.WallKicks.GetLength(0));
    }

    int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
