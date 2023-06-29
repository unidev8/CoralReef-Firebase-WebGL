using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;


// this class of Coral_Btn is to spawn Coral_lbl object

public class CoralLabel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler //IPointerEnterHandler, IPointerExitHandler
{
    public GameObject lblObj;

    private Vector2 lastMousePosition;
    private GameObject SpawnedLabel;

    //private bool isPointerEntered = false;
    //private bool isSelected = false;    
    

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!SpawnedLabel)
        {
            SpawnedLabel = GameObject.Instantiate(lblObj, this.transform.position, this.transform.rotation);
            SpawnedLabel.transform.parent = gameObject.transform.parent;
            //Debug.Log("this.parent is " + gameObject.transform.parent.name);
        }
            
        //Debug.Log("Label_BeginDrag: curPos = " + eventData.position);
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Vector2 currentPosition = eventData.position;
        //Vector2 diff = currentPosition - lastMousePosition;
        //RectTransform rect = GetComponent<RectTransform>();

        //Vector3 newPositon = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
        //Vector3 oldPos = rect.position;
        //rect.position = newPositon;       

        //if (!IsRectTransformInsideSreen(rect))
        {
            //rect.position = oldPos;            
        }
        //lastMousePosition = currentPosition;
        //Debug.Log("Label_OnDrag: curPos = " + eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Label_End Drag: curPos = " + eventData.position);
        //GetDocument(string collectionPath, string documentId, string objectName, string callback, string fallback);
    }
    


    public void OnPointerEnter(PointerEventData eventData)
    {
        //isPointerEntered = true;
        //Debug.Log("Entered " + this.gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //isPointerEntered = false;
        //Debug.Log("Exited " + this.gameObject.name);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown (0) && isPointerEntered )
        {
            //isSelected = true;// !isSelected;
            //Debug.Log("Selected " + this.gameObject.name + ", " + isSelected );
        }
        //if (Input.GetMouseButton (0) && isSelected )
        {
            //float dx = Input.GetAxis("Horizontal") * speed;
            //float dy = Input.GetAxis("Vertical") * speed;
            //dx *= Time.deltaTime;
            //dy *= Time.deltaTime;
            //gameObject.transform.Translate(dx, dy, 0f);
            //Debug.Log("Move " + this.gameObject.name + " dx = " + dx +", dy = " +  dy + "isSelected = " + isSelected );
        }
    }
}
