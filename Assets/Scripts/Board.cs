using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] Tetrominoes;
    public Piece actualPiece { get; private set; }
    public Vector3Int SpawnPos;

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        actualPiece = GetComponent<Piece>();

        for (int i = 0; i < this.Tetrominoes.Length; i++)
        {
            this.Tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.Tetrominoes.Length);
        TetrominoData data = this.Tetrominoes[random];

        this.actualPiece.Initialize(this, SpawnPos, data);
        Set(actualPiece);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.TheCells.Length; i++)
        {
            Vector3Int tilePos = piece.TheCells[i] + piece.ThePosition;
            tilemap.SetTile(tilePos, piece.Tetrodata.tile);
        }
    }
}
