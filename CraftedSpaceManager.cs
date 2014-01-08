using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for List<> and Dictionary<>
using Boomlagoon.JSON;
using System;
using System.Text;

namespace CraftedInc.AppCrafted
{
	class Container {
		public Dictionary<string, Asset> assets 
			= new Dictionary<string, Asset>();
	}
	class Asset {
		public Dictionary<string, object> attributes 
			= new Dictionary<string, object>();
	}

	class CraftedSpaceManager : MonoBehaviour { //MonoBehaviour required for coroutine
		private string endpoint = "http://api.appcrafted.com/v0/assets/";
		private string accessKey;
		private string secretKey;

		public Dictionary<string, Container> containers = new Dictionary<string, Container>();
		public delegate	void AssetDelegate(Asset asset);
		public static event AssetDelegate OnLoaded;

		//Create a singleton that doesn't need to be attached to a gameobject
		#region Create Singleton
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
					// component-based approach to use coroutine
					Debug.Log ("instantiate a CraftedSpaceManager");
					GameObject go = new GameObject();
					instance = go.AddComponent<CraftedSpaceManager>();
					go.name = "AppCraftedController";
					
				}
				return instance;
			}
		}
		#endregion

		//a public method to register credentials
		public void RegisterCredentials(string accessKey, string secretKey) {
			this.accessKey = accessKey;
			this.secretKey = secretKey;
		}

		//Retrieves the specified Asset.
		public void GetAsset(string containerID, string assetID){
			try {
				Container container = this.containers[containerID];
				Asset asset = container.assets[assetID];
//				if (container != null && asset != null){
					//trigger event OnLoaded
					if (OnLoaded != null) {
						OnLoaded(asset);
					}
//				}
			}
			catch (KeyNotFoundException e){
				StartCoroutine(RetrieveAsset(containerID, assetID));
			}
			 
		}

		//a coroutine that retrives all assets in a container 
		private IEnumerator RetrieveAsset(string containerID, string assetID) {
			Container container = new Container();
			Asset asset = new Asset();
			//validate credentials
			if (this.accessKey == null || this.secretKey == null) {
				throw new System.MemberAccessException("missing credentials");
			}
			Hashtable headers = new Hashtable();
			headers["Authorization"] = "Basic " +
				System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
					this.accessKey + ":" + this.secretKey));
			string url = this.endpoint + containerID + "/all";			
			WWW www = new WWW(url, null, headers);
			yield return www;

			JSONObject JSONObject = JSONObject.Parse(www.text); 

			//process JSON here:
			Debug.Log ("JSON OBJ: " + JSONObject.GetArray("Assets").ToString());
			for (int i = 0; i < JSONObject.GetArray("Assets").Length; i++){
				JSONObject newAsset = JSONObject.Parse(JSONObject.GetArray("Assets")[i].ToString());
				Debug.Log ("newAsset: " + newAsset);
				string userID = newAsset.GetString("UserID");
				asset.attributes.Add("UserID", userID);

			}


			//trigger event OnLoaded
//			if (OnLoaded != null) { OnLoaded(asset); }
		}

//		// GET the JSON file from server and retrive assets from server
//		IEnumerator GetCraftedSpace (string containerID) {
//
//			#region Setting Header for Authorization
//			//header for authorization
//			Hashtable headers = new Hashtable();
//			headers["Authorization"] = "Basic " +
//				System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
//					this.accessKey + ":" + this.secretKey));
//			#endregion
//
//			#region GET data from server
//			//string endpoint = this.endpoint; 
//			string url = this.endpoint + containerID + "/all";
//			
//			WWW www = new WWW(url, null, headers);	// Start a download of the given URL 
//			yield return www;			// wait until the download is done
//			#endregion
//
//			JSONObject thisJSONObject = JSONObject.Parse(www.text); //define the JSON Object
//
//			//initialize craftedAssets array in the craftedSpace
//			craftedSpace.craftedAssets = new CraftedAsset[thisJSONObject.GetArray("Assets").Length];
//
//			for (int i = 0; i < craftedSpace.craftedAssets.Length; i++) {
//				craftedSpace.craftedAssets[i] = new CraftedAsset();
//			}
//
//			//load CraftedAssets (an array of CraftedAsset)
//			for (int i = 0; i < craftedSpace.craftedAssets.Length; i++) {
//				JSONObject thisAsset = JSONObject.Parse(thisJSONObject.GetArray("Assets")[i].ToString());
//				
//				//check if there's an image link
//				if (thisAsset.GetString ("image") != null){
//					//get the image from the image link
//					WWW wwwImage = new WWW(thisAsset.GetString ("image")); // "image" contains a link (string) to the image
//					yield return wwwImage;
//					craftedSpace.craftedAssets[i].image = wwwImage.texture;
//				}
//			}
//		}

		//unload previously loaded assets (particularly images) from memory
		private void UnloadCraftedSpaceFromMemory(CraftedSpace craftedSpace) { //ToDo: need to re-evaluate, not generic enough 
			for (int i = 0; i < craftedSpace.craftedAssets.Length; i++) {
				Destroy(craftedSpace.craftedAssets[i].image);	//removes the image texture from memory
			}
			
		}
	}

}