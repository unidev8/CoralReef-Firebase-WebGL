using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using FirebaseWebGL.Scripts.FirebaseBridge;
using UnityEditor;
using System.Linq;
//using Newtonsoft.Json;
using System.Collections.Generic;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.Objects;
using UnityEngine.UIElements;
using UnityEngine.UI;

namespace CoralReef
{
    public class CoralControl : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler  
    {
        public GameObject coralObj;
        private GameObject SpawnedCoral;
        
        private bool isSelected = false;
        private bool isPlanted = true;
        private Material originalMat;
        private Animator aniCoralButton;
       
        CoralInfo coralInfo;



        private void Start()
        {
            isSelected = false;
            isPlanted = true; // Should be like this
            aniCoralButton = gameObject.GetComponent<Animator>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isPlanted) return;

            //aniCoralButton.SetBool("selCoralInventory", true);

            isSelected = true;
            isPlanted = false;
            Vector3 firstPos = Vector3.zero;
            SpawnedCoral = GameObject.Instantiate(coralObj, firstPos, Quaternion.identity); // Generate Random Rotation
            SpawnedCoral.name = this.gameObject.name;
            
            Renderer objectRenderer = SpawnedCoral.GetComponent<MeshRenderer>();
            originalMat = objectRenderer.material;// child.GetComponent<Material>();
            objectRenderer.material = Resources.Load("Force Field", typeof(Material)) as Material;
            
            //objectRenderer.material.SetFloat("FresnelPower", frenselPower);
            //Debug.Log("SetWinWalk: frenselPower = " + frenselPower + ", objectRenderer" + objectRenderer.material);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isSelected  && !isPlanted)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, 100))
                {
                    //Debug.Log("CoalControl.Raycst: hitObj = " + hit.collider.gameObject.name + ", tag = " + hit.collider.gameObject.tag + ", this =" + this.gameObject.name);
                    if (hit.collider && ( hit.collider.gameObject.tag == "ground"  || hit.collider.gameObject.tag == "Coral"))
                    {
                        SpawnedCoral.transform.position = hit.point - new Vector3 (0f, 0.1f, 0f);
                        if (SpawnedCoral)
                        {
                            GameControl.instance.aniInventory.SetInteger("inventoryState", 3);
                            //GameControl.instance.ViewContents(GameControl.instance.scrollRect, false);
                            GameObject[] gameObjectsInScrollView = GameControl.instance.scrollRect.content.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
                            foreach (GameObject obj in gameObjectsInScrollView)
                            {
                                obj.transform.localScale = Vector3.zero;
                            }
                        }                        
                    }                        
                }               
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {            
            isPlanted = true;
            SpawnedCoral.GetComponent<MeshRenderer>().material = originalMat;
            SpawnedCoral.layer = 0;
            //Debug.Log ("SelCoralObjID= " + this.gameObject.name);
/*
            CoralInfo updateCoralInfo = GameControl.instance.dicCoral_Inventory[gameObject.name];
            Debug.Log(" selObjValue= " + updateCoralInfo.coralIdx);
            updateCoralInfo.in_inventory = false;
            updateCoralInfo.x = SpawnedCoral.transform.position.x;
            updateCoralInfo.y = SpawnedCoral.transform.position.y;
            updateCoralInfo.z = SpawnedCoral.transform.position.z;
            updateCoralInfo.scale = SpawnedCoral.transform.localScale.x;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            string jsonSelCoral = JsonConvert.SerializeObject(updateCoralInfo, settings);
            Debug.Log("jsonSelCoral = " + jsonSelCoral);

            FirebaseFirestore.UpdateDocument(firebaselogin.instance.collectionPath_reef, gameObject.name, jsonSelCoral, gameObject.name, "DisplayInfo", "DisplayErrorObject");
*/
            //Debug.Log("dicCoral_Inventory.count = " + GameControl.instance.dicCoral_Whole.Count);
            
            for (int i = 0; i < GameControl.instance.dicCoral_Whole.Count; i ++)
            {
                //Debug.Log("Key = " + GameControl.instance.dicCoral_Whole.ElementAt(i).Key);
                if (GameControl.instance.dicCoral_Whole.ElementAt (i).Key == gameObject.name)
                {
                    //Debug.Log(" selObjValue= " + GameControl.instance.dicCoral_Whole.ElementAt(i).Value.coralIdx);
                    CoralInfo updateCoralInfo = GameControl.instance.dicCoral_Whole.ElementAt(i).Value;
                    updateCoralInfo.in_inventory = false;
                    updateCoralInfo.x = SpawnedCoral.transform.position.x;
                    updateCoralInfo.y = SpawnedCoral.transform.position.y;
                    updateCoralInfo.z = SpawnedCoral.transform.position.z;
                    updateCoralInfo.rotX = SpawnedCoral.transform.rotation.eulerAngles.x;
                    updateCoralInfo.rotY = SpawnedCoral.transform.rotation.eulerAngles.y;
                    updateCoralInfo.rotZ = SpawnedCoral.transform.rotation.eulerAngles.z;
                    updateCoralInfo.scale = SpawnedCoral.transform.localScale.x;

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    string jsonSelCoral = JsonConvert.SerializeObject(updateCoralInfo, settings);
                    //Debug.Log("jsonSelCoral = " + jsonSelCoral);
                    FirebaseFirestore.UpdateDocument(firebaselogin.instance.collectionPath_reef, gameObject.name, jsonSelCoral, GameControl.instance.gameObject.name, "DisplayInfo", "DisplayErrorObject");

                    GameControl.instance.aniInventory.SetInteger("inventoryState", 1);
                    //GameControl.instance.ViewContents(GameControl.instance.scrollRect, true);
                    GameObject[] gameObjectsInScrollView = GameControl.instance.scrollRect.content.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
                    foreach (GameObject obj in gameObjectsInScrollView)
                    {
                        obj.transform.localScale = Vector3.one;
                    }
                    GameObject.Destroy(gameObject, 0.2f);
                    GameControl.instance.inventoryCount -= 1;
                    return;
                }
            }                      
        }

        public void DisplayInfo(string info)
        {
            //outputText.color = Color.white;
            //outputText.text = info;
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
            //outputText.text = error;
            Debug.LogError(error);
        }



        /*       public void OnBeginDrag(PointerEventData eventData)
               {
                   if (!SpawnedButton)
                   {
                       SpawnedButton = GameObject.Instantiate(lblObj, this.transform.position, this.transform.rotation);                
                       SpawnedButton.transform.parent = gameObject.transform.parent;
                       SpawnedButton.transform.localScale = Vector3.one;
                       Debug.Log("this.parent is " + gameObject.transform.parent.name);
                   }
                   Debug.Log("Control_BeginDrag: curPos = " + eventData.position );
                   lastMousePosition = eventData.position;
               }

               public void OnDrag (PointerEventData eventData)
               {
                   Vector2 currentPosition = eventData.position;
                   Vector2 diff = currentPosition - lastMousePosition;
                   RectTransform rect = GetComponent<RectTransform>();

                   Vector3 newPositon = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
                   Vector3 oldPos = rect.position;
                   rect.position = newPositon;
                   rect.position = newPositon;

                   if (!IsRectTransformInsideSreen(rect))
                   {
                       rect.position = oldPos;
                   }
                   lastMousePosition = currentPosition;
                   //Debug.Log("Control_OnDrag: curPos" + eventData.position);
               }

               public void OnEndDrag (PointerEventData eventData)
               {
                   if (SpawnedButton)
                   {
                       GameObject.Destroy(gameObject);
                       Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, 25f);// MainCam.GetComponent<Camera>().nearClipPlane);
                       Vector3 worldPosition = MainCam.GetComponent <Camera>().ScreenToWorldPoint(screenPos);
                       SpawnedCoral = GameObject.Instantiate(coralObj, worldPosition, Quaternion.identity); // Generate Random Rotation
                       //SpawnedButton.transform.parent = gameObject.transform.parent;
                       Debug.Log("OnEndDrag: screenPos = " + screenPos + ", worldPos = " + worldPosition + "lblPos = "  + gameObject.transform.position );
                   }
                   Debug.Log("End Drag: curPos = " + eventData.position);
               }

               private bool IsRectTransformInsideSreen(RectTransform rectTransform)
               {
                   bool isInside = false;
                   Vector3[] corners = new Vector3[4];
                   rectTransform.GetWorldCorners(corners);
                   int visibleCorners = 0;
                   Rect rect = new Rect(0, 0, Screen.width, Screen.height);
                   foreach (Vector3 corner in corners)
                   {
                       if (rect.Contains(corner))
                       {
                           visibleCorners++;
                       }
                   }
                   if (visibleCorners == 4)
                   {
                       isInside = true;
                   }
                   return isInside;
               }

               // Start is called before the first frame update
               void Start()
               {
                   MainCam = GameObject.FindWithTag("MainCamera");
               }*/

    }
 
}
