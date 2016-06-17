using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationScheduler : MonoBehaviour {

	private AndroidJavaObject notificationHandler = null;
	private AndroidJavaObject activityContext = null;

	/**
	 * 
	 * Handles the click on "Submit notifications" button
	 * It dispose all the previous submitted notifications and
	 * send new ones
	 *	
	 * */
	public void onSubmitClick(){
		this.disposeNotifications ();
		this.sendNotifications ();
		activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
			notificationHandler.Call("showMessage", "Notifications submitted");
		}));
	}

	/**
	 * 
	 * Handles the click on "Dispose notifications" button
	 * 
	 * */
	public void onRemoveClick(){
		this.disposeNotifications ();
		activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
			notificationHandler.Call("showMessage", "Notifications disposed");
		}));
	}

	/**
	 * 
	 * Handles the click on single notification dispose button
	 * 
	 * */
	public void onSingleRemoveClick(string id){
		this.disposeNotification (id);
		activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
			notificationHandler.Call("showMessage", "beer "+id+" disposed");
		}));
	}

	/**
	 * 
	 * Sends the requested notifications to the android plugin
	 * 
	 * */
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
			notificationHandler.Call("submitNotification",notification[0],notification[1],notification[2],notification[3],60*1000*delayCounter++);
		}		
	}

	/**
	 * 
	 * Sends a dispose request for all the notifications to the android plugin
	 * 
	 * */
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

	/**
	 * 
	 * Sends a dispose request to the android plugin for a single notification
	 * 
	 * */
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
