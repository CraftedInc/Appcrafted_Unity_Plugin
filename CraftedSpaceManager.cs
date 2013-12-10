using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for List<> http://msdn.microsoft.com/en-us/library/6sh2ey19.aspx
using Boomlagoon.JSON;
using System;
using System.Text;

public class CraftedSpaceManager : MonoBehaviour { //need to be a MonoBehaviour to use Coroutine for WWW class
	
	public List<CraftedSpace> craftedSpaces = new List<CraftedSpace>();
	private string accessKey;
	private string secretKey;

	public delegate	void AssetAction(string craftedSpaceID);
	public static event AssetAction OnAssetDownloaded;
	
	#region Create Singleton
	//[begin]creating a singleton that doesn't need to be attached to a gameobject
	private static CraftedSpaceManager instance = null;
	public CraftedSpaceManager()
	{
		if (instance !=null)
		{
			Debug.LogError ("Cannot have two instances of singleton.");
			return;
		}
		instance = this;
	}
	public static CraftedSpaceManager Instance
	{
        get
        {
            if (instance == null)
			{
				// component-based - we have to use a component-based approach since we cannot use coroutine if this is not a monobehavior
				Debug.Log ("instantiate");
				GameObject go = new GameObject();
				instance = go.AddComponent<CraftedSpaceManager>();
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

	public void registerCraftedSpacesFromApp(params string[] craftedSpaceIDs){
		StartCoroutine(registerCraftedSpaces(craftedSpaceIDs));
	}

	public IEnumerator registerCraftedSpaces(params string[] craftedSpaceIDs) {

		//validate credentials
		if (this.accessKey == null || this.secretKey == null) {
			throw new System.MemberAccessException("missing credentials");
		}

		for (int i = 0; i < craftedSpaceIDs.Length; i++) {
			//check if any craftedSpaceID already exists in adSpaces list
			bool craftedSpaceAlreadyRegistered = craftedSpaces.Exists ( delegate (CraftedSpace obj) {return obj.craftedSpaceID == craftedSpaceIDs[i];} );
			if (craftedSpaceAlreadyRegistered) {	//re-register a previously registered adspace
				Debug.Log("CraftedSpace: "+ craftedSpaceIDs[i] + " has been registered before. Re-registering.");
				CraftedSpace craftedSpaceToReregister = craftedSpaces.Find ( delegate (CraftedSpace obj) {return obj.craftedSpaceID == craftedSpaceIDs[i];} ); //find the repeated adspace from the List
				unloadCraftedSpaceFromMemory(craftedSpaceToReregister); //unload unused assets from memory
				yield return StartCoroutine(GETCraftedSpace(craftedSpaceToReregister));	//load new ads from on the server	
				Debug.Log ("CraftedSpace: "+ craftedSpaceToReregister.craftedSpaceID + " has been re-registered.");

				//trigger event OnAssetDownloaded
				if (OnAssetDownloaded != null) {
					OnAssetDownloaded(craftedSpaceToReregister.craftedSpaceID);
				}
			
			} else {	//register a new craftedSpace
				CraftedSpace newCraftedSpace = new CraftedSpace(craftedSpaceIDs[i]);
				craftedSpaces.Add(newCraftedSpace);
				yield return StartCoroutine(GETCraftedSpace(newCraftedSpace));	//load new ads from on the server		
				Debug.Log ("CraftedSpace: "+ newCraftedSpace.craftedSpaceID + " has been registered.");

				//trigger event OnAssetDownloaded
				if (OnAssetDownloaded != null) {
					OnAssetDownloaded(newCraftedSpace.craftedSpaceID);
				}
			}
		}
	}


	public CraftedAsset[] getAllAssets(string craftedSpaceID) {

		//check if any craftedSpaceID already exists in adSpaces list (if already registered)
		bool craftedSpaceAlreadyRegistered = craftedSpaces.Exists ( delegate (CraftedSpace obj) {return obj.craftedSpaceID == craftedSpaceID;} );
		if (!craftedSpaceAlreadyRegistered) {
			Debug.LogError("You Should Register the CraftedSpace First Before using getAllAds(string craftedSpaceID)");
			StartCoroutine(registerCraftedSpaces(craftedSpaceID));
			//find the specified adspace from the List
			return craftedSpaces.Find ( delegate (CraftedSpace obj) { return obj.craftedSpaceID == craftedSpaceID; } ).craftedAssets;
		}

		//find the specified adspace from the List
		CraftedSpace craftedSpaceToLoad = craftedSpaces.Find ( delegate (CraftedSpace obj) { return obj.craftedSpaceID == craftedSpaceID; } );
		return craftedSpaceToLoad.craftedAssets;
	}


	public CraftedAsset[] getAllAssets() {
		if (this.craftedSpaces[0] != null) {
			//return the 1st adspace from the List
			return this.craftedSpaces[0].craftedAssets;
		} else {
			Debug.LogError ("No craftedSpace registered");
			return null;
		}
	}

	public CraftedAsset getAsset(string craftedSpaceID, int assetID) {
		//find the specified adspace from the List
		CraftedSpace craftedSpaceToLoad = craftedSpaces.Find (	
			delegate(CraftedSpace obj) {
				return obj.craftedSpaceID == craftedSpaceID;
			}
		);
		
		return craftedSpaceToLoad.craftedAssets[assetID];
	}

	IEnumerator GETCraftedSpace (CraftedSpace craftedSpace) {

		//header for authentication
		Hashtable headers = new Hashtable();
		headers["Authorization"] = "Basic " +
									System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
									this.accessKey + ":" + this.secretKey));

		string apiUrlPart1 = "http://api.appcrafted.com/alpha/cspace/"; 
		string apiUrlPart2 = "/asset";	
		string JSONurl = apiUrlPart1 + craftedSpace.craftedSpaceID + apiUrlPart2;

		WWW www = new WWW(JSONurl, null, headers);	// Start a download of the given URL
		yield return www;			// wait until the download is done
		JSONObject thisJSONObject = JSONObject.Parse(www.text); //define the JSON Object
				
		//initialize craftedAssets array in the craftedSpace
		craftedSpace.craftedAssets = new CraftedAsset[thisJSONObject.GetArray("Assets").Length];
		for (int i = 0; i < craftedSpace.craftedAssets.Length; i++) {
			craftedSpace.craftedAssets[i] = new CraftedAsset();
		}
		
		//load CraftedAssets (an array of CraftedAsset)
		for (int i = 0; i < craftedSpace.craftedAssets.Length; i++) {
			JSONObject thisAsset = JSONObject.Parse(thisJSONObject.GetArray("Assets")[i].ToString());

			//check if there's an image link
			if (thisAsset.GetString ("image") != null){
				//get the image from the image link
				WWW wwwImage = new WWW(thisAsset.GetString ("image")); // "image" contains a link (string) to the image
				yield return wwwImage;
				craftedSpace.craftedAssets[i].image = wwwImage.texture;
			}
				
			//get all other values
			craftedSpace.craftedAssets[i].title = thisAsset.GetString ("title");
			craftedSpace.craftedAssets[i].text = thisAsset.GetString ("text");
			craftedSpace.craftedAssets[i].link = thisAsset.GetString ("link");
		}

	}
	
	private void unloadCraftedSpaceFromMemory(CraftedSpace craftedSpace) { //ToDo: need to re-evaluate, not generic enough 
		for (int i = 0; i < craftedSpace.craftedAssets.Length; i++) {
			Destroy(craftedSpace.craftedAssets[i].image);	//removes the image texture from memory
		}
				
	}
	
	//Register impressions and clicks of an asset	
	public void registerImpressionsAndClicks(string craftedSpaceID, int assetID, int impressions, int clicks) {
		//validate credentials
		if (this.accessKey == null || this.secretKey == null) {
			throw new System.MemberAccessException("missing credentials");
		}

		StartCoroutine (PostData(craftedSpaceID,assetID,impressions,clicks));
	}
	
	IEnumerator PostData(string craftedSpaceID, int assetID, int impressions, int clicks) {

		string ourPostData = "{\"impressions\": " + impressions.ToString() + ", \"clicks\": " + clicks.ToString() +"}";
		
		Hashtable headers = new Hashtable();
		headers.Add("Content-Type", "application/json");
		headers["Authorization"] = "Basic " +
			System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
			this.accessKey + ":" + this.secretKey));
		
		byte[] body = Encoding.UTF8.GetBytes(ourPostData);
		
		WWW www = new WWW("http://api.appcrafted.com/alpha/cspace/"+craftedSpaceID+"/asset/"+assetID.ToString()+"/metrics", body, headers);
		
		yield return www;
		JSONObject thisJSONObject = JSONObject.Parse(www.text); 
		Debug.Log ("POST result: "+ thisJSONObject);
		
	}
}

