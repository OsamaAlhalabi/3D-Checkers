using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject board;
    public Text text1;
    public Text text2;
    public Text timer;

    private float startTime;
    static public string RunningGame;

    public void SetTime()
    {
        float currentTime = Time.timeSinceLevelLoad - startTime - 2;
        string minutes = ((int)currentTime / 60).ToString();
        string seconds = (currentTime % 60).ToString("f0");
        timer.text = minutes + ":" + seconds;
    }

    void Start()
    {
        startTime = 0.0f;
    }

    void Update()
    {
        GameObject g = GameObject.Find("Board");
        Checkers.Game game = BoardController.currentGame;
        if (game != null)
        {
            int w = 0, b= 0;
            foreach (Checkers.Piece piece in game.WhitePieces)
                if (piece.Dead != 1)
                    w++;
            foreach (Checkers.Piece piece in game.BlackPieces)
                if (piece.Dead != 1)
                    b++;
            text1.text = "White Pieces\n " + w;
            text2.text = "Black Pieces\n " + b;
            if (board.GetComponent<BoardController>().IsReady)
                SetTime();

        }


    }
 
}
