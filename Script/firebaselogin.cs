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
    // Start is called before the first frame update

    //public TMP_InputField emailInputField;
    //public TMP_InputField passwordInputField;


    [HideInInspector]
    public string collectionPath_user = "";
    [HideInInspector]                       
    public string collectionPath_reef = "";
    [HideInInspector]
    public string my_reef_id = "";
       
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


    }
   
    public void GetReef_Id(string _reef_Id)
    {
        if (_reef_Id != "")
        {
            //statusText.text = userInfo;
            //playerInfo = JsonUtility.FromJson<PlayerInfo>(Reef_Id);
            //FirebaseFirestore.GetDocument(collectionPath_user, documentIdInputField.text, gameObject.name, "DisplayData", "DisplayErrorObject");
            reef_ID = _reef_Id;
        }        
    }

    public void LoadMainScene()
    {
        if (isAuth && reef_ID != "")
        {
            collectionPath_reef = "/ reef_id /" + reef_ID + "/ reef_assets";
            SceneManager.LoadScene(1);
        }
            
        else
        {
            statusErrorText.text = "You should be SignIn!";
        }
    }

    public void AuthStateChangedCallback(string user)
    {
        var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), user) as FirebaseUser;
        //DisplayData($"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}");

        statusAuthChangeText.color = statusAuthChangeText.color == Color.green ? Color.blue : Color.green;
        statusAuthChangeText.text = "Name: " + parsedUser.displayName + "uid: " + parsedUser.uid;
        if (((FirebaseUser)parsedUser).isAnonymous)
        {
            isAuth = true;
            statusErrorText.text = "Signed In! Please Load Assets";
            uID = parsedUser.uid;            
        }
        else
        {
            isAuth = false;
            statusErrorText.text = "Signed Out";
        }
        Debug.Log("DisplayUserInfo: " + user);        
    }         

    public void DisplayInfo(string info)
    {
        statusSignInText.color = Color.white;
        statusSignInText.text = info;
        Debug.Log(info);
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
    public string asset_filename;
    public string coralIdx;
    public bool editableState;
    public bool in_inventory;
    public string reef_id;
    public float rotX;
    public float rotY;
    public float rotZ;
    public float scale;
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public struct PlayerInfo
{
    public string SecurityToken;
    public DateTime SecurityTokenUpdated;
    public string nonce;
    public string reef_id;
    public string user_uid;
    public string walletId;

}