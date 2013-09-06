using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class AdSpace
{
	public CraftedAd[] craftedAds;
	public string adSpaceID;
	public int indexOfAdToShow;
	
	public AdSpace(string ID)
	{
		adSpaceID = ID;
		//initialize craftedAds array
		craftedAds = new CraftedAd[1];	// initialize to 1 ad in the array
		for (int i = 0; i < craftedAds.Length; i++)
		{
			craftedAds[i] = new CraftedAd();
		}
		
		indexOfAdToShow = 0;
	}
	
	public AdSpace()
	{
		adSpaceID = "n/a";
		//initialize craftedAds array
		craftedAds = new CraftedAd[1];	// initialize to 1 ad in the array
		for (int i = 0; i < craftedAds.Length; i++)
		{
			craftedAds[i] = new CraftedAd();
		}
		
		indexOfAdToShow = 0;
	}
}
