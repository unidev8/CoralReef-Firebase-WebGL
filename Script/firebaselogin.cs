using UnityEngine;
using cakeslice;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using System;

public class firebaselogin : MonoBehaviour
{
    public TMP_Text statusText;

    // Start is called before the first frame update

    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
       
    CoralInfo coralInfo;
   
    PlayerInfo playerInfo;

    public string collectionPath_user = "https://console.firebase.google.com/u/0/project/lionkin-gs/firestore/data/~2Freef_users";
    public string collectionPath_reef = "https://console.firebase.google.com/u/0/project/lionkin-gs/firestore/data/~2Freef_assets";
    public string my_reef_id = "test_cscnwjhcuqgeifubwicbwiuefhiwufb";

    public static firebaselogin instance = null;

    public GameObject coralObj;
    

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
        FirebaseAuth.OnAuthStateChanged(gameObject.name, "DisplayUserInfo", "DisplayInfo");
        
    }

    public void SignInWithEmailAndPassword() =>
        FirebaseAuth.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void LoginToFirebase(string customToken)
    {
        if (customToken != "")
        {
            //FirebaseAuth.SignInWithCustomTokenAsync(customToken, "DisplayUserInfo", "DisplayErrorObject");       
            statusText.text = customToken;
            playerInfo = JsonUtility.FromJson<PlayerInfo>(customToken);
            //FirebaseFirestore.GetDocument(collectionPath_user, documentIdInputField.text, gameObject.name, "DisplayData", "DisplayErrorObject");
        }

        SceneManager.LoadScene(1);
    }
       
    public void DisplayUserInfo(string user)
    {
        //var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), user) as FirebaseUser;
        //DisplayData($"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}");

        SceneManager.LoadScene(1);
    }
        
    public void DisplayData(string data)
    {
        statusText.color = statusText.color == Color.green ? Color.blue : Color.green;
        statusText.text = data;
        Debug.Log(data);
    }

    public void DisplayInfo(string info)
    {
        statusText.color = Color.white;
        statusText.text = info;
        Debug.Log(info);
    }

    public void DisplayErrorObject(string error)
    {
        var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        statusText.color = Color.red;
        statusText.text = error;
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