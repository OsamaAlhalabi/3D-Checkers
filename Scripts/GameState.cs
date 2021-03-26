using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    static public bool white =true;
    static public bool international;
    static public bool loaded;
    static public bool multiplayer=true;
    static public List<Piece> LoadedGameGrid;
    public void StartNewGame()
    {
        loaded = false;
        LoadedGameGrid = new List<Piece>();
        SceneManager.LoadScene("MainGame");
    }
    public void StartLoadedGame()
    {
        loaded = true;
        SceneManager.LoadScene("MainGame");
    }
    public void BlackPlayer()
    {
        white = false;
    }
    public void WhitePlayer()
    {
        white = true;
    }
    public void InternationalGame()
    {
        international = true;
    }
    public void TurkishGame()
    {
        international = false;
    }
    public void MultiPlayer()
    {
        multiplayer = true;
    }
    public void SinglePlayer()
    {
        multiplayer = false;
    }

    public bool IsItWhiteTurn()
    {
        return white;
    }
    public bool IsItInternationalGame()
    {
        return international;
    }
    public bool IsItLoadedGame()
    {
        return loaded;
    }
    public bool IsMultiplayerMode()
    {
        return multiplayer;
    }
    public List<Piece> GetLoadedGrid()
    {
        return LoadedGameGrid;
    }
}
