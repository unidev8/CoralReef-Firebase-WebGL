using UnityEngine;
using cakeslice;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using System;
using Models;

public class firebaselogin : MonoBehaviour
{
    public TMP_Text statusAuthChangeText;
    public TMP_Text statusSignInText;
    public TMP_Text statusErrorText;
    public TMP_Text txtTitle;
    public TMP_FontAsset fontAsset;

    // Start is called before the first frame update

    //public TMP_InputField emailInputField;
    //public TMP_InputField passwordInputField;


    [HideInInspector]
    public string collectionPath_user = "";
    [HideInInspector]                       
    public string collectionPath_reef = "";
           
    [HideInInspector]
    public string uID = "";
    [HideInInspector]
    public string reef_ID = "";

    private bool isAuth;

    public static firebaselogin instance = null;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
            return;
        }
        FirebaseAuth.OnAuthStateChanged(gameObject.name, "AuthStateChangedCallback", "DisplayInfo");

        FirebaseAuth.SignInAnonymously(gameObject.name, "DisplayInfo", "DisplayErrorObject");

        var _params = CoralReefImportJS.GetSearchParams();
        reef_ID = _params.Substring(_params.LastIndexOf("reef_id=") + 8);

        //txtTitle.textStyle.
        Debug.Log("Start: reef_id = " + reef_ID);
    }
   
    public void LoadMainScene()
    {
        statusErrorText.text = "reef_ID: " + reef_ID;
        //Debug.Log("LoadMainScene: reef_id = " + reef_ID);
        if (isAuth && (reef_ID != null || reef_ID != ""))
        {
            collectionPath_reef = "/reef_id/" + reef_ID + "/reef_assets";            
            SceneManager.LoadScene(1);
            //Debug.Log("loadMainScene: collectionPath_reef = " + collectionPath_reef);
        }            
        else
        {
            statusErrorText.text = "You should be SignIn!";
            //Debug.Log("loadMainScene: You should be SignIn!");
        }

        //SceneManager.LoadScene(1);
    }

    public void AuthStateChangedCallback(string user)
    {
        var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), user) as FirebaseUser;
        //DisplayData($"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}");

        statusAuthChangeText.color = statusAuthChangeText.color == Color.green ? Color.blue : Color.green;
        statusAuthChangeText.text = "User ID: \n" + parsedUser.uid;
        if (((FirebaseUser)parsedUser).isAnonymous)
        {
            isAuth = true;
                statusErrorText.text = " Play start! ";
            uID = parsedUser.uid;
            LoadMainScene();
        }
        else
        {
            isAuth = false;
                statusErrorText.text = "Signed Out";
        }
        //Debug.Log("AuthStateChangedCallback: " + user);        
    }         

    public void DisplayInfo(string info)
    {
        //statusSignInText.color = Color.white;
        //statusSignInText.text = "Success: signed up";//info;
        Debug.Log("info = " + info);
    }

    public void DisplayErrorObject(string error)
    {
        var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        statusErrorText.color = Color.red;
        statusErrorText.text = error;
        Debug.LogError(error);
    }  

}


[System.Serializable]
public struct CoralInfo
{
    //public string asset_filename;
    public string coralIdx;
    public bool in_inventory;
    //public string purchased_by;
    public string reef_id;
    public float rotX;
    public float rotY;
    public float rotZ;
    public float scale;
    public float x;
    public float y;
    public float z;
}