# AppCrafted - Unity plugin / SDK
## How to use this plugin? (C# example) 

#### Step 1 - using AppCrafted:
First make sure you are using the AppCrafted namespace at the top of your script (where "using UnityEngine" and "using System.Collections" usually are).
	
	using CraftedInc.AppCrafted;


#### Step 2 - register your credentials and the CraftedSpaces:
In the Start() method of a component, add these two lines:

    CraftedSpaceManager.Instance.registerCredentials([access key], [secret key]);
    CraftedSpaceManager.Instance.registerCraftedSpaces([CraftedSpace IDs]);

You can find your access key, secret key, and CraftedSpace ID on the web dashboard [http://developer.appcrafted.com/](http://developer.appcrafted.com/). Make sure you put double quotation marks around the IDs as they need to be strings. 

By doing this, you register with the server your credentials and start requesting assets from your CraftedSpaces. You can enter multiple CraftedSpace IDs by separating the strings with a ",". 

Example:
	CraftedSpaceManager.Instance.registerCredentials("ZjBSgd2sdG4syQac4aSA", "mAefTOvxe241KU1aNqps");
	CraftedSpaceManager.Instance.registerCraftedSpaces("QV1rGxGBJab6j3dDK63n", "Hn82kV1rGxGab6j3K63n");

#### Step 3 - Create a delegation
In your script, define a variable that holds a CraftedAsset array. For example:

	private	CraftedAsset[] currentAssets;

And add the following code:

	#region CraftedSpaceManager Delegation
	void OnEnable() { 
		CraftedSpaceManager.OnAssetDownloaded += GetAssets; 
	}
	void OnDisable() { 
		CraftedSpaceManager.OnAssetDownloaded -= GetAssets; 
	}
	void GetAssets(string craftedSpaceID) { 
		this.currentAssets = CraftedSpaceManager.Instance.getAllAssets(craftedSpaceID); 
	}
	#endregion

The GetAssets() method will subscribe to the OnAssetDownloaded event and gets called when the delivery of your assets via the network is finished. You can add additional code here to process your assets if you want them to be processed when the app finishes downloading the assets. 

Note: make sure the method name "GetAssets" is the same as the ones in OnEnable() and OnDisable(). In OnEnable() we subscribe GetAssets() to the OnAssetDownloaded event, and in OnDisable() we unsubscribe GetAssets(() from the OnAssetDownloaded event so that we don't keep it in the memory. 

#### Step 4:
Now you can access assets from the server with:

	currentAssets[index].title
	currentAssets[index].text
	currentAssets[index].image
	currentAssets[index].link

#### Step 5:
To use the image, assign the image to a texture on a material. For example:

	if (currentAssets[this.index].image != null { 
		transform.renderer.material.mainTexture = currentAssets[this.index].image; 
	}

---
 
### Alternative Step 3-5:
Alternatively, after step 1 and step 2, you can access assets directly with the following. However this option does not wait for assets to be downloaded so make sure you check that the assets are not null before using it. 

#### Step 3:
You can access assets with:

	CraftedSpaceManager.Instance.getAsset([CraftedSpace ID], [asset ID]).title
	CraftedSpaceManager.Instance.getAsset([CraftedSpace ID], [asset ID]).text
	CraftedSpaceManager.Instance.getAsset([CraftedSpace ID], [asset ID]).image
	CraftedSpaceManager.Instance.getAsset([CraftedSpace ID], [asset ID]).link

Alternatively you can use:

	CraftedSpaceManager.Instance.getAllAssets([CraftedSpace ID])[index].title
	CraftedSpaceManager.Instance.getAsset([CraftedSpace ID])[index].text
	CraftedSpaceManager.Instance.getAsset([CraftedSpace ID])[index].image
	CraftedSpaceManager.Instance.getAsset([CraftedSpace ID])[index].link

Again, make sure your CraftedSpace IDs and Asset IDs are strings and have quotation marks around them.

#### Step 4:
To use the image, assign the image to a texture on a material. For example:

	if (currentAssets[index].image != null { 
		transform.renderer.material.mainTexture = currentAssets[index].image; 
	}

