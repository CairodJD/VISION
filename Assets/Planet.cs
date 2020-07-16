using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Planet : MonoBehaviour {

    public Transform cellHolder;
    
    public Material matEau;
    PlanetCell[] cells;
    //[Range(0.58f,0.59f)]
    public float rayon;
    public Transform Pivot;
    public float rotationSpeed = 1;

    public Vision vision;
 
    
    private void Awake() {
        Init();
    }


    private void Update() {

        Pivot.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit cell = HexCellByRC();
            Paint2(cell, rayon, CellType.Eau, vision.GetTexture(20));
        }
    }

    public void Init() {
        //instciate cells 
        cells = new PlanetCell[cellHolder.childCount];
        for (int i = 0; i < cells.Length; i++) {
            cells[i] = new PlanetCell( cellHolder.GetChild(i).gameObject, CellType.Base);
        }
        
    }

    // point de pivot fucked trouver une soluce
    public void Paint(RaycastHit source,float rayon,CellType Type) {
        //Debug.Log(source.point);
        Collider[] hits = Physics.OverlapSphere(source.point, rayon,1<<8);
        //retirer les cells deja coloré M
        GameObject[] toColor = hits.Select(x => x.gameObject).ToArray();
        Debug.Log(toColor.Length) ;
        foreach (GameObject item in toColor) {
            //Debug.Log(item.name);
            //doesnt work
            //cells[IndexByName(item)].setCellType(CellType.Eau, matEau);  
            item.GetComponent<MeshRenderer>().sharedMaterial = matEau;
        }
    }

    public void Paint2(RaycastHit source, float rayon, CellType Type, Texture2D frmTxt) {
        //Debug.Log(source.point);
        Collider[] hits = Physics.OverlapSphere(source.point, rayon, 1 << 8);
        //retirer les cells deja coloré M
        GameObject[] toColor = hits.Select(x => x.gameObject).ToArray();
        Debug.Log(toColor.Length);
        int c = 0;
        Debug.Log(frmTxt.texelSize);
        Debug.Log(frmTxt.width + "  " + frmTxt.height);
        for (int i = 0; i < frmTxt.width; i++) {
            for (int j = 0; j < frmTxt.height; j++) {
                toColor[c].GetComponent<MeshRenderer>().sharedMaterial.color = frmTxt.GetPixel(i,j);
                c++;
            }
        }
        
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
    public RaycastHit HexCellByRC() {
        RaycastHit hit;
        if (Physics.Raycast(Vector3.zero, Vector3.forward, out hit, Mathf.Infinity, 1 << 8)) {
            
            Debug.Log(hit.collider.gameObject.name);
            //Debug.Log(IndexByName(hit.collider.gameObject));
            //Debug.Log(cells[IndexByName(hit.collider.gameObject)].cellObject.name);
            return hit;
        }
        return hit;
    }


    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, rayon);
    }

}

public class PlanetCell {
    public GameObject cellObject;
    public CellType cellType;
    public bool coloried = false;

    public PlanetCell(GameObject cellObject, CellType cellType, bool coloried = false) {
        this.cellObject = cellObject;
        setCellType(cellType);
        this.coloried = coloried;
    }

    public void setCellType(CellType _type , Material newmat = null) {
        Material mat = newmat;
        switch (cellType) {
            case CellType.Base:
                mat = new Material(Shader.Find("Standard"));
                break;
            case CellType.Eau:
                Debug.Log("c'est de leau");
                mat = newmat;
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

    public void Flag() {
        coloried = true;
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