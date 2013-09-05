using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class AdSpace
{
	public List<CraftedAd> craftedAds = new List<CraftedAd>();
	
	public AdSpace(string adSpaceID)
	{
		string adSpaceJSONUrlForAds = "http://api.adcrafted.com/adspace/" + adSpaceID + "/ad";

	}
	
	
	IEnumerator LoadJSON (string JSONurl) {
				
		//load JSON into a string
		string JSONString;
		WWW www = new WWW(JSONurl);	// Start a download of the given URL
		yield return www;			// wait until the download is done
		JSONString = www.text;
		JSONObject AdSpaceJSONObject = JSONObject.Parse(JSONString); //define the JSON Object
		
		JSONObject AdSpaceJSONObject0 = JSONObject.Parse(AdSpaceJSONObject.GetArray("Ads")[0].ToString());
		JSONObject AdSpaceJSONObject1 = JSONObject.Parse(AdSpaceJSONObject.GetArray("Ads")[1].ToString());
		
		
		//grab key values from JSON Object for the image
		AdSpaceImageLink0 = AdSpaceJSONObject0.GetString("image");
		WWW wwwImage0 = new WWW(AdSpaceImageLink0);
		yield return wwwImage0;
		//wwwImage.LoadImageIntoTexture(AdSpaceImage); //use LoadImageIntoTexture() to replace the contents of an existing Texture2D with an image from the downloaded data. (use either LoadImageIntoTexture() or www.texture, but not both)
		AdSpaceImage0 = wwwImage0.texture; // use www.texture, which returns a Texture2D generated from the downloaded data (Read Only). (use either LoadImageIntoTexture() or www.texture, but not both)
		
		//grab non-image key values from JSON Object
		AdSpaceTitle0 = AdSpaceJSONObject0.GetString("title");
		AdSpaceText0 = AdSpaceJSONObject0.GetString ("text");
		AdSpaceLink0 = AdSpaceJSONObject0.GetString("link");
		
		//grab key values from JSON Object for the image
		AdSpaceImageLink1 = AdSpaceJSONObject1.GetString("image");
		WWW wwwImage1 = new WWW(AdSpaceImageLink1);
		yield return wwwImage1;
		//wwwImage.LoadImageIntoTexture(AdSpaceImage); //use LoadImageIntoTexture() to replace the contents of an existing Texture2D with an image from the downloaded data. (use either LoadImageIntoTexture() or www.texture, but not both)
		AdSpaceImage1 = wwwImage1.texture; // use www.texture, which returns a Texture2D generated from the downloaded data (Read Only). (use either LoadImageIntoTexture() or www.texture, but not both)
		
		//grab non-image key values from JSON Object
		AdSpaceTitle1 = AdSpaceJSONObject1.GetString("title");
		AdSpaceText1 = AdSpaceJSONObject1.GetString ("text");
		AdSpaceLink1 = AdSpaceJSONObject1.GetString("link");
	
		//define initial value of ads to show		
		AdSpaceTitle = AdSpaceTitle0;
		AdSpaceText = AdSpaceText0;
		AdSpaceLink = AdSpaceLink0;
		AdSpaceImage = AdSpaceImage0;
		
		#region ad value debug
		//Debug: check if values are correct
		Debug.Log(	"Ad: " 	+
					"\n\"title\": "	+ 	AdSpaceTitle	+
					"\n\"text\": "	+ 	AdSpaceText	+
					"\n\"imagelink\": "	+ 	AdSpaceImageLink	+
					//"\n\"image\": "	+ 	AdSpaceImage.name	+
					"\n\"link\": "	+ 	AdSpaceLink	);
		#endregion
		
	}
	
	
	
	
}
