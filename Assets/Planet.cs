using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    public Transform cellHolder;

    List<PlanetCell> cells;
   

    public void Init() {
        //instciate cells 
        cells = new List<PlanetCell>(cellHolder.childCount);
        for (int i = 0; i < cells.Count; i++) {
            cells[i] = new PlanetCell( cellHolder.GetChild(i).gameObject, CellType.Base);
        }
    }


    public void Paint(PlanetCell source,float rayon,CellType Type) {

    }

    //On sort les hex avec leur id
    public int IndexByName(GameObject hex) {
        int id = -1;
        //361 c'est l'id du premier hex 
        id = int.Parse(hex.name.Split('_')[1]) - 361;

        return id;
    }

    //placer une raycast au milieu de la sphere et on clik lancer un rayon sur la sphere et ret
    //retourner la cell toucher
    public PlanetCell HexCellByRC() {
        return null;
    }
}

public class PlanetCell {
    GameObject cellObject;
    CellType cellType;
    bool coloried = false;

    public PlanetCell(GameObject cellObject, CellType cellType, bool coloried = false) {
        this.cellObject = cellObject;
        setCellType(cellType);
        this.coloried = coloried;
    }

    public void setCellType(CellType _type) {
        Material mat = null;
        switch (cellType) {
            case CellType.Base:
                mat = new Material(Shader.Find("Standard"));
                break;
            case CellType.Eau:
                Debug.Log("c'est de leau");
                break;
            case CellType.Terre:
                Debug.Log("c'est de la terre");
                break;
            case CellType.Gaz:
                Debug.Log("c'est du gaz");
                break;
            default:
                break;
        }

        setMaterial(mat);
    }
    //set the marial color of this cell
    public void setMaterial(Material material) {
        cellObject.GetComponent<MeshRenderer>().sharedMaterial = material;
    }
}

public enum CellType {
    Base = 0,
    Eau  = 1,
    Terre  = 2,
    Gaz  = 3,
}