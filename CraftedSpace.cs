using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class CraftedSpace {
	public CraftedAsset[] craftedAssets;
	public string craftedSpaceID;

	public CraftedSpace(string craftedSpaceID)
	{
		this.craftedSpaceID = craftedSpaceID;
	}
	public CraftedSpace() { }
}
