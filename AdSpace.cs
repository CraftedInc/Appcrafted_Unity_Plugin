using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class AdSpace : MonoBehaviour //MonoBehaviour so that we can use Coroutine to yield return www
{
	public CraftedAd[] craftedAds;
	public string adSpaceID;
	
	public AdSpace(string ID)
	{
		adSpaceID = ID;
	}
	
	
}
