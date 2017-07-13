
using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;
using System.IO;

namespace NetWorkedData
{
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		#if UNITY_EDITOR

		static Editor mGameObjectEditor;

		public static void DrawInEditor ()
		{
			DrawTableEditor ();

		}
		#endif
	}
}