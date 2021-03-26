using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Checkers;
using Newtonsoft.Json;
using UnityEngine.UI;

public enum playerTurn {
    WHITE,
    BLACK
}

public class BoardController : MonoBehaviour {
    private Square[,] grid;
    public static Game currentGame;
    public GameObject[,] pieces;
    private BoardCoords coords = new BoardCoords();
    private Vector2Int previousSquare = new Vector2Int();
    private playerTurn playerColor;

    private Vector3 size;
    private Vector3 tileSize;
    private Vector2 mousePointer;
    private GameObject selectedPiece;
    private Square selectedTarget;
    private playerTurn PT;
    private bool runningGame;
    public GameObject tilePrefab1;
    public GameObject tilePrefab2;
    public GameObject whitePiece;
    public GameObject blackPiece;
    public GameObject whiteKing;
    public GameObject blackKing;
    public GameObject gameOver;

    public GameObject GameState;

    public Material highLightMaterial1;
    public Material highLightMaterial2;
    public Material highLightMaterial3;
    public Material highLightMaterial4;


    private bool isitHint = false;
    private bool isitinitialized = false;
    private bool isitCreated = false;
    private bool stopmove = false;
    private bool useHint = false;
    private bool useUndo = false;

    public string message = "Enter Your Message";
    #region Setters & Getters
    public int getWPC() {
        GameObject[] whiteCount = GameObject.FindGameObjectsWithTag("WhitePiece");
        return whiteCount.Length;
    }
    public int getBPC() {
        GameObject[] blackCount = GameObject.FindGameObjectsWithTag("BlackPiece");
        return blackCount.Length;
    }
    public void Enable() {
        runningGame = true;
    }
    public void Disable() {
        runningGame = false;
    }
    public playerTurn getTurn() {
        return this.PT;
    }
    public void SetTurn(playerTurn p) {
        this.PT = p;
    }

    #endregion

    bool createGame = true;
    #region Creating the board
    public void CreateGrid() {
        if (isitCreated)
            return;
        isitCreated = true;
        Vector3 offsetLeft = new Vector3(transform.position.x - size.x / 2 + tileSize.x / 2,
                   transform.position.y,
                   transform.position.z - size.z / 2 + tileSize.z / 2);
        Vector3 startPoint = offsetLeft;
        GameObject currentTilePrefab = tilePrefab1;
        grid = new Square[currentGame.Grid.N, currentGame.Grid.M];
        pieces = new GameObject[currentGame.Grid.N, currentGame.Grid.M];
        for (int i = 0; i < currentGame.Grid.M; i++) {
            for (int j = 0; j < currentGame.Grid.N; j++) {
                startPoint.x = offsetLeft.x + tileSize.x * i;
                startPoint.z = offsetLeft.z + tileSize.z * j;
                GameObject square = Instantiate(currentTilePrefab, startPoint, currentTilePrefab.transform.rotation) as GameObject;
                Square sq = square.AddComponent<Square>();
                sq.setCol(j);
                sq.setRow(i);
                grid[i, j] = sq;
                sq.ScaleIn(Random.Range(0f, 1f), Random.Range(1f, 2f), currentTilePrefab.transform.localScale);
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                square.transform.parent = transform;
                square.transform.localScale = Vector3.zero;
                currentTilePrefab = SwitchTilePrefab(currentTilePrefab);

            }
            currentTilePrefab = SwitchTilePrefab(currentTilePrefab);
            createGame = true;
        }
    }
    public IEnumerator SpawnPieces() {
        while (!IsReady) {
            yield return null;
        }
        FillBoard();
    }
    public void SpawnPiece(int row, int col, GameObject piece) {
        Square gridSquare = GetSquareAt(row, col);
        GameObject newPiece = Instantiate(piece, gridSquare.transform.position + Vector3.up * 0.75f, piece.transform.rotation) as GameObject;
        newPiece.transform.localScale = Vector3.zero;
        Piece pieceScript = newPiece.GetComponent(typeof(Piece)) as Piece;
        if (pieceScript)
            pieceScript.ScaleIn(Random.Range(0f, 1f), Random.Range(1f, 2f), piece.transform.localScale);
        pieces[row, col] = newPiece;

    }
    private void FillBoard() {
        foreach (Checkers.Piece wp in currentGame.WhitePieces) {
            if (wp.Type == Type.KING)
                SpawnPiece(wp.Cell.R, wp.Cell.C, whiteKing);
            else {
                SpawnPiece(wp.Cell.R, wp.Cell.C, whitePiece);
            }
        }
        foreach (Checkers.Piece wp in currentGame.BlackPieces) {
            if (wp.Type == Type.KING)
                SpawnPiece(wp.Cell.R, wp.Cell.C, blackKing);
            else
                SpawnPiece(wp.Cell.R, wp.Cell.C, blackPiece);
        }
        if (currentGame.CurrentTurn == 1)
            PT = playerTurn.BLACK;
        else
            PT = playerTurn.WHITE;
    }
    public bool IsReady {
        get {
            if (grid != null) {
                for (int row = 0; row < currentGame.Grid.N; row++) {
                    for (int col = 0; col < currentGame.Grid.M; col++) {
                        Square square = grid[row, col];
                        if (!square.IsReady()) return false;
                    }
                }
                Enable();
                return true;
            }
            else {
                return false;
            }
        }
    }
    public GameObject SwitchTilePrefab(GameObject tile) {
        if (tile == tilePrefab1)
            return tilePrefab2;
        return tilePrefab1;
    }
    public Square GetSquareAt(int row, int col) {
        if (row < 0 || row > 9 || col < 0 || col > 9)
            return null;
        return grid[row, col];
    }
    #endregion

    #region Playing Funcs
    private void UpdateMousePointer() {
        if (!Camera.main)
            return;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
            if (currentGame.CurrentTurn == 1)
                playerColor = playerTurn.WHITE;
            else
                playerColor = playerTurn.BLACK;
            if (playerColor == playerTurn.WHITE) {
                if (hit.transform.tag == "WhitePiece" || hit.transform.tag == "Board")
                    mousePointer = new Vector2(hit.point.x, hit.point.z);
                else
                    mousePointer = new Vector2(-10.0f, -10.0f);
            }
            else if (playerColor == playerTurn.BLACK) {
                if (hit.transform.tag == "BlackPiece" || hit.transform.tag == "Board")
                    mousePointer = new Vector2(hit.point.x, hit.point.z);
                else
                    mousePointer = new Vector2(-10.0f, -10.0f);
            }
        }
    }
    private void SelectMove(int x, int y) {
        if (x < 0 || x >= grid.Length || y < 0 || y >= grid.Length)
            return;
        GameObject p = pieces[x, y];
        if (p != null) {
            selectedPiece = p;
            selectedPiece.GetComponent<Piece>().setRow(x);
            selectedPiece.GetComponent<Piece>().setCol(y);
            selectedPiece.GetComponent<Piece>().SelectPiece();
            if (isitHint) {
                isitHint = false;
            }
            else {
                FirstButtonRequest fbr = new FirstButtonRequest(currentGame.Id, x, y);
                SocketManager.socket.Emit("get_possible_moves", SocketManager.ToJson(fbr));
            }
        }
    }
    private void selectAutoMove(int x, int y) {
        if (x < 0 || x >= grid.Length || y < 0 || y >= grid.Length)
            return;
        GameObject p = pieces[x, y];
        if (p != null) {
            selectedPiece = p;
            selectedPiece.GetComponent<Piece>().setRow(x);
            selectedPiece.GetComponent<Piece>().setCol(y);
            selectedPiece.GetComponent<Piece>().SelectPiece();
        }
    }
    private void ReplacePiece() {
        for (int i = 0; i < currentGame.Grid.M; i++) {
            if (pieces[currentGame.Grid.M - 1, i] != null)
                if (!pieces[currentGame.Grid.M - 1, i].GetComponent<Piece>().getIsWhite() && !pieces[currentGame.Grid.M - 1, i].GetComponent<Piece>().getIsKing()) {
                    Destroy(pieces[currentGame.Grid.M - 1, i]);
                    SpawnPiece(currentGame.Grid.M - 1, i, blackKing);
                }
            if (pieces[0, i] != null)
                if (pieces[0, i].GetComponent<Piece>().getIsWhite() && !pieces[0, i].GetComponent<Piece>().getIsKing()) {
                    Destroy(pieces[0, i]);
                    SpawnPiece(0, i, whiteKing);
                }
        }
    }
    private void SelectTarget(int x, int y) {
        if (x < 0 || x >= grid.Length || y < 0 || y >= grid.Length)
            return;
        if (pieces[x, y] == null)
            selectedTarget = grid[x, y];
        else
            selectedPiece.GetComponent<Piece>().Shake();
    }

    private void UpdateGridAfterMove(int row, int col) {
        if (selectedPiece.GetComponent<Piece>().getRow() == row && selectedPiece.GetComponent<Piece>().getCol() == col)
            return;
        else {
            pieces[row, col] = selectedPiece;
            pieces[selectedPiece.GetComponent<Piece>().getRow(), selectedPiece.GetComponent<Piece>().getCol()] = null;
        }
    }
    private void HighLightingWhilePointerMove(int row, int col) {
        if (!coords.Check(row, col,currentGame.Mode))
            return;
        if (previousSquare.x != row || previousSquare.y != col) {

            if (pieces[row, col] != null && grid[row, col].getMat() != highLightMaterial2 && grid[previousSquare.x, previousSquare.y].getMat() != highLightMaterial2) {
                if (grid[previousSquare.x, previousSquare.y].getMat() != highLightMaterial3) {
                    grid[previousSquare.x, previousSquare.y].UnHighLightSquare();
                    grid[row, col].HighLightMove(highLightMaterial3);
                    previousSquare = new Vector2Int(row, col);
                }
            }
        }

    }
    public void Capture(GameObject PieceToMove, List<Square> listOfMoves) {
        if (selectedPiece != null) {
            GameObject p = selectedPiece;
            p.GetComponent<Piece>().MaximumEatMove(listOfMoves);
            selectedPiece.GetComponent<Piece>().UnHighLightSquares();
            UpdateGridAfterMove(listOfMoves[listOfMoves.Count - 1].getRow(), listOfMoves[listOfMoves.Count - 1].getCol());
            selectedPiece = null;
            selectedTarget = null;
            stopmove = false;
        }

    }
    public void AutoMove(GameObject PieceToMove, List<Square> listOfMoves) {
        if (selectedPiece != null) {
            GameObject p = selectedPiece;
            p.GetComponent<Piece>().MaximumEatMove(listOfMoves);
            UpdateGridAfterMove(listOfMoves[listOfMoves.Count - 1].getRow(), listOfMoves[listOfMoves.Count - 1].getCol());
            selectedPiece = null;
            selectedTarget = null;
            stopmove = false;
        }

    }
    public List<List<int>> ConvertS2Ps(List<Square> list) {
        if (selectedPiece != null) {
            List<List<int>> res = new List<List<int>>();
            foreach (Square square in list) {
                List<int> sq = new List<int>();
                sq.Add(square.row);
                sq.Add(square.col);
                res.Add(new List<int>(sq));
            }
            return res;
        }
        return null;
    }
    private List<Square> ConvertA2S(List<Action> list) {
        List<Square> res = new List<Square>();
        res.Add(grid[list[0].Src.R, list[0].Src.C]);
        foreach (Action action in list)
            res.Add(grid[action.Dst.R, action.Dst.C]);
        return res;
    }
    private void MakeDrop() {
        if (selectedPiece != null) {
            selectedPiece.GetComponent<Piece>().Drop();
            selectedPiece.GetComponent<Piece>().UnHighLightSquares();
            selectedPiece = null;
            selectedTarget = null;
        }
    }
    private void MakeSelect(int x, int y) {
        if (selectedPiece == null)
            SelectMove(x, y);
    }

    private bool initGame = true;

    #endregion

    #region Responses
    private void NewGameResponse(SocketIO.SocketIOEvent E) {
        print(E.data.ToString());
        GameResult result = JsonConvert.DeserializeObject<GameResult>(E.data.ToString());
        if (result.State) {
            currentGame = result.Game;
            tileSize = tilePrefab1.GetComponent<Renderer>().bounds.size;
            size = new Vector3(tileSize.x * 10, tileSize.y, tileSize.z * 10);
            CreateGrid();
        }
    }
    private void InitGameResponse(SocketIO.SocketIOEvent E) {
        if (!isitinitialized) {
            GameResult result = JsonConvert.DeserializeObject<GameResult>(E.data.ToString());
            if (result.State) {
                currentGame = result.Game;
                StartCoroutine(SpawnPieces());
            }
            isitinitialized = true;
        }
    }
    private void FindMovesResponse(SocketIO.SocketIOEvent E) {
        print(E.data.ToString());
        MovesResult result = JsonConvert.DeserializeObject<MovesResult>(E.data.ToString());
        if (result.State) {
 
            selectedPiece.GetComponent<Piece>().FindAllPossibleMoves(result.Paths, pieces, grid, highLightMaterial1, highLightMaterial2, highLightMaterial4);
        }
    }
    private void ApplyMoveResponse(SocketIO.SocketIOEvent E) {
        ApplyResult result = JsonConvert.DeserializeObject<ApplyResult>(E.data.ToString());
        if (result.State) {
            if (selectedPiece == null) {
                selectAutoMove(result.path[0].Src.R, result.path[0].Src.C);
                AutoMove(pieces[result.path[0].Src.R, result.path[0].Src.C], ConvertA2S(result.path));
            }
            else {
                Capture(pieces[result.path[0].Src.R, result.path[0].Src.C], ConvertA2S(result.path));
            }
        }

    }
    private bool flip_camera = false;
    private void UpdateGameResponse(SocketIO.SocketIOEvent E) {
        GameResult result = JsonConvert.DeserializeObject<GameResult>(E.data.ToString());
        if (result.State) {

            int old_turn = currentGame.CurrentTurn;
            currentGame = result.Game;
            if (currentGame.CurrentTurn != old_turn)
                flip_camera = true;
        }
    }
    #endregion
    private void HintResponse(SocketIO.SocketIOEvent E) {
        ApplyResult result = JsonConvert.DeserializeObject<ApplyResult>(E.data.ToString());
        if (result.State) {
            isitHint = true;
            MakeSelect(result.path[0].Src.R, result.path[0].Src.C);
            List<List<Action>> send = new List<List<Action>>();
            send.Add(result.path);
            selectedPiece.GetComponent<Piece>().FindAllPossibleMoves(send, pieces, grid, highLightMaterial1, highLightMaterial2, highLightMaterial4);
            //Capture(selectedPiece, ConvertA2S(result.path));
        }
    }
    private void UndoResponse(SocketIO.SocketIOEvent E) {
        GameResult result = JsonConvert.DeserializeObject<GameResult>(E.data.ToString());
        if (result.State) {
            foreach (GameObject piece in pieces) {
                Destroy(piece);
            }
            currentGame = result.Game;
            StartCoroutine(SpawnPieces());
        }

    }
    private void messageResponse(SocketIO.SocketIOEvent E) {
        MessageResult result = JsonConvert.DeserializeObject<MessageResult>(E.data.ToString());
        if (result.State) {
            message = result.msg;
        }
    }

    public void GameOverResponse(SocketIO.SocketIOEvent E) {
        PlayerResult rr = JsonConvert.DeserializeObject<PlayerResult>(E.data.ToString());
        gameOver.SetActive(true);
        gameOver.transform.GetChild(1).GetComponent<Text>().text = rr.Player.Name + " wins";
    }

    private void PointOfViewResponse(SocketIO.SocketIOEvent E) {
        print(E.data.ToString());
        POVResult rr = JsonConvert.DeserializeObject<POVResult>(E.data.ToString());
        print("pov response");
        if (rr.State) {
            if (rr.POV == 2) {
                print("playing as black");
                Camera.main.GetComponent<CameraController>().whiteTurn();
            }
            else {
                Camera.main.GetComponent<CameraController>().BlackTurn();
                print("playing as white");
            }
            tileSize = tilePrefab1.GetComponent<Renderer>().bounds.size;
            size = new Vector3(tileSize.x * 10, tileSize.y, tileSize.z * 10);
            CreateGrid();
        }
    }

    void Start() {
        //SocketManager.socket.On("create_new_game", NewGameResponse);
        SocketManager.socket.On("initialize_game", InitGameResponse);
        SocketManager.socket.On("get_possible_moves", FindMovesResponse);
        SocketManager.socket.On("apply_path", ApplyMoveResponse);
        SocketManager.socket.On("update_game", UpdateGameResponse);
        SocketManager.socket.On("hint", HintResponse);
        SocketManager.socket.On("undo", UndoResponse);
        SocketManager.socket.On("chat", messageResponse);
        SocketManager.socket.On("game_over", GameOverResponse);
        SocketManager.socket.On("save_result", SaveResponse);
        SocketManager.socket.On("pov", PointOfViewResponse);
        if(currentGame.Level == Level.HUMAN)
            SocketManager.socket.Emit("pov", SocketManager.ToJson(new InitRequest() { Id = currentGame.Id }));
        else {
            tileSize = tilePrefab1.GetComponent<Renderer>().bounds.size;
            size = new Vector3(tileSize.x * 10, tileSize.y, tileSize.z * 10);
            CreateGrid();
            if (currentGame.WhitePieces.Count != 0) {
                StartCoroutine(SpawnPieces());
                isitinitialized = true;
            }
        }
    }
    void SaveResponse(SocketIO.SocketIOEvent E) {
        Result rr = JsonConvert.DeserializeObject<Result>(E.data.ToString());
        Debug.Log(rr.Message);
    }
    void Update() {
        if (currentGame.WhitePieces.Count == 0) {
            if (initGame && isitCreated) {
                if (currentGame.Player2 != null) {
                    InitRequest request = new InitRequest() { Id = currentGame.Id };
                    SocketManager.socket.Emit("initialize_game", SocketManager.ToJson(request));
                    initGame = false;
                }
            }
        }
        if (useHint) {
            InitRequest id = new InitRequest() { Id = currentGame.Id };
            SocketManager.socket.Emit("hint", SocketManager.ToJson(id));
            useHint = false;
        }
        if (useUndo) {
            InitRequest id = new InitRequest() { Id = currentGame.Id };
            SocketManager.socket.Emit("undo", SocketManager.ToJson(id));
            useUndo = false;
        }
    }

    void FixedUpdate() {
        if (runningGame) {
            UpdateMousePointer();
            int x = coords.GetRightBoardCoords(mousePointer.x);
            int y = coords.GetRightBoardCoords(mousePointer.y);
            HighLightingWhilePointerMove(x, y);

            if (Input.GetMouseButtonDown(0)) {
                if (selectedPiece != null) {
                    SelectTarget(x, y);
                }
            }
            if (Input.GetMouseButtonDown(0)) {
                MakeSelect(x, y);
            }

            if (Input.GetMouseButtonDown(1)) {
                MakeDrop();
            }

            if (selectedTarget != null && selectedPiece != null && !stopmove) {
                List<Square> listofsq = new List<Square>();
                Piece p = selectedPiece.GetComponent<Piece>();
                listofsq.Add(grid[p.getRow(), p.getCol()]);
                if (selectedPiece.GetComponent<Piece>().possibleMoves.checkTarget(selectedTarget)) {
                    foreach (Square sq in selectedPiece.GetComponent<Piece>().possibleMoves.GetPathbyTarget(selectedTarget)) {
                        listofsq.Add(sq);
                    }
                    SecondButtonRequest sbr = new SecondButtonRequest(ConvertS2Ps(listofsq), currentGame.Id);
                    SocketManager.socket.Emit("apply_action", SocketManager.ToJson(sbr));
                    stopmove = true;
                }
                else {
                    selectedPiece.GetComponent<Piece>().Shake();
                    selectedTarget = null;
                }
            }
        }

    }
    public void Hint() {
        if(!useHint && selectedPiece == null)
            useHint = true;
    }
    public void Undo() {
        useUndo = true;
    }
    public void Msg(string msg) {
        SocketManager.socket.Emit("chat", SocketManager.ToJson(new Message() { Id = currentGame.Id, msg = msg}));
    }
    private IEnumerator Wait() {

        yield return new WaitForSeconds(0.7f);
        Camera.main.GetComponent<CameraController>().switchplayer();
        flip_camera = false;
    }
    private void OnCollisionEnter(Collision collision) {
        ReplacePiece();
        if (flip_camera) {
            if(currentGame.Level == Level.OFFLINE)
                StartCoroutine("Wait");
        }
    }
}
