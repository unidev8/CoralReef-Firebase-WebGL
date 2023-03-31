using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using System;
using static firebaselogin;


public class GameControl : MonoBehaviour//, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject[] coralObj;
    public GameObject[] coralLbl;

    private GameObject curSelObj;
    private bool isEditorMode = false;
    private const float cRotationSpeed = 4f;
    private const float cScaleSpeed = 0.25f;

    
    CoralInfo coralInfo;

    PlayerInfo playerInfo;

    public static GameControl instance = null;
    public CoralInfo[] coralReef;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isEditorMode = false;
        //FirebaseFirestore.GetDocumentsInCollection(collectionPath_reef, gameObject.name, "PlaceAll", "PlaceAllMyDataObject");
    }


    public void GetDocumentsInCollection() =>
           FirebaseFirestore.GetDocumentsInCollection(firebaselogin.instance.collectionPath_reef, gameObject.name, "PlaceAll", "PlaceAllMyDataObject");

    public void PlaceAll (string data)
    {
        StartCoroutine(PlaceAllMyData(data));
        
    }

    public IEnumerator PlaceAllMyData(string data)
    {
        //statusText.color = statusText.color == Color.green ? Color.blue : Color.green;
        //statusText.text = data;
        coralReef = JsonUtility.FromJson<CoralInfo[]>(data);
        foreach (CoralInfo coral in coralReef)
        {
            if (coral.reef_id == firebaselogin.instance.my_reef_id)
            {
                for (int i = 0; i < coralLbl.Length; i++)
                {
                    if (coral.coralIdx == coralLbl[i].name )
                    {
                        if (coral.editableState)
                        {
                            GameObject SpawnedCoral;
                            SpawnedCoral = GameObject.Instantiate(coralObj[i], new Vector3 ( coral.x, coral.y, coral.z), Quaternion.Euler (coral.rotX, coral.rotY, coral.rotZ) );
                        }
                        else
                        {
                            GameObject SpawnedButton;
                            SpawnedButton = GameObject.Instantiate(coralLbl[i], coralLbl[i].transform.position, coralLbl[i].transform.rotation);
                            SpawnedButton.transform.parent = gameObject.transform.parent;
                        }                        
                    }
                }
                yield return null;                
            }
        }
        Debug.Log(data);
    }

    public void PlaceAllMyDataObject(string error)
    {
        Debug.LogError(error);
    }


    void Update()
    {
        if (!isEditorMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Plane plane = new Plane(Vector3.up, 0);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (Input.GetMouseButtonDown(0))
                {                    
                    if (hit.collider && hit.collider.gameObject.tag == "Coral")
                    {
                        curSelObj = hit.collider.gameObject;
                        isEditorMode = !isEditorMode;
                        curSelObj.GetComponent<Outline>().eraseRenderer = !curSelObj.GetComponent<Outline>().eraseRenderer;
                        //Debug.Log("GameControl.FixedUpdate: curSelObj = " + hit.collider.gameObject.name + ", this =" + this.gameObject.name);
                        return;
                    }
                }
            }
        }
        else
        {
            float dx, dy, ds;

            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("EditMode is false!");
                isEditorMode = false;
                curSelObj.GetComponent<Outline>().eraseRenderer = true;
                return;
            }
            if (Input.GetMouseButton(1))
            {
                dx = Input.GetAxis("Mouse X") * cRotationSpeed * 2f;
                curSelObj.transform.Rotate(0f, -dx, 0, Space.World );
            }
            else
            {
                dx = Input.GetAxis("Mouse X") * cRotationSpeed;
                dy = Input.GetAxis("Mouse Y") * cRotationSpeed;
                ds = Input.mouseScrollDelta.y * cScaleSpeed;

                curSelObj.transform.RotateAround(curSelObj.transform.position, Vector3.forward, dx); //- dy, 0f, dx, Space.Self );
                curSelObj.transform.RotateAround(curSelObj.transform.position, Vector3.right, -dy); //- dy, 0f, dx, Space.Self );
                //curSelObj.transform.Rotate(-dy, 0f, dx, Space.Self);
                curSelObj.transform.localScale += new Vector3(ds, ds, ds);
                //curSelObj.transform.rotation = Quaternion.AngleAxis( (curSelObj.transform.eulerAngles.z + dx), Vector3.fwd );
                //curSelObj.transform.rotation = Quaternion.AngleAxis((curSelObj.transform.eulerAngles.x + dy), Vector3.right);
                //Debug.Log("EditMode! dx =" + dx + ", dy = " + dy);
                //curSelObj.transform.rotation = Quaternion.FromToRotation ( );
            }
            if (Input.GetKeyUp (KeyCode.Delete))
            {
                isEditorMode = false;
                GameObject.DestroyObject ( curSelObj );
                return;
            }
        }
        
        
    }

}