//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerContinent : NWEDataTypeMaskGeneric<NWDServerContinent>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDServerContinent All = Add(0, "ALL");
        public static NWDServerContinent Africa = Add(1, "Africa", "AF");
        public static NWDServerContinent Oceania = Add(2, "Oceania", "OC");
        public static NWDServerContinent Asia = Add(3, "Asia", "AS");
        public static NWDServerContinent Europa = Add(4, "Europe", "EU");
        public static NWDServerContinent NorthAmerica = Add(5, "North America", "NA");
        public static NWDServerContinent SouthAmerica = Add(6, "South America", "SA");
        public static NWDServerContinent Antartic = Add(7, "Antartica", "AN");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerCountry : NWEDataTypeMaskGeneric<NWDServerCountry>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDServerCountry A1 = Add(1, "A1", "Anonymous Proxy");
        public static NWDServerCountry A2 = Add(2, "A2", "Satellite Provider");
        public static NWDServerCountry O1 = Add(3, "O1", "Other Country");
        public static NWDServerCountry AD = Add(4, "AD", "Andorra");
        public static NWDServerCountry AE = Add(5, "AE", "United Arab Emirates");
        public static NWDServerCountry AF = Add(6, "AF", "Afghanistan");
        public static NWDServerCountry AG = Add(7, "AG", "Antigua and Barbuda");
        public static NWDServerCountry AI = Add(8, "AI", "Anguilla");
        public static NWDServerCountry AL = Add(9, "AL", "Albania");
        public static NWDServerCountry AM = Add(10, "AM", "Armenia");
        public static NWDServerCountry AO = Add(11, "AO", "Angola");
        public static NWDServerCountry AP = Add(12, "AP", "Asia/Pacific Region");
        public static NWDServerCountry AQ = Add(13, "AQ", "Antarctica");
        public static NWDServerCountry AR = Add(14, "AR", "Argentina");
        public static NWDServerCountry AS = Add(15, "AS", "American Samoa");
        public static NWDServerCountry AT = Add(16, "AT", "Austria");
        public static NWDServerCountry AU = Add(17, "AU", "Australia");
        public static NWDServerCountry AW = Add(18, "AW", "Aruba");
        public static NWDServerCountry AX = Add(19, "AX", "Aland Islands");
        public static NWDServerCountry AZ = Add(20, "AZ", "Azerbaijan");
        public static NWDServerCountry BA = Add(21, "BA", "Bosnia and Herzegovina");
        public static NWDServerCountry BB = Add(22, "BB", "Barbados");
        public static NWDServerCountry BD = Add(23, "BD", "Bangladesh");
        public static NWDServerCountry BE = Add(24, "BE", "Belgium");
        public static NWDServerCountry BF = Add(25, "BF", "Burkina Faso");
        public static NWDServerCountry BG = Add(26, "BG", "Bulgaria");
        public static NWDServerCountry BH = Add(27, "BH", "Bahrain");
        public static NWDServerCountry BI = Add(28, "BI", "Burundi");
        public static NWDServerCountry BJ = Add(29, "BJ", "Benin");
        public static NWDServerCountry BL = Add(30, "BL", "Saint Barthelemey");
        public static NWDServerCountry BM = Add(31, "BM", "Bermuda");
        public static NWDServerCountry BN = Add(32, "BN", "Brunei Darussalam");
        public static NWDServerCountry BO = Add(33, "BO", "Bolivia");
        public static NWDServerCountry BQ = Add(34, "BQ", "Bonaire, Saint Eustatius and Saba");
        public static NWDServerCountry BR = Add(35, "BR", "Brazil");
        public static NWDServerCountry BS = Add(36, "BS", "Bahamas");
        public static NWDServerCountry BT = Add(37, "BT", "Bhutan");
        public static NWDServerCountry BV = Add(38, "BV", "Bouvet Island");
        public static NWDServerCountry BW = Add(39, "BW", "Botswana");
        public static NWDServerCountry BY = Add(40, "BY", "Belarus");
        public static NWDServerCountry BZ = Add(41, "BZ", "Belize");
        public static NWDServerCountry CA = Add(42, "CA", "Canada");
        public static NWDServerCountry CC = Add(43, "CC", "Cocos (Keeling) Islands");
        public static NWDServerCountry CD = Add(44, "CD", "Congo, The Democratic Republic of the");
        public static NWDServerCountry CF = Add(45, "CF", "Central African Republic");
        public static NWDServerCountry CG = Add(46, "CG", "Congo");
        public static NWDServerCountry CH = Add(47, "CH", "Switzerland");
        public static NWDServerCountry CI = Add(48, "CI", "Cote d'Ivoire");
        public static NWDServerCountry CK = Add(49, "CK", "Cook Islands");
        public static NWDServerCountry CL = Add(50, "CL", "Chile");
        public static NWDServerCountry CM = Add(51, "CM", "Cameroon");
        public static NWDServerCountry CN = Add(52, "CN", "China");
        public static NWDServerCountry CO = Add(53, "CO", "Colombia");
        public static NWDServerCountry CR = Add(54, "CR", "Costa Rica");
        public static NWDServerCountry CU = Add(55, "CU", "Cuba");
        public static NWDServerCountry CV = Add(56, "CV", "Cape Verde");
        public static NWDServerCountry CW = Add(57, "CW", "Curacao");
        public static NWDServerCountry CX = Add(58, "CX", "Christmas Island");
        public static NWDServerCountry CY = Add(59, "CY", "Cyprus");
        public static NWDServerCountry CZ = Add(60, "CZ", "Czech Republic");
        public static NWDServerCountry DE = Add(61, "DE", "Germany");
        public static NWDServerCountry DJ = Add(62, "DJ", "Djibouti");
        public static NWDServerCountry DK = Add(63, "DK", "Denmark");
        public static NWDServerCountry DM = Add(64, "DM", "Dominica");
        public static NWDServerCountry DO = Add(65, "DO", "Dominican Republic");
        public static NWDServerCountry DZ = Add(66, "DZ", "Algeria");
        public static NWDServerCountry EC = Add(67, "EC", "Ecuador");
        public static NWDServerCountry EE = Add(68, "EE", "Estonia");
        public static NWDServerCountry EG = Add(69, "EG", "Egypt");
        public static NWDServerCountry EH = Add(70, "EH", "Western Sahara");
        public static NWDServerCountry ER = Add(71, "ER", "Eritrea");
        public static NWDServerCountry ES = Add(72, "ES", "Spain");
        public static NWDServerCountry ET = Add(73, "ET", "Ethiopia");
        public static NWDServerCountry EU = Add(74, "EU", "Europe");
        public static NWDServerCountry FI = Add(75, "FI", "Finland");
        public static NWDServerCountry FJ = Add(76, "FJ", "Fiji");
        public static NWDServerCountry FK = Add(77, "FK", "Falkland Islands (Malvinas)");
        public static NWDServerCountry FM = Add(78, "FM", "Micronesia, Federated States of");
        public static NWDServerCountry FO = Add(79, "FO", "Faroe Islands");
        public static NWDServerCountry FR = Add(80, "FR", "France");
        public static NWDServerCountry GA = Add(81, "GA", "Gabon");
        public static NWDServerCountry GB = Add(82, "GB", "United Kingdom");
        public static NWDServerCountry GD = Add(83, "GD", "Grenada");
        public static NWDServerCountry GE = Add(84, "GE", "Georgia");
        public static NWDServerCountry GF = Add(85, "GF", "French Guiana");
        public static NWDServerCountry GG = Add(86, "GG", "Guernsey");
        public static NWDServerCountry GH = Add(87, "GH", "Ghana");
        public static NWDServerCountry GI = Add(88, "GI", "Gibraltar");
        public static NWDServerCountry GL = Add(89, "GL", "Greenland");
        public static NWDServerCountry GM = Add(90, "GM", "Gambia");
        public static NWDServerCountry GN = Add(91, "GN", "Guinea");
        public static NWDServerCountry GP = Add(92, "GP", "Guadeloupe");
        public static NWDServerCountry GQ = Add(93, "GQ", "Equatorial Guinea");
        public static NWDServerCountry GR = Add(94, "GR", "Greece");
        public static NWDServerCountry GS = Add(95, "GS", "South Georgia and the South Sandwich Islands");
        public static NWDServerCountry GT = Add(96, "GT", "Guatemala");
        public static NWDServerCountry GU = Add(97, "GU", "Guam");
        public static NWDServerCountry GW = Add(98, "GW", "Guinea-Bissau");
        public static NWDServerCountry GY = Add(99, "GY", "Guyana");
        public static NWDServerCountry HK = Add(100, "HK", "Hong Kong");
        public static NWDServerCountry HM = Add(101, "HM", "Heard Island and McDonald Islands");
        public static NWDServerCountry HN = Add(102, "HN", "Honduras");
        public static NWDServerCountry HR = Add(103, "HR", "Croatia");
        public static NWDServerCountry HT = Add(104, "HT", "Haiti");
        public static NWDServerCountry HU = Add(105, "HU", "Hungary");
        public static NWDServerCountry ID = Add(106, "ID", "Indonesia");
        public static NWDServerCountry IE = Add(107, "IE", "Ireland");
        public static NWDServerCountry IL = Add(108, "IL", "Israel");
        public static NWDServerCountry IM = Add(109, "IM", "Isle of Man");
        public static NWDServerCountry IN = Add(110, "IN", "India");
        public static NWDServerCountry IO = Add(111, "IO", "British Indian Ocean Territory");
        public static NWDServerCountry IQ = Add(112, "IQ", "Iraq");
        public static NWDServerCountry IR = Add(113, "IR", "Iran, Islamic Republic of");
        public static NWDServerCountry IS = Add(114, "IS", "Iceland");
        public static NWDServerCountry IT = Add(115, "IT", "Italy");
        public static NWDServerCountry JE = Add(116, "JE", "Jersey");
        public static NWDServerCountry JM = Add(117, "JM", "Jamaica");
        public static NWDServerCountry JO = Add(118, "JO", "Jordan");
        public static NWDServerCountry JP = Add(119, "JP", "Japan");
        public static NWDServerCountry KE = Add(120, "KE", "Kenya");
        public static NWDServerCountry KG = Add(121, "KG", "Kyrgyzstan");
        public static NWDServerCountry KH = Add(122, "KH", "Cambodia");
        public static NWDServerCountry KI = Add(123, "KI", "Kiribati");
        public static NWDServerCountry KM = Add(124, "KM", "Comoros");
        public static NWDServerCountry KN = Add(125, "KN", "Saint Kitts and Nevis");
        public static NWDServerCountry KP = Add(126, "KP", "Korea, Democratic People's Republic of");
        public static NWDServerCountry KR = Add(127, "KR", "Korea, Republic of");
        public static NWDServerCountry KW = Add(128, "KW", "Kuwait");
        public static NWDServerCountry KY = Add(129, "KY", "Cayman Islands");
        public static NWDServerCountry KZ = Add(130, "KZ", "Kazakhstan");
        public static NWDServerCountry LA = Add(131, "LA", "Lao People's Democratic Republic");
        public static NWDServerCountry LB = Add(132, "LB", "Lebanon");
        public static NWDServerCountry LC = Add(133, "LC", "Saint Lucia");
        public static NWDServerCountry LI = Add(134, "LI", "Liechtenstein");
        public static NWDServerCountry LK = Add(135, "LK", "Sri Lanka");
        public static NWDServerCountry LR = Add(136, "LR", "Liberia");
        public static NWDServerCountry LS = Add(137, "LS", "Lesotho");
        public static NWDServerCountry LT = Add(138, "LT", "Lithuania");
        public static NWDServerCountry LU = Add(139, "LU", "Luxembourg");
        public static NWDServerCountry LV = Add(140, "LV", "Latvia");
        public static NWDServerCountry LY = Add(141, "LY", "Libyan Arab Jamahiriya");
        public static NWDServerCountry MA = Add(142, "MA", "Morocco");
        public static NWDServerCountry MC = Add(143, "MC", "Monaco");
        public static NWDServerCountry MD = Add(144, "MD", "Moldova, Republic of");
        public static NWDServerCountry ME = Add(145, "ME", "Montenegro");
        public static NWDServerCountry MF = Add(146, "MF", "Saint Martin");
        public static NWDServerCountry MG = Add(147, "MG", "Madagascar");
        public static NWDServerCountry MH = Add(148, "MH", "Marshall Islands");
        public static NWDServerCountry MK = Add(149, "MK", "Macedonia");
        public static NWDServerCountry ML = Add(150, "ML", "Mali");
        public static NWDServerCountry MM = Add(151, "MM", "Myanmar");
        public static NWDServerCountry MN = Add(152, "MN", "Mongolia");
        public static NWDServerCountry MO = Add(153, "MO", "Macao");
        public static NWDServerCountry MP = Add(154, "MP", "Northern Mariana Islands");
        public static NWDServerCountry MQ = Add(155, "MQ", "Martinique");
        public static NWDServerCountry MR = Add(156, "MR", "Mauritania");
        public static NWDServerCountry MS = Add(157, "MS", "Montserrat");
        public static NWDServerCountry MT = Add(158, "MT", "Malta");
        public static NWDServerCountry MU = Add(159, "MU", "Mauritius");
        public static NWDServerCountry MV = Add(160, "MV", "Maldives");
        public static NWDServerCountry MW = Add(161, "MW", "Malawi");
        public static NWDServerCountry MX = Add(162, "MX", "Mexico");
        public static NWDServerCountry MY = Add(163, "MY", "Malaysia");
        public static NWDServerCountry MZ = Add(164, "MZ", "Mozambique");
        public static NWDServerCountry NA = Add(165, "NA", "Namibia");
        public static NWDServerCountry NC = Add(166, "NC", "New Caledonia");
        public static NWDServerCountry NE = Add(167, "NE", "Niger");
        public static NWDServerCountry NF = Add(168, "NF", "Norfolk Island");
        public static NWDServerCountry NG = Add(169, "NG", "Nigeria");
        public static NWDServerCountry NI = Add(170, "NI", "Nicaragua");
        public static NWDServerCountry NL = Add(171, "NL", "Netherlands");
        public static NWDServerCountry NO = Add(172, "NO", "Norway");
        public static NWDServerCountry NP = Add(173, "NP", "Nepal");
        public static NWDServerCountry NR = Add(174, "NR", "Nauru");
        public static NWDServerCountry NU = Add(175, "NU", "Niue");
        public static NWDServerCountry NZ = Add(176, "NZ", "New Zealand");
        public static NWDServerCountry OM = Add(177, "OM", "Oman");
        public static NWDServerCountry PA = Add(178, "PA", "Panama");
        public static NWDServerCountry PE = Add(179, "PE", "Peru");
        public static NWDServerCountry PF = Add(180, "PF", "French Polynesia");
        public static NWDServerCountry PG = Add(181, "PG", "Papua New Guinea");
        public static NWDServerCountry PH = Add(182, "PH", "Philippines");
        public static NWDServerCountry PK = Add(183, "PK", "Pakistan");
        public static NWDServerCountry PL = Add(184, "PL", "Poland");
        public static NWDServerCountry PM = Add(185, "PM", "Saint Pierre and Miquelon");
        public static NWDServerCountry PN = Add(186, "PN", "Pitcairn");
        public static NWDServerCountry PR = Add(187, "PR", "Puerto Rico");
        public static NWDServerCountry PS = Add(188, "PS", "Palestinian Territory");
        public static NWDServerCountry PT = Add(189, "PT", "Portugal");
        public static NWDServerCountry PW = Add(190, "PW", "Palau");
        public static NWDServerCountry PY = Add(191, "PY", "Paraguay");
        public static NWDServerCountry QA = Add(192, "QA", "Qatar");
        public static NWDServerCountry RE = Add(193, "RE", "Reunion");
        public static NWDServerCountry RO = Add(194, "RO", "Romania");
        public static NWDServerCountry RS = Add(195, "RS", "Serbia");
        public static NWDServerCountry RU = Add(196, "RU", "Russian Federation");
        public static NWDServerCountry RW = Add(197, "RW", "Rwanda");
        public static NWDServerCountry SA = Add(198, "SA", "Saudi Arabia");
        public static NWDServerCountry SB = Add(199, "SB", "Solomon Islands");
        public static NWDServerCountry SC = Add(200, "SC", "Seychelles");
        public static NWDServerCountry SD = Add(201, "SD", "Sudan");
        public static NWDServerCountry SE = Add(202, "SE", "Sweden");
        public static NWDServerCountry SG = Add(203, "SG", "Singapore");
        public static NWDServerCountry SH = Add(204, "SH", "Saint Helena");
        public static NWDServerCountry SI = Add(205, "SI", "Slovenia");
        public static NWDServerCountry SJ = Add(206, "SJ", "Svalbard and Jan Mayen");
        public static NWDServerCountry SK = Add(207, "SK", "Slovakia");
        public static NWDServerCountry SL = Add(208, "SL", "Sierra Leone");
        public static NWDServerCountry SM = Add(209, "SM", "San Marino");
        public static NWDServerCountry SN = Add(210, "SN", "Senegal");
        public static NWDServerCountry SO = Add(211, "SO", "Somalia");
        public static NWDServerCountry SR = Add(212, "SR", "Suriname");
        public static NWDServerCountry SS = Add(213, "SS", "South Sudan");
        public static NWDServerCountry ST = Add(214, "ST", "Sao Tome and Principe");
        public static NWDServerCountry SV = Add(215, "SV", "El Salvador");
        public static NWDServerCountry SX = Add(216, "SX", "Sint Maarten");
        public static NWDServerCountry SY = Add(217, "SY", "Syrian Arab Republic");
        public static NWDServerCountry SZ = Add(218, "SZ", "Swaziland");
        public static NWDServerCountry TC = Add(219, "TC", "Turks and Caicos Islands");
        public static NWDServerCountry TD = Add(220, "TD", "Chad");
        public static NWDServerCountry TF = Add(221, "TF", "French Southern Territories");
        public static NWDServerCountry TG = Add(222, "TG", "Togo");
        public static NWDServerCountry TH = Add(223, "TH", "Thailand");
        public static NWDServerCountry TJ = Add(224, "TJ", "Tajikistan");
        public static NWDServerCountry TK = Add(225, "TK", "Tokelau");
        public static NWDServerCountry TL = Add(226, "TL", "Timor-Leste");
        public static NWDServerCountry TM = Add(227, "TM", "Turkmenistan");
        public static NWDServerCountry TN = Add(228, "TN", "Tunisia");
        public static NWDServerCountry TO = Add(229, "TO", "Tonga");
        public static NWDServerCountry TR = Add(230, "TR", "Turkey");
        public static NWDServerCountry TT = Add(231, "TT", "Trinidad and Tobago");
        public static NWDServerCountry TV = Add(232, "TV", "Tuvalu");
        public static NWDServerCountry TW = Add(233, "TW", "Taiwan");
        public static NWDServerCountry TZ = Add(234, "TZ", "Tanzania, United Republic of");
        public static NWDServerCountry UA = Add(235, "UA", "Ukraine");
        public static NWDServerCountry UG = Add(236, "UG", "Uganda");
        public static NWDServerCountry UM = Add(237, "UM", "United States Minor Outlying Islands");
        public static NWDServerCountry US = Add(238, "US", "United States");
        public static NWDServerCountry UY = Add(239, "UY", "Uruguay");
        public static NWDServerCountry UZ = Add(240, "UZ", "Uzbekistan");
        public static NWDServerCountry VA = Add(241, "VA", "Holy See (Vatican City State)");
        public static NWDServerCountry VC = Add(242, "VC", "Saint Vincent and the Grenadines");
        public static NWDServerCountry VE = Add(243, "VE", "Venezuela");
        public static NWDServerCountry VG = Add(244, "VG", "Virgin Islands, British");
        public static NWDServerCountry VI = Add(245, "VI", "Virgin Islands, U.S.");
        public static NWDServerCountry VN = Add(246, "VN", "Vietnam");
        public static NWDServerCountry VU = Add(247, "VU", "Vanuatu");
        public static NWDServerCountry WF = Add(248, "WF", "Wallis and Futuna");
        public static NWDServerCountry WS = Add(249, "WS", "Samoa");
        public static NWDServerCountry YE = Add(250, "YE", "Yemen");
        public static NWDServerCountry YT = Add(251, "YT", "Mayotte");
        public static NWDServerCountry ZA = Add(252, "ZA", "South Africa");
        public static NWDServerCountry ZM = Add(253, "ZM", "Zambia");
        public static NWDServerCountry ZW = Add(254, "ZW", "Zimbabwe");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDServerDistribution
    {
        debian9,
        debian10,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDInternalKeyNotEditable]
    [NWDClassTrigrammeAttribute("SSH")]
    [NWDClassDescriptionAttribute("Server descriptions Class")]
    [NWDClassMenuNameAttribute("Server")]
    public partial class NWDServer : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("DNS use to find IP")]
        [NWDTooltips("Optional DNS of this server (not the public DNS, just usable DNS)")]
        public string DomainNameServer { get; set; }
        public NWDServerContinent Continent { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Authentification SSH / SFTP")]
        [NWDEntitled("SSH IP")]
        public NWDIPType IP { get; set; }
        [NWDEntitled("SSH Port")]
        public bool PortChanged { get; set; }
        [NWDIf("PortChanged", false)]
        public int Port { get; set; }
        [NWDEntitled("SSH change to Port")]
        public int FuturPort { get; set; }
        [NWDInspectorGroupEnd()]

        [NWDInspectorGroupStart("Admmin")]
        public bool AdminInstalled { get; set; }
        [NWDIf("AdminInstalled", false)]
        [NWDEntitled("SSH Admin User")]
        public string Admin_User { get; set; }
        [NWDIf("AdminInstalled", false)]
        [NWDIf("AdminInstalled", false)]
        [NWDEntitled("SSH Admin Password (AES)")]
        public NWDSecurePassword Admin_Secure_Password { get; set; }
        [NWDInspectorGroupEnd()]

        [NWDInspectorGroupStart("Root")]
        public bool RootForbidden { get; set; }
        [NWDIf("RootForbidden", false)]
        [NWDHidden]
        public bool SudoI { get; set; } // for kimsufi : ssh -l debian xxx.xxx.xxx.xxx sudo -i & echo root:pa1452sesd | chpasswd
        [NWDIf("RootForbidden", false)]
        [NWDEntitled("SSH Root User")]
        public string Root_User { get; set; }
        [NWDIf("RootForbidden", false)]
        [NWDEntitled("SSH Root Password (AES)")]
        public NWDSecurePassword Root_Secure_Password { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Install Server Options")]
        public NWDServerDistribution Distribution { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================