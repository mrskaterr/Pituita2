using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using TMPro; 
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;


public class AutoPlay : MonoBehaviour 
{  
    public bool soloGame;
    string whoIs;
    [SerializeField] GameObject Manager; 
    [SerializeField] GameObject CustomGame;
    [SerializeField] GameObject CreateOrJoin;  
    [SerializeField] GameObject PrivateRoomDetails; 

    bool []isDone=new bool[]{false,false,false}; 
    public void NameButton(string buff)
    {
        whoIs=buff;
    }
    void Start() 
    {
        if(SceneManager.GetActiveScene()!=SceneManager.GetSceneAt(0))
            this.enabled=false;
        //Manager.GetComponent<PlayfabLogin>().LoginButtonMethod(); 
    } 
    void Update() 
    { 
        if(!isDone[0] && CustomGame.activeInHierarchy) 
        { 
            Manager.GetComponent<UIManager>().SetSection_SessionDetails(); 
            isDone[0]=true; 
        } 
        if(!isDone[1] && CreateOrJoin.activeInHierarchy) 
        { 
            Manager.GetComponent<LobbyManagerV2>().JoinOrCreateSession(); 
            Manager.GetComponent<UIManager>().SetLoadingIconActive(true); 
            CreateOrJoin.GetComponent<Button>().interactable=false; 
            isDone[1]=true; 
        } 
        if(!isDone[2] && PrivateRoomDetails.activeInHierarchy) 
        {
            if(whoIs=="1" )
                Manager.GetComponent<LobbyManagerV2>().SetRole(4);
            if(whoIs=="3")
                Manager.GetComponent<LobbyManagerV2>().SetRole(5);
            if(whoIs=="2")
                Manager.GetComponent<LobbyManagerV2>().SetRole(2); 
            if(soloGame)
                Manager.GetComponent<LobbyManagerV2>().IsReady();
            
            isDone[2]=true;
        } 
 
    } 
} 
