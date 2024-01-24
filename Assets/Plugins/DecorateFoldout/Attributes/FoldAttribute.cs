using UnityEngine;

namespace UnityInspector
{
	public class FoldoutAttribute : PropertyAttribute
	{
		public string name;
		public bool foldEverything;
		public bool readOnly;
		public bool styled;

		/// <summary>Adds the property to the specified foldout group.</summary>
		/// <param name="_name">Name of the foldout group.</param>
		/// <param name="_foldEverything">Toggle to put all properties to the specified group.</param>
		/// <param name="_readOnly">Defines if field are editable.</param>
		public FoldoutAttribute(string _name, bool _foldEverything = false, bool _readOnly = false, bool _styled = false)
		{
			foldEverything = _foldEverything;
			name           = _name;
			readOnly = _readOnly;
			styled = _styled;
		}
	}

	public class ReadOnlyAttribute : PropertyAttribute { }
}