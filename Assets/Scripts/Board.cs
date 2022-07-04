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
        Set(ActualPiece);
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
}
