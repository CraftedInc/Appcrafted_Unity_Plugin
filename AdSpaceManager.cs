using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for List<> http://msdn.microsoft.com/en-us/library/6sh2ey19.aspx
using Boomlagoon.JSON;
using System;
using System.Text;

public class AdSpaceManager : MonoBehaviour { //need to be a MonoBehaviour to use Coroutine for WWW class
	
	public List<AdSpace> adSpaces = new List<AdSpace>();
	private string accessKey;
	private string secretKey;
	
	#region Create Singleton
	//[begin]creating a singleton that doesn't need to be attached to a gameobject
	private static AdSpaceManager instance = null;
	public AdSpaceManager()
	{
		if (instance !=null)
		{
			Debug.LogError ("Cannot have two instances of singleton.");
			return;
		}
		instance = this;
	}
	public static AdSpaceManager Instance
	{
        get
        {
            if (instance == null)
			{
				// component-based - we have to use a component-based approach since we cannot use coroutine if this is not a monobehavior
				Debug.Log ("instantiate");
				GameObject go = new GameObject();
				instance = go.AddComponent<AdSpaceManager>();
				go.name = "CraftedController";
				
            }
            return instance;
        }
    }
	//[end]creating a singleton that doesn't need to be attached to a gameobject//
	#endregion

	public void registerCredentials(string accessKey, string secretKey) {
		this.accessKey = accessKey;
		this.secretKey = secretKey;
	}

	public IEnumerator registerAdSpaces(params string[] adSpaceIDs) {

		//validate credentials
		if (this.accessKey == null || this.secretKey == null) {
			throw new System.MemberAccessException("missing credentials");
		}

		for (int i = 0; i < adSpaceIDs.Length; i++) {
			//check if any adSpaceID already exists in adSpaces list
			bool adSpaceAlreadyRegistered = adSpaces.Exists ( delegate (AdSpace obj) {return obj.adSpaceID == adSpaceIDs[i];} );
			if (adSpaceAlreadyRegistered) {	//re-register a previously registered adspace
				Debug.Log("AdSpace: "+ adSpaceIDs[i] + " has been registered before. Re-registering.");
				AdSpace adSpaceToReregister = adSpaces.Find ( delegate (AdSpace obj) {return obj.adSpaceID == adSpaceIDs[i];} ); //find the repeated adspace from the List
				unloadAdSpaceFromMemory(adSpaceToReregister); //unload unused assets from memory
				yield return StartCoroutine(GETAdSpace(adSpaceToReregister));	//load new ads from on the server	
				Debug.Log ("AdSpace: "+ adSpaceToReregister.adSpaceID + " has been re-registered.");
			
			} else {	//register a new adspace
				AdSpace newAdSpace = new AdSpace(adSpaceIDs[i]);
				adSpaces.Add(newAdSpace);
				yield return StartCoroutine(GETAdSpace(newAdSpace));	//load new ads from on the server		
				Debug.Log ("AdSpace: "+ newAdSpace.adSpaceID + " has been registered.");
			}
		}
	}


	public CraftedAd[] getAllAds(string adSpaceID) {

		//check if any adSpaceID already exists in adSpaces list (if already registered)
		bool adSpaceAlreadyRegistered = adSpaces.Exists ( delegate (AdSpace obj) {return obj.adSpaceID == adSpaceID;} );
		if (!adSpaceAlreadyRegistered) {
			Debug.LogError("You Should Register the AdSpace First Before using getAllAds(string adSpaceID)");
			StartCoroutine(registerAdSpaces(adSpaceID));
			//find the specified adspace from the List
			return adSpaces.Find ( delegate (AdSpace obj) { return obj.adSpaceID == adSpaceID; } ).craftedAds;
		}

		//find the specified adspace from the List
		AdSpace adSpaceToLoad = adSpaces.Find ( delegate (AdSpace obj) { return obj.adSpaceID == adSpaceID; } );
		return adSpaceToLoad.craftedAds;
	}


	public CraftedAd[] getAllAds() {
		if (this.adSpaces[0] != null) {
			//return the 1st adspace from the List
			return this.adSpaces[0].craftedAds;
		} else {
			Debug.LogError ("No adSpace registered");
			return null;
		}
	}

	public CraftedAd getAd(string adSpaceID, int adID) {
		//find the specified adspace from the List
		AdSpace adSpaceToLoad = adSpaces.Find (	
			delegate(AdSpace obj) {
				return obj.adSpaceID == adSpaceID;
			}
		);
		
		return adSpaceToLoad.craftedAds[adID];
	}

	IEnumerator GETAdSpace (AdSpace adSpace) {

		//header for authentication
		Hashtable headers = new Hashtable();
		headers["Authorization"] = "Basic " +
									System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
									this.accessKey + ":" + this.secretKey));

		string apiUrlPart1 = "http://api.adcrafted.com/adspace/";
		string apiUrlPart2 = "/ad";	
		string JSONurl = apiUrlPart1 + adSpace.adSpaceID + apiUrlPart2;

		WWW www = new WWW(JSONurl, null, headers);	// Start a download of the given URL
		yield return www;			// wait until the download is done
		JSONObject thisJSONObject = JSONObject.Parse(www.text); //define the JSON Object
				
		//initialize craftedAds array in the adspace
		adSpace.craftedAds = new CraftedAd[thisJSONObject.GetArray("Ads").Length];
		for (int i = 0; i < adSpace.craftedAds.Length; i++) {
			adSpace.craftedAds[i] = new CraftedAd();
		}
		
		//load CraftedAds (an array of CraftedAd)
		for (int i = 0; i < adSpace.craftedAds.Length; i++) {
			JSONObject thisAd = JSONObject.Parse(thisJSONObject.GetArray("Ads")[i].ToString());

			//check if there's an image link
			if (thisAd.GetString ("image") != null){
				//get the image from the image link
				WWW wwwImage = new WWW(thisAd.GetString ("image")); // "image" contains a link (string) to the image
				yield return wwwImage;
				adSpace.craftedAds[i].image = wwwImage.texture;
			}
				
			//get all other values
			adSpace.craftedAds[i].title = thisAd.GetString ("title");
			adSpace.craftedAds[i].text = thisAd.GetString ("text");
			adSpace.craftedAds[i].link = thisAd.GetString ("link");
		}

		// To Do: call the delegate


	}
	
	private void unloadAdSpaceFromMemory(AdSpace adSpace) { //ToDo: need to re-evaluate, not generic enough 
		for (int i = 0; i < adSpace.craftedAds.Length; i++) {
			Destroy(adSpace.craftedAds[i].image);	//removes the image texture from memory
		}
				
	}
	
	//Register impressions and clicks of an ad	
	public void registerImpressionsAndClicks(string adSpaceID, int adID, int impressions, int clicks) {
		//validate credentials
		if (this.accessKey == null || this.secretKey == null) {
			throw new System.MemberAccessException("missing credentials");
		}

		StartCoroutine (PostData(adSpaceID,adID,impressions,clicks));
	}
	
	IEnumerator PostData(string adSpaceID, int adID, int impressions, int clicks) {

		string ourPostData = "{\"impressions\": " + impressions.ToString() + ", \"clicks\": " + clicks.ToString() +"}";
		
		Hashtable headers = new Hashtable();
		headers.Add("Content-Type", "application/json");
		headers["Authorization"] = "Basic " +
			System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
			this.accessKey + ":" + this.secretKey));
		
		byte[] body = Encoding.UTF8.GetBytes(ourPostData);
		
		WWW www = new WWW("http://api.adcrafted.com/adspace/"+adSpaceID+"/ad/"+adID.ToString()+"/metrics", body, headers);
		
		yield return www;
		JSONObject thisJSONObject = JSONObject.Parse(www.text); 
		Debug.Log ("POST result: "+ thisJSONObject);
		
	}
}

