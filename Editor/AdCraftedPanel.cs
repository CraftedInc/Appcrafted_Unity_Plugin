/*

using UnityEngine;
using UnityEditor;

public class AdCraftedPanel : EditorWindow {

	string AdSpaceID = "2766b754-b673-43f2-9be7-c9ab7bc6d60f";
	
	// Add menu named "AdCrafted Panel" to the Window menu
	[MenuItem ("Window/AdCrafted Panel")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		AdCraftedPanel window = (AdCraftedPanel)EditorWindow.GetWindow (typeof (AdCraftedPanel));
	}
	
	void OnGUI () {
		GUILayout.Label ("AdCrafted Settings", EditorStyles.boldLabel);
			AdSpaceID = EditorGUILayout.TextField ("AdSpace ID", AdSpaceID);
		
	}
	
}

*/