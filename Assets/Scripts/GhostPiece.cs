using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostPiece : MonoBehaviour
{
    public Tile TheTile;
    public Board TheBoard;
    public Piece TrackinPiece;

    public Tilemap TheTileMap { get; private set; }
    public Vector3Int ThePosition { get; private set; }
    public Vector3Int[] TheCells { get; private set; }

    private void Awake()
    {
        TheTileMap = GetComponentInChildren<Tilemap>();
        TheCells = new Vector3Int[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    void Clear()
    {
        for (int i = 0; i < TheCells.Length; i++)
        {
            Vector3Int tilePos = TheCells[i] + ThePosition;
            TheTileMap.SetTile(tilePos, null);
        }
    }

    void Copy()
    {
        for (int i = 0; i < TheCells.Length; i++)
        {
            TheCells[i] = TrackinPiece.TheCells[i];
        }
    }

    void Drop()
    {
        Vector3Int pos = TrackinPiece.ThePosition;

        int currentRow = pos.y;
        int bottomBoard = -TheBoard.BoardSize.y / 2 - 1;

        TheBoard.Clear(TrackinPiece);

        for(int row =  currentRow; row >= bottomBoard; row--)
        {
            pos.y = row;

            if(TheBoard.ValidPos(TrackinPiece, pos))
            {
                ThePosition  = pos;
            }
            else
            {
                break;
            }
        }

        TheBoard.Set(TrackinPiece);
    }

    void Set()
    {
        for (int i = 0; i < TheCells.Length; i++)
        {
            Vector3Int tilePos = TheCells[i] + ThePosition;
            TheTileMap.SetTile(tilePos, TheTile);
        }
    }
}
