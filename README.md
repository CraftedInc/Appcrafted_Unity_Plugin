# AppCrafted - Unity Plugin
## How to use this plugin?
### Method 1:
#### Step 1:
In the Start() method of a component, add these two lines:

    AdSpaceManager.Instance.registerCredentials("[your access key]", "[your secret key]");
    AdSpaceManager.Instance.registerAdSpacesFromApp("[your adspace ID]");

#### Step 2:
Define a variable that holds a CraftedAd array. For example:

	private	CraftedAd[] currentAds;

#### Step 3:
In your script, add the following code:

	#region AdSpaceManager Delegation
	void OnEnable() { AdSpaceManager.OnAssetDownloaded += GetAds; }
	void OnDisable() { AdSpaceManager.OnAssetDownloaded -= GetAds; }
	void GetAds(string adSpaceID) { this.currentAds = AdSpaceManager.Instance.getAllAds(adSpaceID); }
	#endregion

*replace "this.currentAds" with your own variable

#### Step 4:
Now you can access assets from the server with:

	currentAds[index].title
	currentAds[index].text
	currentAds[index].image
	currentAds[index].link

#### Step 5:
To use the image, assign the image to a texture on a material. For example:

	if (currentAds[this.index].image != null) { transform.renderer.material.mainTexture = currentAds[this.index].image; }

### Method 2
#### Step 1:
Add this block of code into your script:

	IEnumerator GetAds(string adSpaceID) {
		yield return StartCoroutine(AdSpaceManager.Instance.registerAdSpaces(adSpaceID));
		this.currentAds = AdSpaceManager.Instance.getAllAds(adSpaceID); 
	}

#### Step 2:
In the Start() method, add these two lines:

	AdSpaceManager.Instance.registerCredentials("[your access key]", "[your secret key]");
	StartCoroutine(GetAds("[your adspace ID]"));
#### Step 3:
Define a variable that holds a CraftedAd array. For example:

	private	CraftedAd[] currentAds;

#### Step 4:
Now you can access assets from the server with:

	currentAds[index].title
	currentAds[index].text
	currentAds[index].image
	currentAds[index].link

#### Step 5:
To use the image, assign the image to a texture on a material. For example:

	if (currentAds[this.index].image != null) { transform.renderer.material.mainTexture = currentAds[this.index].image; }

### Method 3
#### Step 1:
In the Start() method of a component, add these two lines:

    AdSpaceManager.Instance.registerCredentials("[your access key]", "[your secret key]");
    AdSpaceManager.Instance.registerAdSpacesFromApp("[your adspace ID]");
#### Step 2:
You can access assets from the server with:

	AdSpaceManager.Instance.getAd([your adSpace ID], [ad ID]).title
	AdSpaceManager.Instance.getAd([your adSpace ID], [ad ID]).text
	AdSpaceManager.Instance.getAd([your adSpace ID], [ad ID]).image
	AdSpaceManager.Instance.getAd([your adSpace ID], [ad ID]).link

Alternatively you can use:

	AdSpaceManager.Instance.getAllAds([your adSpace ID])[index].title
	AdSpaceManager.Instance.getAd([your adSpace ID])[index].text
	AdSpaceManager.Instance.getAd([your adSpace ID])[index].image
	AdSpaceManager.Instance.getAd([your adSpace ID])[index].link

#### Step 3:
To use the image, assign the image to a texture on a material. For example:

	if (currentAds[this.index].image != null) { transform.renderer.material.mainTexture = currentAds[this.index].image; }

