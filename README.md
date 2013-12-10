# AppCrafted - Unity Plugin
## How to use this plugin?

First make sure you are using the AppCrafted namespace at the top of your script (where using UnityEngine and using System.Collections are).
	using UnityEngine;
	using System.Collections;
	using AppCrafted;

Note in the code above, the first two lines are usually generated automatically by Unity when you create a new C# script.

### Method 1:
#### Step 1:
In the Start() method of a component, add these two lines:

    CraftedSpaceManager.Instance.registerCredentials("[your access key]", "[your secret key]");
    CraftedSpaceManager.Instance.registerCraftedSpacesFromApp("[your craftedSpace ID]");

#### Step 2:
Define a variable that holds a CraftedAsset array. For example:

	private	CraftedAsset[] currentAssets;

#### Step 3:
In your script, add the following code:

	#region CraftedSpaceManager Delegation
	void OnEnable() { CraftedSpaceManager.OnAssetDownloaded += GetAssets; }
	void OnDisable() { CraftedSpaceManager.OnAssetDownloaded -= GetAssets; }
	void GetAssets(string craftedSpaceID) { this.currentAssets = CraftedSpaceManager.Instance.getAllAssets(craftedSpaceID); }
	#endregion

*replace "this.currentAssets" with your own variable

#### Step 4:
Now you can access assets from the server with:

	currentAssets[index].title
	currentAssets[index].text
	currentAssets[index].image
	currentAssets[index].link

#### Step 5:
To use the image, assign the image to a texture on a material. For example:

	if (currentAssets[this.index].image != null) { transform.renderer.material.mainTexture = currentAssets[this.index].image; }

### Method 2
#### Step 1:
Add this block of code into your script:

	IEnumerator GetAssets(string craftedSpaceID) {
		yield return StartCoroutine(CraftedSpaceManager.Instance.registerCraftedSpaces(craftedSpaceID));
		this.currentAssets = CraftedSpaceManager.Instance.getAllAssets(craftedSpaceID); 
	}

#### Step 2:
In the Start() method, add these two lines:

	CraftedSpaceManager.Instance.registerCredentials("[your access key]", "[your secret key]");
	StartCoroutine(GetAssets("[your craftedSpace ID]"));
#### Step 3:
Define a variable that holds a CraftedAsset array. For example:

	private	CraftedAsset[] currentAssets;

#### Step 4:
Now you can access assets from the server with:

	currentAssets[index].title
	currentAssets[index].text
	currentAssets[index].image
	currentAssets[index].link

#### Step 5:
To use the image, assign the image to a texture on a material. For example:

	if (currentAssets[this.index].image != null) { transform.renderer.material.mainTexture = currentAssets[this.index].image; }

### Method 3
#### Step 1:
In the Start() method of a component, add these two lines:

    CraftedSpaceManager.Instance.registerCredentials("[your access key]", "[your secret key]");
    CraftedSpaceManager.Instance.registerCraftedSpacesFromApp("[your craftedSpace ID]");
#### Step 2:
You can access assets from the server with:

	CraftedSpaceManager.Instance.getAsset([your craftedSpace ID], [ad ID]).title
	CraftedSpaceManager.Instance.getAsset([your craftedSpace ID], [ad ID]).text
	CraftedSpaceManager.Instance.getAsset([your craftedSpace ID], [ad ID]).image
	CraftedSpaceManager.Instance.getAsset([your craftedSpace ID], [ad ID]).link

Alternatively you can use:

	CraftedSpaceManager.Instance.getAllAssets([your craftedSpace ID])[index].title
	CraftedSpaceManager.Instance.getAsset([your craftedSpace ID])[index].text
	CraftedSpaceManager.Instance.getAsset([your craftedSpace ID])[index].image
	CraftedSpaceManager.Instance.getAsset([your craftedSpace ID])[index].link

#### Step 3:
To use the image, assign the image to a texture on a material. For example:

	if (currentAssets[this.index].image != null) { transform.renderer.material.mainTexture = currentAssets[this.index].image; }

