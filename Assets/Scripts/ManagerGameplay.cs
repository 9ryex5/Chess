using UnityEngine;

public class ManagerGameplay : MonoBehaviour
{
    public Transform parentCells;
    public Transform prefabCellWhite, prefabCellBlack;

    public Transform parentPieces;
    public Transform prefabPawnWhite, prefabKnightWhite, prefabBishopWhite, prefabRookWhite, prefabQueenWhite, prefabKingWhite;
    public Transform prefabPawnBlack, prefabKnightBlack, prefabBishopBlack, prefabRookBlack, prefabQueenBlack, prefabKingBlack;

    private Cell[,] board;

    private struct Cell
    {
        public Vector2Int pos;
        public Transform piece;
        public PieceType pieceType;
        public bool pieceBlack;
    }

    private enum PieceType
    {
        NONE,
        PAWN,
        KNIGHT,
        BISHOP,
        ROOK,
        QUEEN,
        KING,
    }

    private void Start()
    {
        board = new Cell[8, 8];

        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                board[x, y].pos = new Vector2Int(x, y);
                Instantiate((x % 2 == 0 && y % 2 == 0 || x % 2 != 0 && y % 2 != 0) ? prefabCellBlack : prefabCellWhite, new Vector2(x, y), Quaternion.identity, parentCells);
            }

        SpawnPieces();
    }

    private void SpawnPieces()
    {
        for (int x = 0; x < 8; x++)
        {
            board[x, 1].pieceType = PieceType.PAWN;
            board[x, 1].piece = Instantiate(prefabPawnWhite, new Vector2(x, 1), Quaternion.identity, parentPieces);
        }

        board[0, 0].pieceType = PieceType.ROOK;
        board[0, 0].piece = Instantiate(prefabRookWhite, new Vector2(0, 0), Quaternion.identity, parentPieces);
        board[1, 0].pieceType = PieceType.KNIGHT;
        board[1, 0].piece = Instantiate(prefabKnightWhite, new Vector2(1, 0), Quaternion.identity, parentPieces);
        board[2, 0].pieceType = PieceType.BISHOP;
        board[2, 0].piece = Instantiate(prefabBishopWhite, new Vector2(2, 0), Quaternion.identity, parentPieces);
        board[3, 0].pieceType = PieceType.QUEEN;
        board[3, 0].piece = Instantiate(prefabQueenWhite, new Vector2(3, 0), Quaternion.identity, parentPieces);
        board[4, 0].pieceType = PieceType.KING;
        board[4, 0].piece = Instantiate(prefabKingWhite, new Vector2(4, 0), Quaternion.identity, parentPieces);
        board[5, 0].pieceType = PieceType.BISHOP;
        board[5, 0].piece = Instantiate(prefabBishopWhite, new Vector2(5, 0), Quaternion.identity, parentPieces);
        board[6, 0].pieceType = PieceType.KNIGHT;
        board[6, 0].piece = Instantiate(prefabKnightWhite, new Vector2(6, 0), Quaternion.identity, parentPieces);
        board[7, 0].pieceType = PieceType.ROOK;
        board[7, 0].piece = Instantiate(prefabRookWhite, new Vector2(7, 0), Quaternion.identity, parentPieces);

        for (int x = 0; x < 8; x++)
        {
            board[x, 6].pieceType = PieceType.PAWN;
            board[x, 6].pieceBlack = true;
            board[x, 7].pieceBlack = true;
            board[x, 6].piece = Instantiate(prefabPawnBlack, new Vector2(x, 6), Quaternion.identity, parentPieces);
        }

        board[0, 7].pieceType = PieceType.ROOK;
        board[0, 7].piece = Instantiate(prefabRookBlack, new Vector2(0, 7), Quaternion.identity, parentPieces);
        board[1, 7].pieceType = PieceType.KNIGHT;
        board[1, 7].piece = Instantiate(prefabKnightBlack, new Vector2(1, 7), Quaternion.identity, parentPieces);
        board[2, 7].pieceType = PieceType.BISHOP;
        board[2, 7].piece = Instantiate(prefabBishopBlack, new Vector2(2, 7), Quaternion.identity, parentPieces);
        board[3, 7].pieceType = PieceType.QUEEN;
        board[3, 7].piece = Instantiate(prefabQueenBlack, new Vector2(3, 7), Quaternion.identity, parentPieces);
        board[4, 7].pieceType = PieceType.KING;
        board[4, 7].piece = Instantiate(prefabKingBlack, new Vector2(4, 7), Quaternion.identity, parentPieces);
        board[5, 7].pieceType = PieceType.BISHOP;
        board[5, 7].piece = Instantiate(prefabBishopBlack, new Vector2(5, 7), Quaternion.identity, parentPieces);
        board[6, 7].pieceType = PieceType.KNIGHT;
        board[6, 7].piece = Instantiate(prefabKnightBlack, new Vector2(6, 7), Quaternion.identity, parentPieces);
        board[7, 7].pieceType = PieceType.ROOK;
        board[7, 7].piece = Instantiate(prefabRookBlack, new Vector2(7, 7), Quaternion.identity, parentPieces);
    }
}
