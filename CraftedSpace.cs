using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

namespace CraftedInc.AppCrafted
{
	class CraftedSpace {
		public CraftedAsset[] craftedAssets;
		public string craftedSpaceID;
		
		public CraftedSpace(string craftedSpaceID)
		{
			this.craftedSpaceID = craftedSpaceID;
			//this.craftedAssets = new CraftedAsset[2];
		}
		public CraftedSpace() { }
	}
}