//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

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
	//TODO: FINISH THIS CLASS NWDDateTimeType
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDDateTimeType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDDateTimeType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDDateTimeType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string[] kDays = new string[] {
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
		public static string[] kDaysB = new string[] {
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
		};
		public static string[] kDaysC = new string[] {
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
		};
		public static string[] kDaysD = new string[] {
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
		public static string[] kDayNames = new string[] {
			"Monday",
			"Tuesday",
			"Wednesday",
			"Thursday",
			"Friday",
			"Saturday",
			"Sunday"
		};
		public static string[] kMonths = new string[] {
			"January",
			"February",
			"March",
			"April",
			"May",
			"June",
			"July",
			"August",
			"September",
			"October",
			"November",
			"December"
		};
        public static int kYearStart = 2010;
		public static string[] kYears = new string[] {
			//"1900", "1901", "1902", "1903", "1904", "1905", "1906", "1907", "1908", "1909",
			//"1910", "1911", "1912", "1913", "1914", "1915", "1916", "1917", "1918", "1919",
			//"1920", "1921", "1922", "1923", "1924", "1925", "1926", "1927", "1928", "1929",
			//"1930", "1931", "1932", "1933", "1934", "1935", "1936", "1937", "1938", "1939",
			//"1940", "1941", "1942", "1943", "1944", "1945", "1946", "1947", "1948", "1949",
			//"1950", "1951", "1952", "1953", "1954", "1955", "1956", "1957", "1958", "1959",
			//"1960", "1961", "1962", "1963", "1964", "1965", "1966", "1967", "1968", "1969",
			//"1970", "1971", "1972", "1973", "1974", "1975", "1976", "1977", "1978", "1979",
			//"1980", "1981", "1982", "1983", "1984", "1985", "1986", "1987", "1988", "1989",
			//"1990", "1991", "1992", "1993", "1994", "1995", "1996", "1997", "1998", "1999",
			//"2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009",
			"2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019",
			"2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029",
            "2030", "2031", "2032", "2033", "2034", "2035", "2036", "2037", "2038", "2039",
            "2040", "2041", "2042", "2043", "2044", "2045", "2046", "2047", "2048", "2049",
            "2050", "2051", "2052", "2053", "2054", "2055", "2056", "2057", "2058", "2059",

		};

		public static string[] kHours = new string[] {
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11",
			"12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23",
		};

		public static string[] kMinutes = new string[] {
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
		};

		public static string[] kSeconds = new string[] {
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
		};
		//-------------------------------------------------------------------------------------------------------------
		public void SetDateTime (DateTime sDatetime)
		{
			Value = sDatetime.Year+NWDConstants.kFieldSeparatorA+
				sDatetime.Month+NWDConstants.kFieldSeparatorA+
				sDatetime.Day+NWDConstants.kFieldSeparatorA+
				sDatetime.Hour+NWDConstants.kFieldSeparatorA+
				sDatetime.Minute+NWDConstants.kFieldSeparatorA+
				sDatetime.Second;
		}
		//-------------------------------------------------------------------------------------------------------------
		public DateTime ToDateTime ()
		{
			string[] tDateComponent=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
			int tYear = 1970;
			int tMonth = 1;
			int tDay = 1;
			int tHour = 0;
			int tMinute = 0;
			int tSecond = 0;
			if (tDateComponent.Count() == 6) {
				int.TryParse(tDateComponent [0], out tYear);
				int.TryParse(tDateComponent [1], out tMonth);
				int.TryParse(tDateComponent [2],out tDay);
				int.TryParse(tDateComponent [3], out tHour);
				int.TryParse(tDateComponent [4], out tMinute);
				int.TryParse(tDateComponent [5], out tSecond);
			}
			// test result of parsing 
			if (tYear < 1 || tYear > 3000) {
				tYear = 1970;
			}
			if (tMonth < 1 || tMonth > 12) {
				tMonth = 1;
			}
			if (tDay < 1) {
				tDay = 1;
			}
			int tDaysTest = DateTime.DaysInMonth (tYear, tMonth);
			if (tDay > tDaysTest) {
				tDay = tDaysTest;
			}
			if (tHour < 0 || tHour > 23 ) {
				tHour = 0;
			}
			if (tMinute < 0 || tMinute > 59 ) {
				tMinute = 0;
			}
			if (tSecond < 0 || tSecond > 59 ) {
				tSecond = 0;
			}

			DateTime rReturn = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
            return NWDConstants.kPopupdStyle.fixedHeight*2 + NWDConstants.kFieldMarge;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPos, string sEntitled, string sTooltips = "")
		{
            NWDDateTimeType tTemporary = new NWDDateTimeType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            float tHeight = NWDConstants.kPopupdStyle.fixedHeight;
			string[] tDateComponent=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
			int tYear = 1970;
			int tMonth = 1;
			int tDay = 1;
			int tHour = 0;
			int tMinute = 0;
			int tSecond = 0;
			if (tDateComponent.Count() == 6) {
				int.TryParse(tDateComponent [0], out tYear);
				int.TryParse(tDateComponent [1], out tMonth);
				int.TryParse(tDateComponent [2],out tDay);
				int.TryParse(tDateComponent [3], out tHour);
				int.TryParse(tDateComponent [4], out tMinute);
				int.TryParse(tDateComponent [5], out tSecond);
			}
			// test result of parsing 
			if (tYear < 1 || tYear > 3000) {
				tYear = 1970;
			}
			if (tMonth < 1 || tMonth > 12) {
				tMonth = 1;
			}
			if (tDay < 1) {
				tDay = 1;
			}
			int tDaysTest = DateTime.DaysInMonth (tYear, tMonth);
			if (tDay > tDaysTest) {
				tDay = tDaysTest;
			}
			if (tHour < 0 || tHour > 23 ) {
				tHour = 0;
			}
			if (tMinute < 0 || tMinute > 59 ) {
				tMinute = 0;
			}
			if (tSecond < 0 || tSecond > 59 ) {
				tSecond = 0;
			}

			float tX = sPos.x + EditorGUIUtility.labelWidth;

			DateTime tDateTime = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond);

			float tTiersWidth = Mathf.Ceil( (sPos.width - EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge) / 3.0F);
			float tTiersWidthB = tTiersWidth - NWDConstants.kFieldMarge;
//			float tTiersWidthC = tTiersWidth - NWDConstants.kFieldMarge*3;
			float tHeightAdd = 0;

			float tWidthYear = tTiersWidthB + 10;
			float tWidthMonth = tTiersWidthB -5;
			float tWidthDay = tTiersWidthB -5;
            EditorGUI.LabelField (new Rect (sPos.x, sPos.y, sPos.width, NWDConstants.kLabelStyle.fixedHeight), tContent);

            // remove EditorGUI.indentLevel to draw next controller without indent 
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;


            tYear = NWDDateTimeType.kYearStart+ EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tWidthYear, NWDConstants.kPopupdStyle.fixedHeight),
                                                                 tDateTime.Year - NWDDateTimeType.kYearStart, NWDDateTimeType.kYears);

            tMonth = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +NWDConstants.kFieldMarge, sPos.y + tHeightAdd, tWidthMonth, NWDConstants.kPopupdStyle.fixedHeight),
				tDateTime.Month-1, NWDDateTimeType.kMonths);

			int tDayNumber = DateTime.DaysInMonth(tYear,tMonth);

			if (tDayNumber == 31 )
			{
                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tDateTime.Day-1, NWDDateTimeType.kDays);
			}
			else if (tDayNumber == 30)
			{
                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tDateTime.Day-1, NWDDateTimeType.kDaysB);
			}
			else if (tDayNumber == 28)
			{
                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tDateTime.Day-1, NWDDateTimeType.kDaysC);
			}
			else if (tDayNumber == 29)
			{
                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tDateTime.Day-1, NWDDateTimeType.kDaysD);
			}

			tHeightAdd += tHeight + NWDConstants.kFieldMarge;

            GUI.Label (new Rect (tX , sPos.y+tHeightAdd,tTiersWidthB*2+NWDConstants.kFieldMarge-2, NWDConstants.kSeparatorStyle.fixedHeight), ":",NWDConstants.kSeparatorStyle);
            GUI.Label (new Rect (tX + tTiersWidthB + NWDConstants.kFieldMarge, sPos.y+tHeightAdd, tTiersWidthB*2+NWDConstants.kFieldMarge-2, NWDConstants.kSeparatorStyle.fixedHeight), ":",NWDConstants.kSeparatorStyle);

            tHour = EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB, NWDConstants.kPopupdStyle.fixedHeight),
				tDateTime.Hour, NWDDateTimeType.kHours);
            tMinute = EditorGUI.Popup (new Rect (tX +tTiersWidth, sPos.y + tHeightAdd, tTiersWidthB, NWDConstants.kPopupdStyle.fixedHeight),
				tDateTime.Minute, NWDDateTimeType.kMinutes);
            tSecond = EditorGUI.Popup (new Rect (tX +tTiersWidth*2, sPos.y + tHeightAdd, tTiersWidthB, NWDConstants.kPopupdStyle.fixedHeight),
				tDateTime.Second, NWDDateTimeType.kSeconds);
			tTemporary.Value = tYear+NWDConstants.kFieldSeparatorA+
				tMonth+NWDConstants.kFieldSeparatorA+
				tDay+NWDConstants.kFieldSeparatorA+
				tHour+NWDConstants.kFieldSeparatorA+
				tMinute+NWDConstants.kFieldSeparatorA+
				tSecond;

			//GUI.Label (new Rect (sPos.x, sPos.y+tHeightAdd, sPos.width, sPos.height), Value);

            // move EditorGUI.indentLevel to draw next controller with indent 
            EditorGUI.indentLevel = tIndentLevel;

			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================