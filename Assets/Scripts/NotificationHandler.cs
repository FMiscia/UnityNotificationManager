using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class NotificationHandler : MonoBehaviour {

	private AndroidJavaObject activityContext = null;
	private Texture2D iconTexture = null;
	private AndroidJavaObject androidNotificationData = null;

	/**
	 * 
	 * Handles notifications from notification bar
	 * 
	 * */
	void Start () { 
		// Retrieves buttons status from preferences
		Dictionary<string,ArrayList> notifications = DataHandler.getInstance ().getNotifications ();
		foreach(ArrayList notification in notifications.Values){
			bool singleRemoveStatus = PlayerPrefs.GetInt ((string)notification[3])==1?true:false;
			((Button)GameObject.Find ((string)notification[3]).GetComponent<Button>()).interactable = singleRemoveStatus;
		}
		bool removeStatus = PlayerPrefs.GetInt("Button_Remove")==1?true:false;
		((Button)GameObject.Find ("Button_Remove").GetComponent<Button> ()).interactable = removeStatus;
		//Checks intent from android plugin
		using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			if (activityClass != null) {
				activityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
				AndroidJavaObject intent = activityContext.Call<AndroidJavaObject> ("getIntent");
				if (intent != null) {
					androidNotificationData  = new AndroidJavaObject ("java.util.ArrayList");
					androidNotificationData = intent.Call<AndroidJavaObject> ("getStringArrayListExtra", "NotificationData");
					if (androidNotificationData != null && androidNotificationData.Call<int>("size") == 4) {
						setIcon(androidNotificationData.Call<string>("get",3));
						setMessage(androidNotificationData.Call<string> ("get", 2));
					}
				}
			}
		}
	} 

	/**
	 * 
	 * Shows the notification on the GUI
	 * 
	 * */
	void OnGUI(){
		//If there is new data we show it on a Gui Box
		if (DataHandler.getInstance().getMessage() != null && DataHandler.getInstance().getIcon () != null 
		   && !DataHandler.getInstance().getMessage().Equals(DataHandler.getInstance().getOldMessage()) 
		   && !DataHandler.getInstance().getIcon().Equals(DataHandler.getInstance().getOldIcon())) {
			int disabledButtons = 0;
			//If all the single buttons are not interactable, so we received all the notifications 
			((Button)GameObject.Find (DataHandler.getInstance().getIcon()).GetComponent<Button>()).interactable = false;
			Button[] singleButtons = GameObject.Find ("bottomPanel").GetComponentsInChildren<Button>(true);
			foreach (Button button in singleButtons){
				if(button.interactable == false)
					disabledButtons++;
			}
			//If all the notifications are received we dispose the "remove button" 
			if(disabledButtons == DataHandler.getInstance().getNotifications().Count){
				((Button)GameObject.Find ("Button_Remove").GetComponent<Button>()).interactable = false;
			}
			//Let's show the notification content
			iconTexture = (Texture2D)Resources.Load (DataHandler.getInstance().getIcon()) as Texture2D;
			GUIStyle style = new GUIStyle (GUI.skin.box);
			style.alignment = TextAnchor.MiddleLeft;
			style.fontSize = 50;
			GUI.Box (new Rect (20, 10, Screen.width-40, 200), new GUIContent (" " + DataHandler.getInstance().getMessage(), iconTexture), style);
		}
	}

	/**
	 * 
	 * Saves the status of the buttons 
	 * 
	 * */
	public void OnApplicationPause(bool pause){
	
		if (pause) {
			Dictionary<string,ArrayList> notifications = DataHandler.getInstance ().getNotifications ();
			foreach(ArrayList notification in notifications.Values){
				PlayerPrefs.SetInt((string)notification[3],((Button)GameObject.Find ((string)notification[3]).GetComponent<Button>()).interactable?1:0);
			}
			PlayerPrefs.SetInt("Button_Remove",((Button)GameObject.Find ("Button_Remove").GetComponent<Button>()).interactable?1:0);
			PlayerPrefs.Save();
		}
	}

	/**
	 * 
	 * Sets the icon of the notification 
	 * 
	 * */
	void setIcon (string icon){
		DataHandler.getInstance ().setIcon (icon);
	}

	/**
	 * 
	 * Sets the content message of the notification 
	 * 
	 * */
	void setMessage (string message){
		DataHandler.getInstance ().setMessage (message);
	}
	
}
