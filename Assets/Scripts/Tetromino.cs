using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I, J, L, O, S, T, Z
}

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] Cells { get; private set; }
    public Vector2Int[,]WallKicks { get; private set; }

    public void Initialize()
    {
        Cells = Data.Cells[this.tetromino];
        WallKicks = Data.WallKicks[this.tetromino];
    }
}