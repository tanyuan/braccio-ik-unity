using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RotateMesh : EditorWindow {

	private string error = "";

	[MenuItem("Window/Rotate Mesh %#r")]
	public static void ShowWindow() {
		EditorWindow.GetWindow(typeof(RotateMesh));
	}

	void OnGUI() {
		Transform curr = Selection.activeTransform;
		GUILayout.Label ("Creates a clone of the game object with a rotated mesh\n" + 
			"so that the rotation will be (0,0,0) and the scale will\nbe (1,1,1).");
		GUILayout.Space(20);

		if (GUILayout.Button ("Rotate Mesh")) {
			error = "";
			RotateTheMesh();
		}

		GUILayout.Space(20);
		GUILayout.Label(error);
	}

	void RotateTheMesh() {
		List<Transform> children = new List<Transform>();
		Transform curr = Selection.activeTransform;

		MeshFilter mf;
		if (curr == null) {
			error = "No appropriate object selected.";
			Debug.Log (error);    
			return;
		}

		if (curr.localScale.x < 0.0 || curr.localScale.y < 0.0f || curr.localScale.z < 0.0f) {
			error = "Cannot process game objecrt with negative scale values.";
			Debug.Log (error);
			return;
		}

		mf = curr.GetComponent<MeshFilter>();
		if (mf == null || mf.sharedMesh == null) {
			error = "No mesh on the selected object";
			Debug.Log (error);
			return;
		}

		// Create the duplicate game object
		GameObject go = Instantiate (curr.gameObject) as GameObject;
		mf = go.GetComponent<MeshFilter>();
		mf.sharedMesh = Instantiate (mf.sharedMesh) as Mesh;
		curr = go.transform;

		// Disconnect any child objects and same them for later
		foreach (Transform child in curr) {
			if (child != curr) {
				children.Add (child);
				child.parent = null;
			}
		}

		// Rotate and scale the mesh
		Vector3[] vertices = mf.sharedMesh.vertices;
		for (int i = 0; i < vertices.Length; i++) {
			vertices[i] = curr.TransformPoint(vertices[i]) - curr.position;
		}
		mf.sharedMesh.vertices = vertices;


		// Fix the normals
		Vector3[] normals = mf.sharedMesh.normals;
		if (normals != null) {
			for (int i = 0; i < normals.Length; i++)
				normals[i] = curr.rotation * normals[i];
		}
		mf.sharedMesh.normals = normals;
		mf.sharedMesh.RecalculateBounds();

		curr.transform.rotation = Quaternion.identity;
		curr.localScale = new Vector3(1,1,1);

		// Restore the children
		foreach (Transform child in children) {
			child.parent = curr;
		}

		// Set selection to new game object
		Selection.activeObject = curr.gameObject;

		//--- Do a rudamentary fixup of mesh, box, and sphere colliders----
		MeshCollider mc = curr.GetComponent<MeshCollider>();
		if (mc != null) {
			mc.sharedMesh = mf.sharedMesh;
		}

		BoxCollider bc = curr.GetComponent<BoxCollider>();
		if (bc != null) {
			DestroyImmediate(bc);
			curr.gameObject.AddComponent<BoxCollider>();
		}
		SphereCollider sc = curr.GetComponent<SphereCollider>();
		if (sc != null) {
			DestroyImmediate(sc);
			curr.gameObject.AddComponent<SphereCollider>();
		}

		if (curr.GetComponent<Collider>()) {
			error = "Be sure to verify size of collider.";
		}

		// Save a copy to disk
		string name = "Assets/Editor/"+go.name+Random.Range (0, int.MaxValue).ToString()+".asset";
		AssetDatabase.CreateAsset(mf.sharedMesh, name);
		AssetDatabase.SaveAssets();
	}
}