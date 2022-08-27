using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }
    public TetrominoData[] Tetrominoes;
    public Piece ActualPiece { get; private set; }
    public Vector3Int SpawnPos;
    public Vector2Int BoardSize = new Vector2Int(10, 20);
    public RectInt Bounds
    {
        get
        {
            Vector2Int pos = new Vector2Int(-BoardSize.x / 2, -BoardSize.y / 2);
            return new RectInt(pos, BoardSize);
        }
    }

    private void Awake()
    {
        this.Tilemap = GetComponentInChildren<Tilemap>();
        ActualPiece = GetComponent<Piece>();

        for (int i = 0; i < Tetrominoes.Length; i++)
        {
            Tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, Tetrominoes.Length);
        TetrominoData data = Tetrominoes[random];

        ActualPiece.Initialize(this, SpawnPos, data);

        if (ValidPos(ActualPiece, SpawnPos))
        {
            Set(ActualPiece);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Tilemap.ClearAllTiles();
        Debug.Log("Game over");
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.TheCells.Length; i++)
        {
            Vector3Int tilePos = piece.TheCells[i] + piece.ThePosition;
            Tilemap.SetTile(tilePos, piece.Tetrodata.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.TheCells.Length; i++)
        {
            Vector3Int tilePos = piece.TheCells[i] + piece.ThePosition;
            Tilemap.SetTile(tilePos, null);
        }
    }

    public bool ValidPos(Piece piece, Vector3Int pos)
    {
        RectInt gridBounds = Bounds;

        for (int i = 0; i < piece.TheCells.Length; i++)
        {
            Vector3Int tilePos = piece.TheCells[i] + pos;

            if (!gridBounds.Contains((Vector2Int)tilePos))
            {
                return false;
            }

            if (Tilemap.HasTile(tilePos))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt theBounds = Bounds;
        int row = theBounds.yMin;

        while (row < theBounds.yMax)
        {
            if (LineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private void LineClear(int row)
    {
        RectInt theBounds = Bounds;

        for (int col = theBounds.xMin; col < theBounds.xMax; col++)
        {
            Vector3Int pos = new Vector3Int(col, row, 0);
            Tilemap.SetTile(pos, null);
        }

        while (row < Bounds.yMax)
        {
            for (int col = theBounds.xMin; col < theBounds.xMax; col++)
            {
                Vector3Int pos = new Vector3Int(col, row + 1, 0);
                TileBase above = Tilemap.GetTile(pos);

                pos = new Vector3Int(col, row, 0);
                Tilemap.SetTile(pos, above);
            }

            row++;
        }
    }

    bool LineFull(int row)
    {
        RectInt theBounds = Bounds;

        for (int col = theBounds.xMin; col < theBounds.xMax; col++)
        {
            Vector3Int pos = new Vector3Int(col, row, 0);

            if (!Tilemap.HasTile(pos))
            {
                return false;
            }
        }
        return true;
    }
}
