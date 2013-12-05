using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class AdSpace {
	public CraftedAd[] craftedAds;
	public string adSpaceID;

	public AdSpace(string adSpaceID)
	{
		this.adSpaceID = adSpaceID;
	}
	public AdSpace() { }
}
