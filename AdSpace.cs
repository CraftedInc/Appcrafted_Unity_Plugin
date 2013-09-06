using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class AdSpace : MonoBehaviour //MonoBehaviour so that we can use Coroutine to yield return www
{
	public CraftedAd[] craftedAds;
	public string adSpaceID;
	
	public AdSpace(string ID)
	{
		adSpaceID = ID;
	}
	
	/*
	IEnumerator LoadJSON (string JSONurl) {
		
		Debug.Log ("LoadJSON called");
				
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
			craftedAds[i].image = wwwImage.texture;
			
			//get all other values
			craftedAds[i].title = thisAd.GetString ("title");
			craftedAds[i].text = thisAd.GetString ("text");
			craftedAds[i].link = thisAd.GetString ("link");
		}
		
		Debug.Log (	"craftedAds array in AdSpace: " +
					"\n" + craftedAds);
	}
	*/
	
}
