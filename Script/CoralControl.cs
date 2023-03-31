using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using FirebaseWebGL.Scripts.FirebaseBridge;
using System.Diagnostics;

namespace CoralReef
{
    public class CoralControl : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler  
    {
        public GameObject coralObj;
        private GameObject SpawnedCoral;
        
        private bool isSelected = false;
        private bool isPlanted = true;
        private Material originalMat;

       
        CoralInfo coralInfo;



        private void Start()
        {
            isSelected = false;
            isPlanted = true; // Should be like this
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isPlanted) return;

            isSelected = true;
            isPlanted = false;
            Vector3 firstPos = Vector3.zero;
            SpawnedCoral = GameObject.Instantiate(coralObj, firstPos, Quaternion.identity); // Generate Random Rotation

            /*
            GameControl gameControl = GameObject.FindGameObjectWithTag("GameControl").GetComponent <GameControl>();
            for (int i = 0; i < gameControl.coralLbl.Length ; i ++ )
            {
                foreach (CoralInfo coral in gameControl.coralReef)
                {
                    if (coral.reef_id == gameControl.my_reef_id)
                    {
                        for (int j = 0; j < gameControl.coralLbl.Length; j++)
                        {
                            if (coral.coralIdx == coralObj.name)
                            {
                                FirebaseFirestore.UpdateDocument(firebaselogin.instance.collectionPath_reef, documentIdInputField.text, valueInputField.text, gameObject.name,
            "UpdateDoc", "DisplayErrorObject");
                            }
                        }                        
                    }
                } 
            }
            */

            Renderer objectRenderer = SpawnedCoral.GetComponent<MeshRenderer>();
            originalMat = objectRenderer.material;// child.GetComponent<Material>();
            objectRenderer.material = Resources.Load("Force Field", typeof(Material)) as Material;
            //objectRenderer.material.SetFloat("FresnelPower", frenselPower);
            //Debug.Log("SetWinWalk: frenselPower = " + frenselPower + ", objectRenderer" + objectRenderer.material);
        }

        public void UpdateDoc(string data)
        {
            UnityEngine.Debug.Log(data);
        }

        public void DisplayErrorObject(string data)
        {
            UnityEngine.Debug.Log(data);
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
                    }                        
                }               
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {            
            isPlanted = true;
            SpawnedCoral.GetComponent<MeshRenderer>().material = originalMat;
            SpawnedCoral.layer = 0;
            //Debug.Log("OnEndDrag: isPlanted = " + isPlanted);
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
