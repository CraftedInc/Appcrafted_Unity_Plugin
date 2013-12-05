using UnityEngine;
using System.Collections;

public class CraftedAd {
	public string title;
	public string text;
	public string imageLink;
	public Texture2D image = new Texture2D(4, 4, TextureFormat.PVRTC_RGBA2, false);
	public string link;

	public CraftedAd (string title, string text, string imageLink, Texture2D image, string link)
	{
		this.title = title;
		this.text = text;
		this.imageLink = imageLink;
		this.image = image;
		this.link = link;
	}

	public CraftedAd () { }
}