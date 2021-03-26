using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : PrefabsController
{
    public int row;
    public int col;
    protected Material highLight;
    public Square(int row, int col)
    {
        this.row = row;
        this.col = col;
    }
    public void setRow(int row)
    {
        this.row = row;
    }
    public void setCol(int col)
    {
        this.col = col;
    }
    public int getRow()
    {
        return row;
    }
    public int getCol()
    {
        return col;
    }
    public Material getMat()
    {
        return renderer.material;
    }
    public void HighLightMove(Material material)
    {
        if (GetComponent<Renderer>().material != tileMaterial)
            return;
        highLight = material;
        setMaterial(material);
    }

    public override string ToString()
    {
        return "" + row + "x" + col;
    }

}
