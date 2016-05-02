using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace sugi.cc
{
	public class TextureViewer : MonoBehaviour
	{
		public static KeyCode showKey = KeyCode.T;
		#region Instance
		static TextureViewer Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = FindObjectOfType<TextureViewer>();
					if (_Instance == null)
						_Instance = new GameObject("TextureViewer").AddComponent<TextureViewer>();
				}
				return _Instance;
			}
		}
		static TextureViewer _Instance;
		#endregion

		public static void AddTexture(Texture tex)
		{
			Instance.Add(tex);
		}

		bool show;
		int viewingIndex;
		List<Texture> textureList = new List<Texture>();
		Rect windowRect = Rect.MinMaxRect(0, 0, 512f, 512f);
		Texture currentTex;

		void Add(Texture tex)
		{
			textureList = textureList.Where(b => b != null).ToList();
			textureList.Add(tex);
		}
		void SetCurrentTex()
		{
			viewingIndex = (int)Mathf.Repeat((float)viewingIndex, (float)textureList.Count);
			currentTex = textureList[viewingIndex];
			windowRect.width = currentTex.width + 32;
			windowRect.height = currentTex.height + 32;
		}
		void Update()
		{
			if (show)
			{
				if (Input.GetKeyDown(KeyCode.LeftArrow))
					viewingIndex--;
				else if (Input.GetKeyDown(KeyCode.RightArrow))
					viewingIndex++;
				SetCurrentTex();
			}

			if (!Input.GetKeyDown(showKey) || textureList.Count == 0)
				return;
			show = !show;
			Cursor.visible = show;
			if (show)
				SetCurrentTex();
		}
		void OnGUI()
		{
			if (!show || textureList.Count == 0)
				return;
			windowRect = GUI.Window(1, windowRect, OnWindow, currentTex.name);
		}
		void OnWindow(int id)
		{
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			GUILayout.Label(currentTex);

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUI.DragWindow();
		}
	}
}
