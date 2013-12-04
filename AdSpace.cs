using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class AdSpace
{
	public CraftedAd[] craftedAds;
	public string adSpaceID;

	public AdSpace(string adSpaceID)
	{
		//this.craftedAds = craftedAds;
		//this.adSpaceID = adSpaceID;

		this.adSpaceID = adSpaceID;
		//initialize craftedAds array
		craftedAds = new CraftedAd[1];	// initialize to 1 ad in the array
		for (int i = 0; i < craftedAds.Length; i++)
		{
			craftedAds[i] = new CraftedAd();
		}

	}


	public AdSpace()
	{
		//this.craftedAds = null;
		//this.adSpaceID = null;


		this.adSpaceID = "n/a";
		//initialize craftedAds array
		craftedAds = new CraftedAd[1];	// initialize to 1 ad in the array
		for (int i = 0; i < craftedAds.Length; i++)
		{
			craftedAds[i] = new CraftedAd();
		}

	}

}
