using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif


//=====================================================================================================================
namespace NetWorkedData
{
	//TODO: FINISH THIS CLASS NWDDateType
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDDateType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDDateType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDDateType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetDate (DateTime sDate)
		{
			Value = sDate.ToLongDateString ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public DateTime ToDate ()
		{
			DateTime rReturn = new DateTime (); 
			DateTime.TryParse (Value, out rReturn);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			GUIStyle tPopupdStyle = new GUIStyle (EditorStyles.popup);
			float tHeight = tPopupdStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
		static string[] kDays = new string[] {
			"01", 
			"02",
			"03",
			"04",
			"05",
			"06",
			"07",
			"08",
			"09",
			"10",
			"11",
			"12",
			"13",
			"14",
			"15",
			"16",
			"17",
			"18",
			"19",
			"20",
			"21",
			"22",
			"23",
			"24",
			"25",
			"26",
			"27",
			"28",
			"29",
			"30",
			"31",
		};
		static string[] kDaysB = new string[] {
			"01", 
			"02",
			"03",
			"04",
			"05",
			"06",
			"07",
			"08",
			"09",
			"10",
			"11",
			"12",
			"13",
			"14",
			"15",
			"16",
			"17",
			"18",
			"19",
			"20",
			"21",
			"22",
			"23",
			"24",
			"25",
			"26",
			"27",
			"28",
			"29",
			"30",
		};static string[] kDaysC = new string[] {
			"01", 
			"02",
			"03",
			"04",
			"05",
			"06",
			"07",
			"08",
			"09",
			"10",
			"11",
			"12",
			"13",
			"14",
			"15",
			"16",
			"17",
			"18",
			"19",
			"20",
			"21",
			"22",
			"23",
			"24",
			"25",
			"26",
			"27",
			"28",
		};static string[] kDaysD = new string[] {
			"01", 
			"02",
			"03",
			"04",
			"05",
			"06",
			"07",
			"08",
			"09",
			"10",
			"11",
			"12",
			"13",
			"14",
			"15",
			"16",
			"17",
			"18",
			"19",
			"20",
			"21",
			"22",
			"23",
			"24",
			"25",
			"26",
			"27",
			"28",
			"29",
		};
		static string[] kMonths = new string[] {
			"january",
			"february",
			"march",
			"april",
			"may",
			"june",
			"july",
			"august",
			"september",
			"october",
			"november",
			"december"
		};
		static string[] kYears = new string[] {
			"1900", "1901", "1902", "1903", "1904", "1905", "1906", "1907", "1908", "1909",
			"1910", "1911", "1912", "1913", "1914", "1915", "1916", "1917", "1918", "1919",
			"1920", "1921", "1922", "1923", "1924", "1925", "1926", "1927", "1928", "1929",
			"1930", "1931", "1932", "1933", "1934", "1935", "1936", "1937", "1938", "1939",
			"1940", "1941", "1942", "1943", "1944", "1945", "1946", "1947", "1948", "1949",
			"1950", "1951", "1952", "1953", "1954", "1955", "1956", "1957", "1958", "1959",
			"1960", "1961", "1962", "1963", "1964", "1965", "1966", "1967", "1968", "1969",
			"1970", "1971", "1972", "1973", "1974", "1975", "1976", "1977", "1978", "1979",
			"1980", "1981", "1982", "1983", "1984", "1985", "1986", "1987", "1988", "1989",
			"1990", "1991", "1992", "1993", "1994", "1995", "1996", "1997", "1998", "1999",
			"2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009",
			"2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019",
			"2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029",
			"2030", "2031", "2032", "2033", "2034", "2035", "2036", "2037", "2038", "2039",

		};

		static string[] kHours = new string[] {
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11",
			"12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23",
		};

		static string[] kMinutes = new string[] {
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
		};

		static string[] kSeconds = new string[] {
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
		};

		//-------------------------------------------------------------------------------------------------------------
		public override object ControlField (Rect sPosition, string sEntitled)
		{
			NWDDateType tTemporary = new NWDDateType ();
			tTemporary.Value = Value;

			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;

			int tNumberOfSubDivision = 3;
			float tWidthSub = Mathf.Ceil ((tWidth - NWDConstants.kFieldMarge * (tNumberOfSubDivision-1)) / tNumberOfSubDivision);

			GUIStyle tPopupdStyle = new GUIStyle (EditorStyles.popup);
			tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight (new GUIContent ("A"), tWidth);

			int tHourIndex = -1;
			int tMinuteIndex = -1;
			int tSecondIndex = -1;

			int tDayIndex = -1;
			int tMonthIndex = -1;
			int tYearIndex = -1;

			tHourIndex = EditorGUI.Popup (new Rect (tX, tY, tWidthSub, tPopupdStyle.fixedHeight),
				sEntitled, tHourIndex, kHours, tPopupdStyle);
			tMinuteIndex = EditorGUI.Popup (new Rect (tX+tWidthSub, tY, tWidthSub, tPopupdStyle.fixedHeight),
				sEntitled, tMinuteIndex, kMinutes, tPopupdStyle);
			tSecondIndex = EditorGUI.Popup (new Rect (tX+tWidthSub*2, tY, tWidthSub, tPopupdStyle.fixedHeight),
				sEntitled, tSecondIndex, kSeconds, tPopupdStyle);

			tDayIndex = EditorGUI.Popup (new Rect (tX, tY, tWidthSub, tPopupdStyle.fixedHeight),
				sEntitled, tDayIndex, kDays, tPopupdStyle);
			tMonthIndex = EditorGUI.Popup (new Rect (tX+tWidthSub, tY, tWidthSub, tPopupdStyle.fixedHeight),
				sEntitled, tMonthIndex, kMonths, tPopupdStyle);
			tYearIndex = EditorGUI.Popup (new Rect (tX+tWidthSub*2, tY, tWidthSub, tPopupdStyle.fixedHeight),
				sEntitled, tYearIndex, kYears, tPopupdStyle);
			
//			if (rIndex != tIndex) 
//			{
//				string tNextValue = tReferenceList.ElementAt (rIndex);
//				tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
//				tTemporary.Value = tNextValue;
//			}

			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================