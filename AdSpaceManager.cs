using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for List<> http://msdn.microsoft.com/en-us/library/6sh2ey19.aspx
using Boomlagoon.JSON;

public class AdSpaceManager : MonoBehaviour //need to be a MonoBehaviour to use Coroutine for WWW class
{
	
	List<AdSpace> adSpaces = new List<AdSpace>();
	
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
				go.name = "AdSpace_singleton";
				
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
			AdSpace newAdSpace = new AdSpace(adSpaceIDs[i]);
			adSpaces.Add(newAdSpace);
			string url = "http://api.adcrafted.com/adspace/" + newAdSpace.adSpaceID + "/ad";
			StartCoroutine(LoadJSON(url), newAdSpace);	
		}
		
	}
	
	public void loadAd(string adSpaceID)
	{
		
	}
	
	public void loadAd(string adSpaceID, int adNumber)
	{
		
	}
	
	public void registerImpression(string adSpaceID, int adNumber)
	{
		
	}
	
	public void registerClick(string adSpaceID, int adNumber)
	{
		
	}
	
	IEnumerator LoadJSON (string JSONurl, AdSpace thisAdSpace) 
	{				
		WWW www = new WWW(JSONurl);	// Start a download of the given URL
		yield return www;			// wait until the download is done
		JSONObject adSpaceJSONObject = JSONObject.Parse(www.text); //define the JSON Object
		
		int numberOfAds = adSpaceJSONObject.GetArray("Ads").Length;
		
		//load ads into an array
		for (int i = 0; i < numberOfAds; i++)
		{
			JSONObject thisAd = JSONObject.Parse(adSpaceJSONObject.GetArray("Ads")[i].ToString());
			
			//get the image from the image link
			WWW wwwImage = new WWW(thisAd.GetString ("image")); // "image" contains a link to the image
			yield return wwwImage;
			thisAdSpace.craftedAds[i].image = wwwImage.texture;
			
			//get all other values
			thisAdSpace.craftedAds[i].title = thisAd.GetString ("title");
			thisAdSpace.craftedAds[i].text = thisAd.GetString ("text");
			thisAdSpace.craftedAds[i].link = thisAd.GetString ("link");
		}
		
		Debug.Log (	"craftedAds array in AdSpace: " +
					"\n" + thisAdSpace.craftedAds);
	}
	
}

