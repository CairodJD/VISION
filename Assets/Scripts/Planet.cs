using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Planet : MonoBehaviour {

    public Transform cellHolder;
    


    PlanetCell[] cells;

    public List<PlanetName> PossibleNames;

    public event Action<string> PlanetFound;

    Dictionary<CellType, float> cellTypePercent = new Dictionary<CellType, float>() {
        {CellType.Base, 0 },
        {CellType.Eau, 0 },
        {CellType.Gaz, 0 },
        {CellType.Terre, 0 },
    };

   
    //[Range(0.58f,0.59f)]
    public float rayon;
    public Transform Pivot;
    public float rotationSpeed = 1;

    public Vision vision;

    [HideInInspector] public string hiddenName;
    private int cellEauCount = 0 ;
    private int cellGazCount = 0 ;
    private int cellTerreCount = 0 ;

    [Range(5,50)]
    public float incertitude = 10f;

    public Animator resetanimator;


    private void Awake() {
        Init();
    }


    private void Update() {
        if (GameManager.gamestarted) {
            Pivot.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Return)) {

                RaycastHit cell = HexCellByRC();
                Paint2(cell, rayon, CellType.Eau, vision.GetTexture(20));
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                ResetPlanet();
            }
        }

        
    }

    public void Init() {
        //instciate cells 
        cells = new PlanetCell[cellHolder.childCount];
        for (int i = 0; i < cells.Length; i++) {
            PlanetCell pCell =cellHolder.GetChild(i).gameObject.AddComponent<PlanetCell>().Set(cellHolder.GetChild(i).gameObject, CellType.Base);
            cells[i] = pCell;
        }
        
    }

  

    public void Paint2(RaycastHit source, float rayon, CellType Type, Texture2D frmTxt) {
        //Debug.Log(source.point);
        Collider[] hits = Physics.OverlapSphere(source.point, rayon, 1 << 8);
        //retirer les cells deja coloré M
        List<GameObject> toColor = hits.Where(
            a => a.GetComponent<PlanetCell>().coloried == false)
            .Select(x => x.gameObject).ToList();

        foreach (GameObject item in toColor) {
            item.GetComponent<PlanetCell>().Flag();
        }
        //Debug.Log(toColor.Count);
        int c = 0;

        for (int i = 0; i < frmTxt.width; i++) {
            for (int j = 0; j < frmTxt.height; j++) {
                if (c < toColor.Count) {
                    Color pcolor = frmTxt.GetPixel(i, j);
                    PlanetCell cell = toColor[c].GetComponent<PlanetCell>();
                    
                    cell.setCellType(pcolor);
                    //Debug.Log(cell.cellType);
                    UpdateTypeCount(cell.cellType);
                    toColor[c].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", pcolor);
                    //this works only with standard materials , we're working with LWRP shaders
                    //toColor[c].GetComponent<MeshRenderer>().sharedMaterial.color = pcolor;
                    c++;
                }
                
            }
        }


        if (DoneColoring()) {
            Debug.Log("Done coloring cells");
            //Update cellTypePercent dictionary
            UpdateCellTypeDico();
            //check names 
            hiddenName = getHiddenName();
            if (hiddenName != null && hiddenName.Length > 0) {
                Debug.Log("NAME FOUND : "+hiddenName);
                if (PlanetFound != null) {
                    PlanetFound(hiddenName);
                }
            }
            showCellTypePercent();
        }



    }


    private void showCellTypePercent() {
        Debug.Log(" Eau : " + cellTypePercent[CellType.Eau]);
        Debug.Log(" gaz : " + cellTypePercent[CellType.Gaz]);
        Debug.Log(" terre : " + cellTypePercent[CellType.Terre]);
    }

    private void UpdateCellTypeDico() {
        //Debug.Log("max " + cells.Length);
        //Debug.Log("cellEauCount " + cellEauCount + " % "+ (float)cellEauCount / cells.Length);
        //Debug.Log("cellGazCount " + cellGazCount + " % " + (float)cellGazCount / cells.Length);
        //Debug.Log("cellTerreCount " + cellTerreCount + " % " + (float)cellTerreCount / cells.Length);
        cellTypePercent[CellType.Eau] = ((float) cellEauCount / cells.Length) * 100;
        cellTypePercent[CellType.Gaz] = ((float) cellGazCount / cells.Length )* 100;
        cellTypePercent[CellType.Terre] = ((float) cellTerreCount / cells.Length) * 100;
    }

    //verifier si la majorité des cell on été colorisé genre + de 95 pc
    private bool DoneColoring(float seuil = 0.95f) {
        float percentColoried= (float)cells.Where(x => x.coloried == true).Count() / cells.Length;
        if (percentColoried >= seuil) {
            return true;
        }
        return false;
    }
    private void UpdateTypeCount(CellType type) {
        //if (type == CellType.Base) {
        //    Debug.Log("why thou");
        //}
        switch (type) {
            
            case CellType.Eau:
                cellEauCount++;
                break;
            case CellType.Terre:
                cellTerreCount++;
                break;
            case CellType.Gaz:
                cellGazCount++;
                break;

        }
    }

    public void ResetPlanet() {
        //joueur l'anim
        resetanimator.SetTrigger("Reset");

        cellEauCount = 0;
        cellGazCount = 0;
        cellTerreCount = 0;

        cellTypePercent[CellType.Eau] = 0;
        cellTypePercent[CellType.Gaz] = 0;
        cellTypePercent[CellType.Terre] = 0;

        for (int i = 0; i < cells.Length; i++) {
            cells[i].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.white);
            cells[i].UnFlag();
        }
    }


    //On sort les hex cell avec leur id
    public int IndexByName(GameObject hex) {
        int id = -1;
        //361 c'est l'id du premier hex 
        id = int.Parse(hex.name.Split('_')[1]) - 361;

        return id;
    }

    //placer une raycast au milieu de la sphere et on clik lancer un rayon sur la sphere et 
    //retourner la cell toucher
    public RaycastHit HexCellByRC() {
        RaycastHit hit;
        if (Physics.Raycast(Vector3.zero, Vector3.forward, out hit, Mathf.Infinity, 1 << 8)) {
            
            //Debug.Log(hit.collider.gameObject.name);
            //Debug.Log(IndexByName(hit.collider.gameObject));
            //Debug.Log(cells[IndexByName(hit.collider.gameObject)].cellObject.name);
            return hit;
        }
        return hit;
    }
    //check if match any planetNam scritableobject
    public string getHiddenName() {
        foreach (PlanetName item in PossibleNames) {
            if (Match(item)) {
                string toreturn = item.hiddenName;
                PossibleNames.Remove(item);
                return toreturn;
            }
        }
        return null;
    }

    //verifie si la config passé corespond
    private bool Match(PlanetName planet) {

        if ( Range(cellTypePercent[CellType.Eau],planet.config[1], incertitude) &&
            Range(cellTypePercent[CellType.Gaz], planet.config[2], incertitude) &&
            Range(cellTypePercent[CellType.Terre], planet.config[3], incertitude)  ) {
            return true;
        }
        return false;
    }

    bool Range(float tocheck ,float with,float incertitude = 5f) {
        float bot = with - incertitude;
        float top = with + incertitude;
        if (tocheck >= bot && tocheck <= top) {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, rayon);
    }

}

public class PlanetCell : MonoBehaviour{
    public GameObject cellObject;
    public CellType cellType;
    public bool coloried = false;

    public PlanetCell Set(GameObject cellObject, CellType cellType, bool coloried = false) {
        this.cellObject = cellObject;
        cellObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.white);
        this.coloried = coloried;
        return this;
    }

    public void setCellType(Color color) {
        float h, s, v= 0;
        Color.RGBToHSV(color, out h, out s,out v);
        float scaledH = h * 360;
        float scaledv = v * 100;
        //GAZ si le v value est entre 85 - 100
        if (scaledv >= 85) {
            cellType = CellType.Gaz;
            return;
        } else if (scaledH >= 0 && scaledH <= 75) {//brown si le h value est entre 20-75 terre C  sur
            //Yellow si le h value est entre 45-75 terre ossi ?
            cellType = CellType.Terre;
        } else if (scaledH > 75 && scaledH <= 160) {//Green si le h value est entre 100-160 d herde ou dla tere osef 
            cellType = CellType.Terre;
        } else if (scaledH >160 && scaledH <= 250) { //Eau si le h value est entre 175-225 claire de l'eau 
            cellType = CellType.Eau;
        } else {
            // couleur avec un h > 250 = couleur funny donc du gaz?
            cellType = CellType.Gaz;
        }
    }

    public void Flag() {
        coloried = true;
    }
    public void UnFlag() {
        coloried = false;
    }
    
}

public enum CellType {
    Base = 0,
    Eau  = 1,
    Terre  = 2,
    Gaz  = 3,
}

