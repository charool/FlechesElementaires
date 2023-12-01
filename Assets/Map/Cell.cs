using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell 
{
    public int textureIndex = 1;
    public int biomeIndex = 0;
    public int itemIndex = 0;
}
public enum CellType
{
    Cliff, Grass1, Grass2, Rock, Sand, Snow, Gravel
}
public enum BiomeType
{
    Water,GrassLand1, GrassLand2, Mountain, Canyon
}