using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingControl : MonoBehaviour
{
    private List<Square> FinalTargets = new List<Square>();
    private List<List<Square>> AllPossiblePaths = new List<List<Square>>();
    private List<List<Square>> AllPossiblePathsEmptyOnly = new List<List<Square>>();

    public List<Square> GetAllFinalTargets()
    {
        return this.FinalTargets;
    }
    
    public void AddPossiblePath(List<Square> list)
    {
        AllPossiblePaths.Add(list);
        this.FinalTargets.Add(list[list.Count - 1]);
        List<Square> emptyPath = new List<Square>();
        for(int i = 0; i < list.Count-1; i++)
            emptyPath.Add(list[i]);
        AllPossiblePathsEmptyOnly.Add(emptyPath);
    }
    public List<List<Square>> GetAllPathsEmpty()
    {
        return this.AllPossiblePathsEmptyOnly;
    }
    public List<Square> GetPathbyTarget(Square square) {
        for(int i= 0; i < FinalTargets.Count; i++) {
            if (FinalTargets[i] == square)
                return AllPossiblePaths[i];
        }
        return null;
    }
    public bool checkTarget(Square square)
    {
        for (int i = 0; i < FinalTargets.Count; i++)
        {
            if (FinalTargets[i] == square)
                return true;
        }
        return false;
    }

}
