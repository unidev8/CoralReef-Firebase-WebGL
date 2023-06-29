using System.Runtime.InteropServices;
using UnityEngine;


public class CoralReefImportJS : MonoBehaviour
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern string getSearchParams();
#else
    [DllImport("coralreef.jslib")]
    private static extern string getSearchParams();
#endif

    public static string GetSearchParams()
    {
#if UNITY_EDITOR
        return "";
#elif UNITY_WEBGL
    return getSearchParams();
#else
    return getSearchParams();
#endif
    }

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void showShop();
#else
    [DllImport("coralreef.jslib")]
    private static extern void showShop();
#endif

    public static void ShowShopOnWebGL()
    {
#if UNITY_EDITOR
        return ;
#elif UNITY_WEBGL
    showShop();
#else
    showShop();
#endif
    }
    
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void userLogin();
#else
    [DllImport("coralreef.jslib")]
    private static extern void userLogin();
#endif

    public static void ShowUserLogin()
    {
#if UNITY_EDITOR
        return;
#elif UNITY_WEBGL
    userLogin();
#else
    userLogin();
#endif
    }

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void dologinAction();
#else
    [DllImport("coralreef.jslib")]
    private static extern void dologinAction();
#endif

    public static void Dologin()
    {
#if UNITY_EDITOR
        return;
#elif UNITY_WEBGL
    dologinAction();
#else
    dologinAction();
#endif
        Debug.Log ("Dologin");
    }
    
}
