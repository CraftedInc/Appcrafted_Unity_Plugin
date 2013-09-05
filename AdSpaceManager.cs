using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for List<> http://msdn.microsoft.com/en-us/library/6sh2ey19.aspx
using Boomlagoon.JSON;

public class AdSpaceManager : MonoBehaviour 
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
	
	void registerAdSpaces(params string[] adSpaceIDs)
	{
		
	}
	
	void loadAd(string adSpaceID)
	{
		
	}
	
	void loadAd(string adSpaceID, int adNumber)
	{
		
	}
	
	void registerImpression(string adSpaceID, int adNumber)
	{
		
	}
	
	void registerClick(string adSpaceID, int adNumber)
	{
		
	}

	
}
	

/*
	
	//public string AdSpaceID = "e9dd3530-4b4d-4ac6-a073-2a690acf5b2f";
	
	string JSONurl = "http://api.adcrafted.com/adspace/276f4c14-4b05-4ceb-b89e-e54243790057/ad";
	
	public string AdSpaceTitle;
	public string AdSpaceText;
	public string AdSpaceImageLink;
	public string AdSpaceLink;
	public Texture2D AdSpaceImage = new Texture2D(4, 4, TextureFormat.PVRTC_RGBA2, false);
	
	private string AdSpaceTitle0;
	private string AdSpaceText0;
	private string AdSpaceImageLink0;
	private string AdSpaceLink0;
	private Texture2D AdSpaceImage0 = new Texture2D(4, 4, TextureFormat.PVRTC_RGBA2, false);
	
	private string AdSpaceTitle1;
	private string AdSpaceText1;
	private string AdSpaceImageLink1;
	private string AdSpaceLink1;
	private Texture2D AdSpaceImage1 = new Texture2D(4, 4, TextureFormat.PVRTC_RGBA2, false);	
	
	public bool initialized = false;
	public int AdNumber = 0;

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
		
	void Awake (){
		DontDestroyOnLoad(instance);
		//LoadNewAd();
	}
	
	IEnumerator LoadJSON () {
				
		
		//load JSON into a string
		string JSONString;
		WWW wwwText = new WWW(JSONurl);	// Start a download of the given URL
		yield return wwwText;			// wait until the download is done
		JSONString = wwwText.text;
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
					
		
		//Debug: check if values are correct
		Debug.Log(	"Ad: " 	+
					"\n\"title\": "	+ 	AdSpaceTitle0	+
					"\n\"text\": "	+ 	AdSpaceText0	+
					"\n\"imagelink\": "	+ 	AdSpaceImageLink0	+
					//"\n\"image\": "	+ 	AdSpaceImage0.name	+
					"\n\"link\": "	+ 	AdSpaceLink0	);
		
		//Debug: check if values are correct
		Debug.Log(	"Ad: " 	+
					"\n\"title\": "	+ 	AdSpaceTitle1	+
					"\n\"text\": "	+ 	AdSpaceText1	+
					"\n\"imagelink\": "	+ 	AdSpaceImageLink1	+
					//"\n\"image\": "	+ 	AdSpaceImage1.name	+
					"\n\"link\": "	+ 	AdSpaceLink1	);
		#endregion
		
	}
	
	public void LoadNewAd() {
		
		//test to see if any ad has been loaded.
		if (!initialized){
			
			if (AdSpaceImage!=null || AdSpaceImage0!=null || AdSpaceImage1!=null) {	//used in conjuction with www.texture so that memory doesn't pile up
				Debug.Log ("remove current NextImage");
				Destroy (AdSpaceImage); 
				Destroy (AdSpaceImage0); 
				Destroy (AdSpaceImage1); 
			}
			
			initialized = true;
			instance.StartCoroutine("LoadJSON");
			Debug.Log("loading init ad");
			
			
		} else {			
			
			if (AdNumber==0){
				AdNumber = 1;
				AdSpaceTitle = AdSpaceTitle1;
				AdSpaceText = AdSpaceText1;
				AdSpaceLink = AdSpaceLink1;
				AdSpaceImage = AdSpaceImage1;				
			} else {
				AdNumber = 0;	
				AdSpaceTitle = AdSpaceTitle0;
				AdSpaceText = AdSpaceText0;
				AdSpaceLink = AdSpaceLink0;
				AdSpaceImage = AdSpaceImage0;
			}
			
			
			
		}
	}
	
	
	
	
	
	
	
	
	
*/
