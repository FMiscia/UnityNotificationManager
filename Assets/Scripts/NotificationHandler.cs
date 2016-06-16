using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class NotificationHandler : MonoBehaviour {

	private AndroidJavaObject activityContext = null;
	private Texture2D iconTexture = null;
	private AndroidJavaObject androidNotificationData = null;

	private string icon = null;
	private string message = null;
	private string oldMessage = null;
	private string oldIcon = null;
	private Dictionary<string,ArrayList> notifications = new Dictionary<string,ArrayList>();

	void Start () { 
		
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

	void OnGUI(){
		if (message != null && icon != null && !message.Equals(oldMessage) && !icon.Equals(oldIcon)) {
			iconTexture = (Texture2D)Resources.Load (icon) as Texture2D;
			GUIStyle style = new GUIStyle (GUI.skin.box);
			style.alignment = TextAnchor.MiddleLeft;
			style.fontSize = 50;
			GUI.Box (new Rect (20, 10, Screen.width-40, 200), new GUIContent (" " + message, iconTexture), style);
		}
	}

	void setIcon (string icon){
		this.oldIcon = this.icon;
		this.icon = icon;
	}

	void setMessage (string message){
		this.oldMessage = this.message;
		this.message = message;
	}

	public Dictionary<string,ArrayList> getNotifications(){
		return this.notifications;
	}
}
