using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for List<> http://msdn.microsoft.com/en-us/library/6sh2ey19.aspx
using Boomlagoon.JSON;

public class AdSpaceManager : MonoBehaviour //need to be a MonoBehaviour to use Coroutine for WWW class
{
	
	public List<AdSpace> adSpaces = new List<AdSpace>();
	
	#region Create Singleton //http://answers.unity3d.com/questions/17916/singletons-with-coroutines.html
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
	
	public void registerAdSpaces(params string[] adSpaceIDs)
	{
		
		int numberOfAdSpaces = adSpaceIDs.Length;
		
		for (int i = 0; i < numberOfAdSpaces; i++)
		{
			//check if any adSpaceID already exists in adSpaces list
			bool adSpaceAlreadyRegistered = adSpaces.Exists ( delegate (AdSpace obj) {return obj.adSpaceID == adSpaceIDs[i];} );
			if (adSpaceAlreadyRegistered)
			{
				Debug.Log("Ad Space Already Registered");
								
				//find the repeated adspace from the List
				AdSpace adSpaceToReregister = adSpaces.Find ( delegate (AdSpace obj) {return obj.adSpaceID == adSpaceIDs[i];} );
				//unload unused assets from memory
				unloadAdSpaceFromMemory(adSpaceToReregister);
				//load new ad from JSON on the server								
				StartCoroutine(LoadJSON(adSpaceToReregister));	
				Debug.Log ("Registering AdSpace: "+ adSpaceToReregister.adSpaceID);
			
			}
			else //register a new adspace
			{
				AdSpace newAdSpace = new AdSpace(adSpaceIDs[i]);
				adSpaces.Add(newAdSpace);
				//load new ad from JSON on the server								
				StartCoroutine(LoadJSON(newAdSpace));	
				Debug.Log ("Registering AdSpace: "+ newAdSpace.adSpaceID);
			}
		}
		
	}
	
	public CraftedAd loadAd(string adSpaceID)
	{
		//find the specified adspace from the List
		AdSpace adSpaceToLoad = adSpaces.Find (	
			delegate(AdSpace obj) {
				return obj.adSpaceID == adSpaceID;
			}
		);
		

		if (adSpaceToLoad.indexOfAdToShow >= adSpaceToLoad.craftedAds.Length)
		{
			adSpaceToLoad.indexOfAdToShow = 1;
			return adSpaceToLoad.craftedAds[0];
		}
		else
		{	
			adSpaceToLoad.indexOfAdToShow += 1;
			return adSpaceToLoad.craftedAds[adSpaceToLoad.indexOfAdToShow-1];
		}
		
	}
	
	public CraftedAd loadAd(string adSpaceID, int adNumber)
	{
		//find the specified adspace from the List
		AdSpace adSpaceToLoad = adSpaces.Find (	
			delegate(AdSpace obj) {
				return obj.adSpaceID == adSpaceID;
			}
		);
		
		return adSpaceToLoad.craftedAds[adNumber];
	}
	
	public void registerImpression(string adSpaceID, int adNumber)
	{
		
	}
	
	public void registerClick(string adSpaceID, int adNumber)
	{
		
	}
	
	IEnumerator LoadJSON (AdSpace thisAdSpace) 
	{		
		
		string apiUrlPart1 = "http://api.adcrafted.com/adspace/";
		string apiUrlPart2 = "/ad";	
		string JSONurl = apiUrlPart1 + thisAdSpace.adSpaceID + apiUrlPart2;
		
		WWW www = new WWW(JSONurl);	// Start a download of the given URL
		yield return www;			// wait until the download is done
		JSONObject thisJSONObject = JSONObject.Parse(www.text); //define the JSON Object
				
		//initialize craftedAds array in this adspace
		thisAdSpace.craftedAds = new CraftedAd[thisJSONObject.GetArray("Ads").Length];
		for (int i = 0; i < thisAdSpace.craftedAds.Length; i++)
		{
			thisAdSpace.craftedAds[i] = new CraftedAd();
		}
		
		//load ads into an array
		for (int i = 0; i < thisAdSpace.craftedAds.Length; i++)
		{
			JSONObject thisAd = JSONObject.Parse(thisJSONObject.GetArray("Ads")[i].ToString());
			
			//get the image from the image link
			WWW wwwImage = new WWW(thisAd.GetString ("image")); // "image" contains a link to the image
			yield return wwwImage;
			thisAdSpace.craftedAds[i].image = wwwImage.texture;
			
			//get all other values
			thisAdSpace.craftedAds[i].title = thisAd.GetString ("title");
			thisAdSpace.craftedAds[i].text = thisAd.GetString ("text");
			thisAdSpace.craftedAds[i].link = thisAd.GetString ("link");
			
			//Debug.Log ("JSON loaded: "+thisAdSpace.craftedAds[i].title);
		}
		
	}
	
	private void unloadAdSpaceFromMemory(AdSpace thisAdSpace) //ToDo: need to re-evaluate, not generic enough 
	{
		for (int i = 0; i < thisAdSpace.craftedAds.Length; i++)
		{
			Destroy(thisAdSpace.craftedAds[i].image);	//removes the image texture from memory
		}
				
	}
	
}

