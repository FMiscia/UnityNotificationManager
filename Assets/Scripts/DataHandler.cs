using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Data Collector
 * */
public class DataHandler  {

	private static DataHandler _instance = null;

	private Dictionary<string,ArrayList> notifications = new Dictionary<string,ArrayList>();

	private DataHandler(){
		if (notifications.Count == 0) {
			notifications.Add ("beer1", new ArrayList (new string[]{ "1", "First", "First Beer", "beer1" }));
			notifications.Add ("beer2", new ArrayList (new string[]{ "2", "Second", "Second Beer", "beer2" }));
			notifications.Add ("beer3", new ArrayList (new string[]{ "3", "Third", "Third Beer", "beer3" }));
			notifications.Add ("beer4", new ArrayList (new string[]{ "4", "Fourth", "Fourth Beer", "beer4" }));
			notifications.Add ("beer5", new ArrayList (new string[]{ "5", "Fifth", "Fifth Beer", "beer5" }));
		}
	}

	public Dictionary<string,ArrayList> getNotifications(){
		return this.notifications;
	}

	public static DataHandler getInstance(){

		if(_instance == null){
			_instance = new DataHandler ();
		}

		return _instance;
	}

}
