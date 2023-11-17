using cakeslice;
using System.Collections;
using UnityEngine;
using TMPro;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
//using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using CoralReef;
//using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class GameControl : MonoBehaviour//, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //public GameObject txt_InventoryCoralCount;

    public GameObject progressLoading;
    public GameObject scrollView;
    public GameObject txtAllCoralCount;
    public ScrollRect scrollRect;
    public GameObject btnBuy;
    public GameObject btnMusic;
    public GameObject btnInventory;
    public GameObject btnRightEnable;
    public GameObject btnLeftEnable;
    public GameObject btnLogin;
    
    public GameObject[] coralObjType;
    public GameObject[] coralLblType;
    
    private GameObject curSelObj;
    private bool isEditorMode = false;
    private bool isUpdated = false;
    [HideInInspector]
    public bool isUserlogin = false;
    private bool isInventory = false;

    [HideInInspector]
    public Animator aniInventory;

    private Vector3 prevMousePos = Vector3.zero;
    private float contentMaskWidth;
    private float buttonCoralWidth;
    private float contentWidth;
    
    private const float cRotationSpeed = 4f;
    private const float cScaleSpeed = 0.25f;
    
    private OwnerInfo baseOwnerInfo;
    [HideInInspector]
    public Dictionary<string, CoralInfo> dicCoral_Whole;
    private List<GameObject> lstCoralLblTypes = new List<GameObject>();
    [HideInInspector]
    public int inventoryCount;

    public static GameControl instance = null;
    
    

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isEditorMode = false;
        string collectionPath = "/reef_id/";
        FirebaseFirestore.ListenForDocumentChange(collectionPath, firebaselogin.instance.reef_ID, true, gameObject.name, "IDStateChangedCallback", "DisplayErrorObject");

        FirebaseFirestore.GetDocumentsInCollection(firebaselogin.instance.collectionPath_reef, gameObject.name, "PlaceAll", "PlaceAllMyDataObject");
        lstCoralLblTypes = coralLblType.ToList();

        Debug.Log("collectionPath_reef = " + firebaselogin.instance.collectionPath_reef);

        aniInventory = scrollView.GetComponent<Animator>();
        aniInventory.SetInteger("inventoryState", 5);

    }

    public void GetDocumentsInCollection() =>
           FirebaseFirestore.GetDocumentsInCollection(firebaselogin.instance.collectionPath_reef, gameObject.name, "PlaceAll", "PlaceAllMyDataObject");

    public void PlaceAll (string data)
    {        
        StartCoroutine(PlaceAllMyData(data));        
    }

    public IEnumerator PlaceAllMyData(string data)
    {
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.DepthOfField>().focalLength = 0f;
        try
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            dicCoral_Whole = JsonConvert.DeserializeObject<Dictionary<string, CoralInfo>>(data, settings);
            
            if (dicCoral_Whole.Count > 0)
            {
                progressLoading.SetActive(true);
            }
            //txt_InventoryCoralCount.SetActive(true);            
        }
        catch (Exception e)
        {
            Debug.Log("Data read error null = " + e);
        }

        int coralsCount = dicCoral_Whole.Count;
        //Debug.Log("PlayceAllData: dicCoral_Whole.Count = " + coralsCount);
        contentMaskWidth = scrollRect.viewport.rect.width;
        buttonCoralWidth = scrollRect.content.GetComponent<GridLayoutGroup>().cellSize.x + scrollRect.content.GetComponent<GridLayoutGroup>().spacing.x;

        int j = 0;
        for (int i = 0; i< coralsCount; i ++)
        {
            CoralInfo coral = dicCoral_Whole.ElementAt(i).Value;
            string keyCoral = dicCoral_Whole.ElementAt(i).Key;

            //if (coral.reef_id == firebaselogin.instance.reef_ID)
            {
                IEnumerable<GameObject> result = from corallbl in lstCoralLblTypes
                             where corallbl.GetComponent<CoralControl>().coralObj.name == coral.coralIdx    // s.Contains("Tutorials")
                             select corallbl ;
                
                if (result.ToArray().Length > 0)
                {
                    GameObject importedCoral = result.ToArray()[0];
                    int idxImportedObj = lstCoralLblTypes.IndexOf(importedCoral);
                    //Debug.Log("PlayceAllData: CoralDocId= " + keyCoral + ", coralIdx= " + coral.coralIdx + ", in_inventory = " + coral.in_inventory );
                    if (!coral.in_inventory)
                    {
                        GameObject SpawnedCoral;
                        SpawnedCoral = GameObject.Instantiate(coralObjType[idxImportedObj], new Vector3(coral.x, coral.y, coral.z), Quaternion.Euler(coral.rotX, coral.rotY, coral.rotZ));
                        SpawnedCoral.transform.localScale = new Vector3(coral.scale, coral.scale, coral.scale); 
                        SpawnedCoral.name = keyCoral;
                        SpawnedCoral.layer = 0;
                    }
                    else
                    {
                        GameObject SpawnedButton;
                        SpawnedButton = GameObject.Instantiate(coralLblType[idxImportedObj], coralLblType[idxImportedObj].transform.position, coralLblType[idxImportedObj].transform.rotation);
                        //GameObject contentsObj = GameObject.FindGameObjectWithTag("UI_Panel");
                        SpawnedButton.transform.parent = scrollRect.content.transform;// contentsObj.transform;
                        SpawnedButton.name = keyCoral;
                        SpawnedButton.transform.localScale = Vector3.one;
                        SpawnedButton.SetActive(false);

                        RectTransform rectTransform = scrollRect.content;// contentsObj.GetComponent<RectTransform>();
                        inventoryCount = j + 1;
                        contentWidth = buttonCoralWidth * inventoryCount;

                        rectTransform.sizeDelta = new Vector2(contentWidth, rectTransform.sizeDelta.y);
                        j++;
                    }                    
                }
                if (coralsCount > 0)
                {
                    txtAllCoralCount.GetComponent<TMP_Text>().text = i.ToString() + " / " + coralsCount.ToString();
                    progressLoading.GetComponent<Slider>().value = (float)i / coralsCount;
                    //txt_InventoryCoralCount.GetComponent<TMP_Text>().text = j.ToString() + " / " + coralsCount.ToString();
                }
                yield return null;                
            }
        }
        btnBuy.SetActive(true);
        btnInventory.SetActive(true);
        btnMusic.SetActive(true);

        if (dicCoral_Whole.Count > 0)
        {
            progressLoading.GetComponent<Animator>().SetBool("Fadeoff", true);            
        }
        GameObject.Destroy(progressLoading, 0.4f);
        
        GameObject[] gameObjectsInScrollView = scrollRect.content.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        //ViewContents(scrollRect, false);
        btnLogin.SetActive(false);
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.DepthOfField>().focalLength = 14f;
    }

    public void PlaceAllMyDataObject(string error)
    {
        Debug.LogError(error);
    }

    void Update()
    {
        if (!isUserlogin) return;

        if (!isEditorMode )
        {
            //Debug.Log("Not in EditMode!");
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.collider && hit.collider.gameObject.tag == "Coral")
                    {
                        curSelObj = hit.collider.gameObject;
                        isEditorMode = true;
                        curSelObj.layer = 2; // raytrace ignore;
                        curSelObj.GetComponent<cakeslice.Outline>().eraseRenderer = false;
                        prevMousePos = Input.mousePosition;
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
                //isMovingMode = true;
                curSelObj.GetComponent<cakeslice.Outline>().eraseRenderer = true;
                curSelObj.layer = 0;// Default raytrace;
                if (isUpdated)
                {
                    UpdateCoral(curSelObj);                    
                }
                return;
            }
            if (Input.GetMouseButtonUp (0) )
            {
                //isMovingMode = false;
            }
            if (Input.GetMouseButton(0))
            {
                if (prevMousePos == Input.mousePosition) return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    //Debug.Log("GameControl.Raycast: hitObj = " + hit.collider.gameObject.name + ", tag = " + hit.collider.gameObject.tag + ", this =" + this.gameObject.name);
                    if (hit.collider == curSelObj) return;
                    if (hit.collider && (hit.collider.gameObject.tag == "ground" || hit.collider.gameObject.tag == "Coral"))
                    {
                        curSelObj.transform.position = hit.point - new Vector3(0f, 0.03f, 0f);
                    }
                }
            }
            else if (Input.GetMouseButton(1))
            {
                dx = Input.GetAxis("Mouse X") * cRotationSpeed * 2f;
                curSelObj.transform.Rotate(0f, -dx, 0, Space.World );
                isUpdated = true;
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
                //curSelObj.transform.rotation = Quaternion.FromToRotation ( );
                isUpdated = true;
            }
            if (Input.GetKeyUp (KeyCode.Delete))
            {
                /*
                isEditorMode = false;
                GameObject.DestroyObject ( curSelObj );
                isUpdated = true;
                //isDeleted = true;
                return;
                */
            }
        }
    }

    private void UpdateCoral(GameObject curSelObj) //, bool _isDeleted = false
    {       
        for (int i = 0; i < dicCoral_Whole.Count; i++)
        {
            if (dicCoral_Whole.ElementAt(i).Key == curSelObj.name)
            {
                //Debug.Log(" UpdateCoral: selObjValue= " + dicCoral_Whole.ElementAt(i).Value.coralIdx + ", docId = " + dicCoral_Whole.ElementAt(i).Key );
                CoralInfo updateCoralInfo = dicCoral_Whole.ElementAt(i).Value;
                updateCoralInfo.in_inventory = false;
                updateCoralInfo.x = curSelObj.transform.position.x;
                updateCoralInfo.y = curSelObj.transform.position.y;
                updateCoralInfo.z = curSelObj.transform.position.z;
                updateCoralInfo.rotX = curSelObj.transform.eulerAngles.x;
                updateCoralInfo.rotY = curSelObj.transform.eulerAngles.y;
                updateCoralInfo.rotZ = curSelObj.transform.eulerAngles.z;
                updateCoralInfo.scale = curSelObj.transform.localScale.x;
                //if (_isDeleted)
                    //updateCoralInfo.in_inventory = true;

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                string jsonSelCoral = JsonConvert.SerializeObject(updateCoralInfo, settings);
                //Debug.Log("jsonSelCoral = " + jsonSelCoral);

                FirebaseFirestore.UpdateDocument(firebaselogin.instance.collectionPath_reef, curSelObj.name, jsonSelCoral, gameObject.name, "DisplayInfo", "DisplayErrorObject");
                
                isUpdated = false;
                //isDeleted = false;
                return;
            }
        }
    }

    public void ContentMoveRight()
    {
        contentMaskWidth = scrollRect.viewport.rect.width;
        buttonCoralWidth = scrollRect.content.GetComponent<GridLayoutGroup>().cellSize.x + scrollRect.content.GetComponent<GridLayoutGroup>().spacing.x;
        //Debug.Log("ScorllPos = " + scrollRect.content.transform.localPosition);
        if (scrollRect.content.transform.localPosition.x < 0f)
        {
            scrollRect.content.transform.localPosition += new Vector3(buttonCoralWidth, 0f, 0f);
            btnLeftEnable.SetActive(true);
        }
        else
        {
            btnRightEnable.SetActive(false);
            scrollRect.content.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
            
        //Debug.Log("NewScorllPos = " + scrollRect.content.transform.localPosition);
    }

    public void ContentMoveLeft()
    {
        contentMaskWidth = scrollRect.viewport.rect.width;
        buttonCoralWidth = scrollRect.content.GetComponent <GridLayoutGroup>().cellSize.x + scrollRect.content.GetComponent<GridLayoutGroup>().spacing.x;
        RectTransform rt = scrollRect.content.GetComponent<RectTransform>();
        contentWidth = buttonCoralWidth * inventoryCount;
        rt.sizeDelta = new Vector2(contentWidth, rt.sizeDelta.y);
        if (contentWidth - (-scrollRect.content.transform.localPosition.x) > contentMaskWidth )
        {
            scrollRect.content.transform.localPosition -= new Vector3(buttonCoralWidth, 0f, 0f);
            btnRightEnable.SetActive(true);
        }
        else
        {
            btnLeftEnable.SetActive(false);
        }
    }

    public void ShowShop()
    {
        CoralReefImportJS.ShowShopOnWebGL();
    }    

    public void ViewInventory()
    {
        isInventory = !isInventory;

        if (isInventory)
        {
            aniInventory.SetInteger("inventoryState", 1);
            if (isUserlogin)
            {
                btnLogin.SetActive(false);
                ViewContents(scrollRect, true);
            }                
            else
            {
                btnLogin.SetActive(true);
                ViewContents(scrollRect, false);
            }
        }
        else
        {
            aniInventory.SetInteger("inventoryState", 3);
            ViewContents(scrollRect, false);
        }
        //CoralReefImportJS.ShowUserLogin();        
    }

    public void ViewContents(ScrollRect scrollRect, bool isView)
    {
        GameObject[] gameObjectsInScrollView = scrollRect.content.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        foreach (GameObject obj in gameObjectsInScrollView)
        {
            obj.SetActive(isView);
        }      

    }

    public void DologinAction()
    {
        CoralReefImportJS.Dologin();
    }
    

    public void IDStateChangedCallback(string data)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        if (baseOwnerInfo == null)
        {
            try
            {
                baseOwnerInfo = JsonConvert.DeserializeObject<OwnerInfo>(data, settings);               
            }
            catch (Exception e)
            {
                Debug.Log("Data read error null = " + e);
            }
            DisplayInfo("Callback of ListenForDocumentChange:\n" + " baseline reef_assets_update= " + baseOwnerInfo.reef_assets_update
               + ", baseline reef_owner_update.seconds = " + baseOwnerInfo.reef_owner_update.seconds);
            Debug.Log("baseOwnerInfo.reef_owner_update.seconds = " + baseOwnerInfo.reef_owner_update.seconds);
        }
        else
        {
            OwnerInfo updateOwnerInfo;
            updateOwnerInfo = JsonConvert.DeserializeObject<OwnerInfo>(data, settings);

            DisplayInfo("Callback of ListenForDocumentChange:\n" + "reef_assets_update= " + updateOwnerInfo.reef_assets_update
               + ", reef_owner_update.seconds = " + updateOwnerInfo.reef_owner_update.seconds);

            if (baseOwnerInfo.reef_owner_update.seconds < updateOwnerInfo.reef_owner_update.seconds)
            {
                //aniInventory.SetInteger("inventoryState", 1);
                isUserlogin = true;
                btnLogin.SetActive(false);
                ViewContents(scrollRect, true);                
            }
        }

        isUserlogin = true;
    }

    public void DisplayInfo(string info)
    {
        //outputText.color = Color.white;
        //outputText.GetComponent<TMP_Text>().text = info;
        Debug.Log(info);
    }

    public void DisplayErrorObject(string error)
    {
        var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        //outputText.color = Color.red;
        //outputText.GetComponent<TMP_Text>().text = error;
        Debug.LogError(error);
    }
    
}

[System.Serializable]
public class OwnerInfo
{
    public int reef_assets_update;
    public string reef_owner_caps;
    public ownerUpdate reef_owner_update;
    public string reef_owner_token;
    public string reef_owner;
}

[System.Serializable]
public struct ownerUpdate
{
    public uint seconds;
    public uint nanoseconds;
}