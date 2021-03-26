using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Checkers;
public class ButtonManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject board;
    public GameObject inputField;
    public GameObject gameOverUI; 
    private string msg;
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        gameIsPaused = true;

    }
    public void Settings()
    {
        //        SceneManager.LoadScene("StartTesting");
    }
    public void Save()
    {
        SocketManager.socket.Emit("save", SocketManager.ToJson(new InitRequest() { Id = BoardController.currentGame.Id}));
    }
    public void Exit()
    {
        print("emiting leave");
        SocketManager.socket.Emit("leave", SocketManager.ToJson(new InitRequest() { Id = BoardController.currentGame.Id }));
        SceneManager.LoadScene("Option");
    }
    public void Retreat()
    {
        board.GetComponent<BoardController>().Undo();
    }
    public void Hint()
    {
        board.GetComponent<BoardController>().Hint();
    }
    public void UpdateMSG()
    {
        msg = inputField.GetComponent<InputField>().text;
        board.GetComponent<BoardController>().Msg(msg);
        inputField.GetComponent<InputField>().text = "";
    }
    public void Back()
    {
        SceneManager.LoadScene("Option");
    }
    public void GameOver()
    {
        inputField.SetActive(false);
        gameOverUI.SetActive(true);
    }
    void Update()
    {
       inputField.GetComponentInChildren<Text>().text =board.GetComponent<BoardController>().message;
    }
}
