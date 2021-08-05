using System.Collections.Generic;
using UnityEngine;

public class ManagerGameplay : MonoBehaviour
{
    public Camera myCamera;

    public Transform parentCells;
    public Transform prefabCellWhite, prefabCellBlack;

    public Transform parentPieces;
    public Transform prefabPawnWhite, prefabKnightWhite, prefabBishopWhite, prefabRookWhite, prefabQueenWhite, prefabKingWhite;
    public Transform prefabPawnBlack, prefabKnightBlack, prefabBishopBlack, prefabRookBlack, prefabQueenBlack, prefabKingBlack;

    public Transform parentPreviewMoves;
    public Transform prefabPreviewMove;
    public Transform prefabPreviewCapture;

    private Cell[,] board;
    private Cell selectedCell;
    private List<Cell> possibleMoves;

    private bool blackTurn;

    private struct Cell
    {
        public Vector2Int pos;
        public Transform tPiece;
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
        possibleMoves = new List<Cell>();

        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                board[x, y].pos = new Vector2Int(x, y);
                Instantiate((x % 2 == 0 && y % 2 == 0 || x % 2 != 0 && y % 2 != 0) ? prefabCellBlack : prefabCellWhite, new Vector2(x, y), Quaternion.identity, parentCells);
            }

        SpawnPieces();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int cellX = Mathf.RoundToInt(myCamera.ScreenToWorldPoint(Input.mousePosition).x);
            int cellY = Mathf.RoundToInt(myCamera.ScreenToWorldPoint(Input.mousePosition).y);

            if (IsPosBoard(new Vector2Int(cellX, cellY))) SelectCell(board[cellX, cellY]);
        }
    }

    private void SpawnPieces()
    {
        for (int x = 0; x < 8; x++)
        {
            board[x, 1].pieceType = PieceType.PAWN;
            board[x, 1].tPiece = Instantiate(prefabPawnWhite, new Vector2(x, 1), Quaternion.identity, parentPieces);
        }

        board[0, 0].pieceType = PieceType.ROOK;
        board[0, 0].tPiece = Instantiate(prefabRookWhite, new Vector2(0, 0), Quaternion.identity, parentPieces);
        board[1, 0].pieceType = PieceType.KNIGHT;
        board[1, 0].tPiece = Instantiate(prefabKnightWhite, new Vector2(1, 0), Quaternion.identity, parentPieces);
        board[2, 0].pieceType = PieceType.BISHOP;
        board[2, 0].tPiece = Instantiate(prefabBishopWhite, new Vector2(2, 0), Quaternion.identity, parentPieces);
        board[3, 0].pieceType = PieceType.QUEEN;
        board[3, 0].tPiece = Instantiate(prefabQueenWhite, new Vector2(3, 0), Quaternion.identity, parentPieces);
        board[4, 0].pieceType = PieceType.KING;
        board[4, 0].tPiece = Instantiate(prefabKingWhite, new Vector2(4, 0), Quaternion.identity, parentPieces);
        board[5, 0].pieceType = PieceType.BISHOP;
        board[5, 0].tPiece = Instantiate(prefabBishopWhite, new Vector2(5, 0), Quaternion.identity, parentPieces);
        board[6, 0].pieceType = PieceType.KNIGHT;
        board[6, 0].tPiece = Instantiate(prefabKnightWhite, new Vector2(6, 0), Quaternion.identity, parentPieces);
        board[7, 0].pieceType = PieceType.ROOK;
        board[7, 0].tPiece = Instantiate(prefabRookWhite, new Vector2(7, 0), Quaternion.identity, parentPieces);

        for (int x = 0; x < 8; x++)
        {
            board[x, 6].pieceType = PieceType.PAWN;
            board[x, 6].pieceBlack = true;
            board[x, 7].pieceBlack = true;
            board[x, 6].tPiece = Instantiate(prefabPawnBlack, new Vector2(x, 6), Quaternion.identity, parentPieces);
        }

        board[0, 7].pieceType = PieceType.ROOK;
        board[0, 7].tPiece = Instantiate(prefabRookBlack, new Vector2(0, 7), Quaternion.identity, parentPieces);
        board[1, 7].pieceType = PieceType.KNIGHT;
        board[1, 7].tPiece = Instantiate(prefabKnightBlack, new Vector2(1, 7), Quaternion.identity, parentPieces);
        board[2, 7].pieceType = PieceType.BISHOP;
        board[2, 7].tPiece = Instantiate(prefabBishopBlack, new Vector2(2, 7), Quaternion.identity, parentPieces);
        board[3, 7].pieceType = PieceType.QUEEN;
        board[3, 7].tPiece = Instantiate(prefabQueenBlack, new Vector2(3, 7), Quaternion.identity, parentPieces);
        board[4, 7].pieceType = PieceType.KING;
        board[4, 7].tPiece = Instantiate(prefabKingBlack, new Vector2(4, 7), Quaternion.identity, parentPieces);
        board[5, 7].pieceType = PieceType.BISHOP;
        board[5, 7].tPiece = Instantiate(prefabBishopBlack, new Vector2(5, 7), Quaternion.identity, parentPieces);
        board[6, 7].pieceType = PieceType.KNIGHT;
        board[6, 7].tPiece = Instantiate(prefabKnightBlack, new Vector2(6, 7), Quaternion.identity, parentPieces);
        board[7, 7].pieceType = PieceType.ROOK;
        board[7, 7].tPiece = Instantiate(prefabRookBlack, new Vector2(7, 7), Quaternion.identity, parentPieces);
    }

    private void SelectCell(Cell _cell)
    {
        ClearPreview();

        for (int i = 0; i < possibleMoves.Count; i++)
            if (possibleMoves[i].pos == _cell.pos)
            {
                MovePiece(new Vector2Int(_cell.pos.x, _cell.pos.y));
                return;
            }

        if (_cell.pieceType == PieceType.NONE || _cell.pieceBlack != blackTurn) return;

        selectedCell = _cell;
        SetPossibleMoves(_cell);
    }

    private void ClearPreview()
    {
        for (int i = 0; i < parentPreviewMoves.childCount; i++)
            Destroy(parentPreviewMoves.GetChild(i).gameObject);
    }

    private void SetPossibleMoves(Cell _cell)
    {
        possibleMoves = new List<Cell>();
        Vector2Int target = Vector2Int.zero;

        switch (_cell.pieceType)
        {
            case PieceType.PAWN:
                target = new Vector2Int(_cell.pos.x, blackTurn ? _cell.pos.y - 1 : _cell.pos.y + 1);
                if (IsPosBoard(target) && board[target.x, target.y].pieceType == PieceType.NONE) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x, blackTurn ? _cell.pos.y - 2 : _cell.pos.y + 2);
                if (IsPosBoard(target) && board[target.x, target.y].pieceType == PieceType.NONE && _cell.pos.y == 1 || _cell.pos.y == 6) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x - 1, blackTurn ? _cell.pos.y - 1 : _cell.pos.y + 1);
                if (IsPosBoard(target) && IsMoveCapture(target)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x + 1, blackTurn ? _cell.pos.y - 1 : _cell.pos.y + 1);
                if (IsPosBoard(target) && IsMoveCapture(target)) possibleMoves.Add(board[target.x, target.y]);
                break;
            case PieceType.KNIGHT:
                target = new Vector2Int(_cell.pos.x - 1, _cell.pos.y + 2);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x + 1, _cell.pos.y + 2);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x + 2, _cell.pos.y + 1);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x + 2, _cell.pos.y - 1);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x + 1, _cell.pos.y - 2);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x - 1, _cell.pos.y - 2);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x - 2, _cell.pos.y - 1);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x - 2, _cell.pos.y + 1);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                break;
            case PieceType.BISHOP:
                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x + i, _cell.pos.y + i);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x + i, _cell.pos.y - i);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x - i, _cell.pos.y - i);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x - i, _cell.pos.y + i);
                    if (!LinearTarget(target)) break;
                }
                break;
            case PieceType.ROOK:
                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x - i, _cell.pos.y);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x + i, _cell.pos.y);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x, _cell.pos.y + i);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x, _cell.pos.y - i);
                    if (!LinearTarget(target)) break;
                }
                break;
            case PieceType.QUEEN:
                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x + i, _cell.pos.y + i);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x + i, _cell.pos.y - i);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x - i, _cell.pos.y - i);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x - i, _cell.pos.y + i);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x - i, _cell.pos.y);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x + i, _cell.pos.y);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x, _cell.pos.y + i);
                    if (!LinearTarget(target)) break;
                }

                for (int i = 1; i < 8; i++)
                {
                    target = new Vector2Int(_cell.pos.x, _cell.pos.y - i);
                    if (!LinearTarget(target)) break;
                }
                break;
            case PieceType.KING:
                target = new Vector2Int(_cell.pos.x - 1, _cell.pos.y + 1);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x, _cell.pos.y + 1);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x + 1, _cell.pos.y + 1);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x + 1, _cell.pos.y);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x + 1, _cell.pos.y - 1);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x, _cell.pos.y - 1);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x - 1, _cell.pos.y - 1);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                target = new Vector2Int(_cell.pos.x - 1, _cell.pos.y);
                if (IsPosBoard(target) && (board[target.x, target.y].pieceType == PieceType.NONE || board[target.x, target.y].pieceBlack != blackTurn)) possibleMoves.Add(board[target.x, target.y]);
                break;
        }

        for (int i = 0; i < possibleMoves.Count; i++)
            Instantiate(IsMoveCapture(possibleMoves[i].pos) ? prefabPreviewCapture : prefabPreviewMove, (Vector2)possibleMoves[i].pos, Quaternion.identity, parentPreviewMoves);
    }

    private bool LinearTarget(Vector2Int _target)
    {
        if (!IsPosBoard(_target)) return false;

        if (board[_target.x, _target.y].pieceType == PieceType.NONE)
        {
            possibleMoves.Add(board[_target.x, _target.y]);
            return true;
        }
        else if (board[_target.x, _target.y].pieceBlack != blackTurn)
        {
            possibleMoves.Add(board[_target.x, _target.y]);
            return false;
        }
        else return false;
    }

    private bool IsMoveCapture(Vector2Int _cell)
    {
        return board[_cell.x, _cell.y].pieceType != PieceType.NONE && board[_cell.x, _cell.y].pieceBlack != blackTurn;
    }

    private void MovePiece(Vector2Int _to)
    {
        if (board[_to.x, _to.y].pieceType != PieceType.NONE) Destroy(board[_to.x, _to.y].tPiece.gameObject);

        board[_to.x, _to.y].tPiece = board[selectedCell.pos.x, selectedCell.pos.y].tPiece;
        board[_to.x, _to.y].pieceBlack = board[selectedCell.pos.x, selectedCell.pos.y].pieceBlack;
        board[_to.x, _to.y].pieceType = board[selectedCell.pos.x, selectedCell.pos.y].pieceType;
        board[selectedCell.pos.x, selectedCell.pos.y].tPiece.position = (Vector2)_to;
        board[selectedCell.pos.x, selectedCell.pos.y].pieceType = PieceType.NONE;
        blackTurn = !blackTurn;
    }

    private bool IsPosBoard(Vector2Int _pos)
    {
        return _pos.x >= 0 && _pos.x < 8 && _pos.y >= 0 && _pos.y < 8;
    }
}
