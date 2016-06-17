using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Data Collector
 * */
public class DataHandler  {

	private static DataHandler _instance = null;

	private Dictionary<string,ArrayList> notifications = new Dictionary<string,ArrayList>();
	
	private string icon = null;
	private string message = null;
	private string oldMessage = null;
	private string oldIcon = null;


	private DataHandler(){
		if (notifications.Count == 0) {
			notifications.Add ("1", new ArrayList (new string[]{ "1", "First", "First Beer", "beer1" }));
			notifications.Add ("2", new ArrayList (new string[]{ "2", "Second", "Second Beer", "beer2" }));
			notifications.Add ("3", new ArrayList (new string[]{ "3", "Third", "Third Beer", "beer3" }));
			notifications.Add ("4", new ArrayList (new string[]{ "4", "Fourth", "Fourth Beer", "beer4" }));
			notifications.Add ("5", new ArrayList (new string[]{ "5", "Fifth", "Fifth Beer", "beer5" }));
		}
	}

	public Dictionary<string,ArrayList> getNotifications(){
		return this.notifications;
	}

	public string getIcon(){
		return this.icon;
	}

	public void setIcon(string icon){
		setOldIcon (this.icon);
		this.icon = icon;
	}

	public string getOldIcon(){
		return this.oldIcon;
	}
	
	public void setOldIcon(string icon){
		this.oldIcon = icon;
	}

	public string getMessage(){
		return this.message;
	}
	
	public void setMessage(string message){
		setOldMessage (this.message);
		this.message = message;
	}

	public string getOldMessage(){
		return this.oldMessage;
	}
	
	public void setOldMessage(string message){
		this.oldMessage = message;
	}

	public static DataHandler getInstance(){

		if(_instance == null){
			_instance = new DataHandler ();
		}

		return _instance;
	}

}
