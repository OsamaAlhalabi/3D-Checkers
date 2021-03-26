using System.Collections.Generic;
using UnityEngine;
using Checkers;

public class Piece : Movement {
    public bool isWhite;
    public bool isKing;
    private bool isMoved;
    private int row;
    private int col;
    public MovingControl possibleMoves;
    private static string movedname;
    public void setRow(int row) {
        this.row = row;
    }
    public void setCol(int col) {
        this.col = col;
    }
    public int getRow() {
        return row;
    }
    public int getCol() {
        return col;
    }
    public bool getIsWhite() {
        return isWhite;
    }
    public bool getIsKing() {
        return isKing;
    }
    public void FindAllPossibleMoves(List<List<Action>> actions, GameObject[,] grid, Square[,] gameBoard, Material material1, Material material2, Material material4) {
        possibleMoves = new MovingControl();

        List<List<Action>> moves = new List<List<Action>>();
        List<int> li = new List<int>();
        for (int i = 0; i < actions.Count; i++)
            for (int j = 0; j < actions[i].Count; j++)
                if (actions[i][j].Src.R == row && actions[i][j].Src.C == col)
                    li.Add(i);
        for (int i = 0; i < actions.Count; i++)
            foreach (int j in li)
                if (j == i)
                    moves.Add(actions[i]);
        foreach (List<Action> a in moves)
            possibleMoves.AddPossiblePath(fillPath(a, gameBoard));
        foreach (Square square in possibleMoves.GetAllFinalTargets()) {
            square.HighLightMove(material4);
        }
        foreach (List<Square> list in possibleMoves.GetAllPathsEmpty())
            foreach (Square square1 in list) {
                if (grid[square1.row, square1.col] != null)
                    square1.HighLightMove(material2);
                else
                    square1.HighLightMove(material1);
            }
    }
    public List<Square> fillPath(List<Action> list, Square[,] gameBoard) {
        List<Square> res = new List<Square>();
        //Debug.Log("filling path");
        for (int i = 0; i < list.Count; i++) {
            int DirR = 0, DirC = 0;
            int r = list[i].Src.R;
            int c = list[i].Src.C;
            int dc = list[i].Dst.C;
            int dr = list[i].Dst.R;
            //Debug.Log("ababa");
            if (dr != r) {
                DirR = (dr - r) / Mathf.Abs(dr - r);
            }
            if (dc != c) {
                DirC = (dc - c) / Mathf.Abs(dc - c);
            }
            //Debug.Log(r + " " + dr + " " + c + " " + dc + " " + DirR + " " + DirC);
            while (r != dr || c != dc) {
                //res.Add(gameBoard[r, c]);
                r += DirR;
                c += DirC;
                //Debug.Log("in while: " + r + " " + c);
                res.Add(gameBoard[r, c]);
            }

            //res.Add(gameBoard[dr, dc]);
            //if (list[i].Src.C != list[i].Dst.C) {

            //    while (c != dc) {
            //        res.Add(gameBoard[r, c]);
            //        //Debug.Log(r + " " + c);
            //        c += DirC;
            //    }
            //    res.Add(gameBoard[dr, dc]);
            //}
        }
        //Debug.Log("Zaher method");
        //foreach (Square sq in res) {
        //    Debug.Log(sq.row + " " + sq.col);
        //}

        //res.Clear();
        //for (int i = 0; i < list.Count; i++) {
        //    if (list[i].Src.R <= list[i].Dst.R) {
        //        if (list[i].Src.C <= list[i].Dst.C)
        //            for (int j = 0; j < (list[i].Dst.R - list[i].Src.R); j++)
        //                res.Add(gameBoard[list[i].Src.R + 1 + j, list[i].Src.C + 1 + j]);
        //        else if (list[i].Src.C >= list[i].Dst.C)
        //            for (int j = 0; j < (list[i].Dst.R - list[i].Src.R); j++)
        //                res.Add(gameBoard[list[i].Src.R + 1 + j, list[i].Src.C - 1 - j]);
        //    }
        //    else if (list[i].Src.R > list[i].Dst.R) {
        //        if (list[i].Src.C <= list[i].Dst.C)
        //            for (int j = 0; j < (list[i].Src.R - list[i].Dst.R); j++)
        //                res.Add(gameBoard[list[i].Src.R - 1 - j, list[i].Src.C + 1 + j]);
        //        else if (list[i].Src.C >= list[i].Dst.C)
        //            for (int j = 0; j < (list[i].Src.R - list[i].Dst.R); j++)
        //                res.Add(gameBoard[list[i].Src.R - 1 - j, list[i].Src.C - 1 - j]);
        //    }
        //}
        //for (int i = 0; i < list.Count; i++) {
        //    if (list[i].Src.R == list[i].Dst.R) {
        //        if (list[i].Src.C < list[i].Dst.C)
        //            for (int j = 0; j < (list[i].Dst.R - list[i].Src.R); j++)
        //                res.Add(gameBoard[list[i].Src.R, list[i].Src.C + j]);
        //        else if (list[i].Src.C > list[i].Dst.C)
        //            for (int j = 0; j < (list[i].Dst.R - list[i].Src.R); j++)
        //                res.Add(gameBoard[list[i].Src.R, list[i].Src.C - j]);
        //    }
        //    if (list[i].Src.R != list[i].Dst.R) {
        //        if (list[i].Src.C < list[i].Dst.C)
        //            for (int j = 0; j < (list[i].Dst.R - list[i].Src.R); j++)
        //                res.Add(gameBoard[list[i].Src.R + j, list[i].Src.C]);
        //        else if (list[i].Src.C > list[i].Dst.C)
        //            for (int j = 0; j < (list[i].Dst.R - list[i].Src.R); j++)
        //                res.Add(gameBoard[list[i].Src.R - j, list[i].Src.C]);
        //    }
        //}
        //Debug.Log("Osama method");
        //foreach(Square sq in res) {
        //    Debug.Log(sq.row + " " + sq.col);
        //}

        return res;
    }
    public void UnHighLightSquares() {
        foreach (List<Square> list in possibleMoves.GetAllPathsEmpty())
            foreach (Square square in list) {
                square.UnHighLightSquare();
            }

        foreach (Square square1 in possibleMoves.GetAllFinalTargets())
            square1.UnHighLightSquare();
    }
    public void SelectPiece() {
        isMoved = true;
        ZeroGravity();
        MoveBy(new Vector3(0, 0.25f, 0), null);

    }
    public void MovePiece(Square target) {
        MovePiece(target, Drop);
    }
    public void MaximumEatMove(List<Square> target) {
        MaximumEatMove(target, Drop);
    }
    public void Drop() {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        isMoved = false;
    }

    public void Eat() {
        EatPiece(gameObject);
    }

    public static List<Piece> list = new List<Piece>();
    private void OnCollisionEnter(Collision collision) {
        if (!isMoved) {
            movedname = transform.name;
        }
        if (!isMoved && (collision.collider.tag == "WhitePiece" || collision.collider.tag == "BlackPiece")) {
            if (collision.collider.name == "WhiteKing(Clone)" || collision.collider.name == "WhitePiece(Clone)") {
                if (movedname != "WhitePiece(Clone)" && movedname != "WhiteKing(Clone)") {
                    if(BoardController.currentGame.Mode == Mode.INTERNATIONAL)
                        list.Add(this);
                    else
                        Eat();

                }
            }
            else if (collision.collider.name == "BlackKing(Clone)" || collision.collider.name == "BlackPiece(Clone)") {
                if (movedname != "BlackPiece(Clone)" && movedname != "BlackKing(Clone)") {
                    if (BoardController.currentGame.Mode == Mode.INTERNATIONAL)
                        list.Add(this);
                    else
                        Eat();
                }
            }

        }
        else if (collision.collider.tag == "Board") {
            foreach (Piece p in list) {
                p.Eat();
            }
            list = new List<Piece>();
            //Debug.Log("Pices : " + p);
        }
    }
}
