using UnityEngine;
using System.Collections;

public class CraftedAd
{
	public string title;
	public string text;
	public string imageLink;
	public Texture2D image = new Texture2D(4, 4, TextureFormat.PVRTC_RGBA2, false);
	public string link;
	
	
	public CraftedAd (string newTitle, string newText, string newImageLink, Texture2D newImage, string newLink)
	{
		title = newTitle;
		text = newText;
		imageLink = newImageLink;
		image = newImage;
		link = newLink;
	}
	
	public CraftedAd ()
	{
		title = "init title value";
		text = "init text value";
		imageLink = "init imagelink value";
		image = null;
		link = "init link value";
	}
	
}