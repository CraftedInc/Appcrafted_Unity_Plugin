using UnityEngine;
using System.Collections;
using CraftedInc.Appcrafted;

public class example : MonoBehaviour {

	public GUIText GUITextToUpdate;
	private string updatedText;
	private Texture2D updatedImage;
	private string assetBundleURL;
	private AssetBundle assetBundle;
	private GameObject newObject;
	private string currentAssetURL;

#region AppcraftedManager Delegate
	void OnEnable()
	{
		AppcraftedManager.OnLoaded += AssignAsset;
	}
	void OnDisable()
	{
		AppcraftedManager.OnLoaded -= AssignAsset;
	}
	void AssignAsset(Asset asset){
		if (asset.attributes.ContainsKey("name")) {
			GUITextToUpdate.text = asset.attributes["name"] as string;
		}
		if (asset.attributes.ContainsKey("image")) {
			updatedImage = asset.attributes["image"] as Texture2D ;
			transform.renderer.material.mainTexture = updatedImage;
		}
		if (asset.attributes.ContainsKey("asset")) {
			assetBundleURL = asset.attributes["asset"] as string;
			if (currentAssetURL != assetBundleURL) {	//check if url to the file has changed
				StartCoroutine(NewObject());
				currentAssetURL = assetBundleURL;
			}
		}
	}
#endregion

	// Use this for initialization
	void Start () 
	{
		//Appcrafted setup
		AppcraftedManager.Instance.RegisterCredentials("ZjBSG4syQacVGyUK4aSA", "mAefTO1KUpe48G1aNqps");
		AppcraftedManager.Instance.GetAsset("U3y4S2HD23HnNwWb60ip", "ninja");
	}

	void Update()
	{
		if (Input.GetKeyDown("r")) {
			AppcraftedManager.Instance.ClearAssetCache ("U3y4S2HD23HnNwWb60ip", "ninja");
		}
		if (Input.GetKeyDown("1")) {
			AppcraftedManager.Instance.GetAsset("U3y4S2HD23HnNwWb60ip", "ninja");
		}
		if (Input.GetKeyDown("2")) {
			AppcraftedManager.Instance.GetAsset("U3y4S2HD23HnNwWb60ip", "banana");
		}
		if ( Input.GetKeyDown("3") && assetBundle != null) {
			Debug.Log ("instantiating asset");
			if (newObject != null)
				Destroy(newObject);
			newObject = Instantiate(assetBundle.mainAsset) as GameObject;
			newObject.transform.position = new Vector3(0,3,0);
		}
		RotateCube();
	}

	IEnumerator NewObject(){

		if (this.assetBundle != null && this.assetBundle.Contains(this.assetBundle.mainAsset.name)){
			Debug.Log ("unload asset");
			this.assetBundle.Unload(false);
		}
		WWW www = new WWW(this.assetBundleURL);
		yield return www;	

		this.assetBundle = www.assetBundle;
	}

	void RotateCube(){
		//rotate the cube
		transform.Rotate((Vector3.up * Time.deltaTime * 100), Space.World);
		transform.Rotate((Vector3.left * Time.deltaTime * 50), Space.Self);
		float newScaleUnit = Mathf.PingPong(Time.time*5, 4.5f)+4.5f;
		transform.localScale = new Vector3(newScaleUnit, newScaleUnit, newScaleUnit);
	}

}




