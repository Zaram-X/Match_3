using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArrayLayout  {

    // A serializable struct to represent one row of boolean values in the grid
	[System.Serializable]
	public struct rowData{
		public bool[] row; // Represents a single row in the grid, using a boolean array
	}

    public Grid grid; // Reference to a Unity Grid component (used for grid-based positioning)
    public rowData[] rows = new rowData[14]; // Array of 14 rows; each row expected to have 7 elements for a 7x7 grid
}
