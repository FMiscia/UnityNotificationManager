using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class submitNotification : MonoBehaviour {

	private AndroidJavaObject notificationHandler = null;
	private AndroidJavaObject activityContext = null;

	public void onSubmitClick(){
		this.disposeNotifications ();
		this.sendNotifications ();
		activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
			notificationHandler.Call("showMessage", "Notifications submitted");
		}));
	}

	public void onRemoveClick(){
		this.disposeNotifications ();
		activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
			notificationHandler.Call("showMessage", "Notifications disposed");
		}));
	}

	public void onSingleRemoveClick(string id){
		this.disposeNotification (id);
		activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
			notificationHandler.Call("showMessage", "beer "+id+" disposed");
		}));
	}

	private void sendNotifications(){		
		Dictionary<string,ArrayList> notifications = DataHandler.getInstance ().getNotifications ();
		using (AndroidJavaClass activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
			activityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
		}

		if(notificationHandler == null){
			using (AndroidJavaClass pluginClass = new AndroidJavaClass ("com.dev.francescomiscia.minicliplibrary.controllers.NotificationHandler")) {
				if (pluginClass != null) {
					notificationHandler = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
				}
			}
		} 
		notificationHandler.Call("setContext", activityContext);
		int delayCounter = 0;
		foreach (ArrayList notification in notifications.Values) {
			notificationHandler.Call("submitNotification",notification[0],notification[1],notification[2],notification[3],10*1000*delayCounter++);
		}		
	}

	private void disposeNotifications(){
		Dictionary<string,ArrayList> notifications = DataHandler.getInstance ().getNotifications ();
		using (AndroidJavaClass activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
			activityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
		}

		if(notificationHandler == null){
			using (AndroidJavaClass pluginClass = new AndroidJavaClass ("com.dev.francescomiscia.minicliplibrary.controllers.NotificationHandler")) {
				if (pluginClass != null) {
					notificationHandler = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
				}
			}
		} 
		notificationHandler.Call ("setContext", activityContext);
		foreach (ArrayList notification in notifications.Values) {
			notificationHandler.Call("disposeNotification",notification [0]);
		}
	}

	private void disposeNotification(string id){
		using (AndroidJavaClass activityClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
			activityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
		}

		if(notificationHandler == null){
			using (AndroidJavaClass pluginClass = new AndroidJavaClass ("com.dev.francescomiscia.minicliplibrary.controllers.NotificationHandler")) {
				if (pluginClass != null) {
					notificationHandler = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
				}
			}
		} 
		notificationHandler.Call ("setContext", activityContext);
		notificationHandler.Call("disposeNotification",id);
	}



}
