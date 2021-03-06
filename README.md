# Appcrafted - Unity plugin / SDK
for [Appcrafted](http://appcrafted.com/)

## How to use this plugin? (C# example) 

#### Step 0 - extract the files
Extract the files into a new folder inside your /Assets folder in your Unity project. To keep things clean, we suggest naming this folder **Appcrafted**. 

#### Step 1 - using Appcrafted namespace:
First make sure you are using the Appcrafted namespace at the top of your script (where "using UnityEngine" and "using System.Collections" usually are).
	
	using CraftedInc.Appcrafted;

#### Step 2 - register your credentials and get your asset:
Before you retrieve assets, you need to register your credentials. 

Use _AppcraftedManager.Instance.RegisterCredentials( **Your Access Key**, **Your Secret Key**)_

For example:

    AppcraftedManager.Instance.RegisterCredentials("Zfasdf4syQacVGyUK4aSA", "mAavs24KUpe48Gqps");

You should be able to find your keys on [Appcrafted](https://developer.appcrafted.com) under [Account > Appcrafted Credentials](https://developer.appcrafted.com/#/account). 

After your credentials have been registered, you can use GetAsset method to get your assets.

_AppcraftedManager.Instance.GetAsset( **container ID**, **asset ID**);_

    AppcraftedManager.Instance.GetAsset("U3y4S2HD23HnNwWb60ip", "ninja");

You can have both _RegisterCredentials()_ and _GetAsset()_ in a _Start()_ when your game/app first launches to start the downloading early.

#### Step 3 - Subscribe to the OnLoaded event to use your assets when download finishes. 

Subscribe to the OnLoaded event, this allows SomeMethod() to be run when the assets are downloaded. 

	#region AppcraftedManager Delegate
	void OnEnable()
	{
		AppcraftedManager.OnLoaded += SomeMethod; //replace SomeMethod with your own method name
	}
	void OnDisable()
	{
		AppcraftedManager.OnLoaded -= SomeMethod;
	}
	void SomeMethod(Asset asset){
		//code to be executed when assets are downloaded
        //for example
        this.asset = asset;
        ninjaName = asset.attributes["name"] as string;
	}
	#endregion

SomeMethod() will subscribe to the OnLoaded event and gets called when the delivery of your assets via the network is finished. Add your code here to process assets when the app finishes downloading them. 

_Note: make sure the method name "SomeMethod" is the same as the ones in OnEnable() and OnDisable(). In OnEnable() we subscribe SomeMethod() to the OnLoaded event, and **in OnDisable() we unsubscribe SomeMethod() from the OnLoaded event to avoid a memory leak**._

Attributes are stored as [Dictionaries](http://msdn.microsoft.com/en-us/library/xfhwa508). Use asset.attributes[ **attribute name** ] to find the value of an attribute. Example:

    ninjaName = asset.attributes["name"] as string;

The values of attribute are of type **object**, so you have to cast to specific types when you use them. In the example above, the value of the "name" attribute is a string, so we add "as string" to the end of the dictionary. 

For example:

    ninjaName = asset.attributes["name"] as string;
    ninjaCrossPromoLink = asset.attributes["link"] as string;
    ninjaHP = asset.attributes["hp"] as double;
    ninjaFace = asset.attributes["face"] as Texture2D;
    ninjaAliases = asset.attributes["aliases"] as string[];
    ninjaTopScores = asset.attributes["topscores"] as double[];

If the attribute is a file (for example, a .unity3d asset bundle), it's a URL and should be cast as a string. You need to retrieve and load the file in your script. 

    ninjaAssetBundle = asset.attributes["assetBundle"] as string;


#### Attribute types

**URL** and **STRING** attributes are `string`.

**FILE** attributes are `string` containing URLs to the files on the CDN (content delivery network). 

**NUMBER** attributes are `double`.

**IMAGE** attributes are `Texture2D`.

**ARRAY(S)** attributes are `string[]`.

**ARRAY(N)** attributes are `double[]`.



