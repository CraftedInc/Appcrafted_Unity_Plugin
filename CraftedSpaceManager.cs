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
					//trigger event OnLoaded
					if (OnLoaded != null) {
						OnLoaded(this.containers[containerID].assets[assetID]);
					}
			}
			catch (KeyNotFoundException e){
				StartCoroutine(RetrieveAsset(containerID, assetID));
			}
			 
		}

		//a coroutine that retrives all assets in a container 
		private IEnumerator RetrieveAsset(string containerID, string assetID) {
			Container container = new Container();
			Asset asset = new Asset();

			//add container
			this.containers.Add(containerID, container);

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

			JSONObject containerJSON = JSONObject.Parse(www.text); 

			//process JSON here:
			//adding assets
			for (int i = 0; i < containerJSON.GetArray("Assets").Length; i++){

				JSONObject assetJSON = JSONObject.Parse(containerJSON.GetArray("Assets")[i].ToString());

				string currentAssetID = assetJSON.GetString("AssetID");
				this.containers[containerID].assets.Add(currentAssetID, asset);
				Debug.Log ("AssetID: " + currentAssetID);

				//adding attributes
				foreach (var keyValuePair in assetJSON.values) {
//					Debug.Log ("Key: " + keyValuePair.Key + "\nValue: " + keyValuePair.Value);
					if (keyValuePair.Value.Type == JSONValueType.Object){

						string attributeName = keyValuePair.Key;

						JSONObject attributeJSON = JSONObject.Parse(keyValuePair.Value.ToString());
//						Debug.Log ("attributeJSON: " + attributeJSON);
//						Debug.Log ("attributeJSON Type: " + attributeJSON.GetString("Type"));
//						Debug.Log ("attributeJSON Value: " + attributeJSON.GetString("Value"));

						this.containers[containerID]
							.assets[currentAssetID]
							.attributes.Add(attributeName, attributeJSON.GetString ("Value"));
						Debug.Log ("attributeName: " + attributeName 
						           + "\n" 
						           + this.containers[containerID]
						           		 .assets[currentAssetID]
						           		 .attributes[attributeName]);
					}
				}
			}

			//trigger event OnLoaded
			if (OnLoaded != null) {
				Debug.Log ("containerID: " + containerID
				           + "\n" + this.containers[containerID]);
				Debug.Log ("assetID: " + assetID
				           + "\n" + this.containers[containerID].assets[assetID]);
				OnLoaded(this.containers[containerID].assets[assetID]); 
			}
		}


//				//check if there's an image link
//				if (thisAsset.GetString ("image") != null){
//					//get the image from the image link
//					WWW wwwImage = new WWW(thisAsset.GetString ("image")); // "image" contains a link (string) to the image
//					yield return wwwImage;
//					craftedSpace.craftedAssets[i].image = wwwImage.texture;
//				}

		//unload previously loaded assets (particularly images) from memory
		private void UnloadCraftedSpaceFromMemory(CraftedSpace craftedSpace) { //ToDo: need to re-evaluate, not generic enough 
			for (int i = 0; i < craftedSpace.craftedAssets.Length; i++) {
				Destroy(craftedSpace.craftedAssets[i].image);	//removes the image texture from memory
			}
			
		}
	}

}