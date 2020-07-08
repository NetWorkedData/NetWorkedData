//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#if UNITY_EDITOR
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEPassAnalyseComputer
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Name;
        public long GigaCyclePerSeconds;
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWEPassAnalyseComputer> kList = new List<NWEPassAnalyseComputer>();
        static public List<string> kListName = new List<string>();
        static public Dictionary<string, NWEPassAnalyseComputer> kDico = new Dictionary<string, NWEPassAnalyseComputer>();
        //-------------------------------------------------------------------------------------------------------------
        public NWEPassAnalyseComputer(string sName, long sGigaCyclePerSeconds)
        {
            this.Initialize(sName, sGigaCyclePerSeconds);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Initialize(string sName, long sGigaCyclePerSeconds)
        {
            Name = sName;
            GigaCyclePerSeconds = sGigaCyclePerSeconds;
            if (kDico.ContainsKey(sName) == false)
            {
                kList.Add(this);
                kListName.Add(sName);
                kDico.Add(sName, this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWEPassAnalyseComputer OneGHertz = new NWEPassAnalyseComputer("OneGHertz", 1);
        static public NWEPassAnalyseComputer TwoGHertz = new NWEPassAnalyseComputer("TwoGHertz", 2);
        static public NWEPassAnalyseComputer ThreeGHertz = new NWEPassAnalyseComputer("ThreeGHertz", 3);
        static public NWEPassAnalyseComputer TenClusterGHertz = new NWEPassAnalyseComputer("TenClusterGHertz", 30);
        static public NWEPassAnalyseComputer HundredClusterGHertz = new NWEPassAnalyseComputer("HundredClusterGHertz", 300);
        static public NWEPassAnalyseComputer ViralClusterGHertz = new NWEPassAnalyseComputer("ViralClusterGHertz", 3000000000);
        static public NWEPassAnalyseComputer MassiveClusterGHertz = new NWEPassAnalyseComputer("MassiveClusterGHertz", 7000000000000);
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEPassAnalyseWindow : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWEPassAnalyseWindow kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static void SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().Show();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        string Password = string.Empty;
        NWEPassAnalyseComputer AttackType = NWEPassAnalyseComputer.OneGHertz;
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem("Tools/Pass Analyze Window")]
        public static NWEPassAnalyseWindow SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWEPassAnalyseWindow)) as NWEPassAnalyseWindow;
                kSharedInstance.Show();
                kSharedInstance.Focus();
            }
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            titleContent = new GUIContent("Password analyzer");
            Password = NWDToolbox.RandomString(16);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AnalyzePassword(string sPassword)
        {
            Password = sPassword;
            Repaint();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {

            GUILayout.Label("Password to analyze", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Enter a password to analyze combinations and crack times", MessageType.Info);
            EditorGUI.indentLevel++;
            Password = EditorGUILayout.TextField("Password", Password);
            EditorGUI.indentLevel--;

            GUILayout.Label("Analyze", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Lenght", Password.Length.ToString());
            NWEPassCharset tPassCharset = NWEPassCharset.FoundCharset(Password);
            EditorGUILayout.LabelField("Charset", tPassCharset.Name);
            EditorGUILayout.LabelField("Charset Ereg", tPassCharset.Ereg(Password));
            EditorGUILayout.LabelField("Charset Lenght", tPassCharset.Lenght.ToString());
            EditorGUILayout.LabelField("Charset Entropie", tPassCharset.EntropieBase.ToString());
            EditorGUILayout.LabelField("Entropie", NWEPassCharset.Entropie(Password, tPassCharset).ToString());
            double tCombinaison = NWEPassCharset.Combinaison(Password, tPassCharset);
            EditorGUILayout.LabelField("Combinaisons", tCombinaison.ToString());
            EditorGUI.indentLevel--;

            GUILayout.Label("Time to crack", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Cycle per seconds", NWEPassCharset.CycleTest(Password, tPassCharset).ToString());

            int tAttackType = NWEPassAnalyseComputer.kList.IndexOf(AttackType);
            tAttackType = EditorGUILayout.Popup("Attack Type", tAttackType, NWEPassAnalyseComputer.kListName.ToArray());
            AttackType = NWEPassAnalyseComputer.kList[tAttackType];
            float tCycle = 1000000000.0F * AttackType.GigaCyclePerSeconds;
            EditorGUILayout.LabelField("Cycle per seconds", tCycle.ToString());
            EditorGUILayout.LabelField("Force attack", NWEPassCharset.ForceAttack(Password, tPassCharset, tCycle));
            EditorGUI.indentLevel--;

            GUILayout.Label("Analyze SHA one of password", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            string tSha = NWESecurityTools.GenerateSha(Password);
            EditorGUILayout.LabelField("SHA", tSha);
            EditorGUILayout.LabelField("SHA Lenght", tSha.Length.ToString());
            NWEPassCharset tShaCharset = NWEPassCharset.FoundCharset(tSha);
            EditorGUILayout.LabelField("SHA Charset", tShaCharset.Name);
            EditorGUILayout.LabelField("Force attack SHA", NWEPassCharset.ForceAttack(tSha, tShaCharset, tCycle));
            EditorGUI.indentLevel--;

            GUILayout.Label("Analyze SHA 512 of password", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            string tSha512 = NWESecurityTools.GenerateSha(Password, NWESecurityShaTypeEnum.Sha512);
            EditorGUILayout.LabelField("SHA512 ", tSha512);
            EditorGUILayout.LabelField("SHA512 Lenght", tSha512.Length.ToString());
            NWEPassCharset tShaCharset512 = NWEPassCharset.FoundCharset(tSha512);
            EditorGUILayout.LabelField("SHA512 Charset", tShaCharset512.Name);
            EditorGUILayout.LabelField("Force attack SHA512", NWEPassCharset.ForceAttack(tSha512, tShaCharset512, tCycle));
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            /*
            Rect tLastRect = GUILayoutUtility.GetLastRect();
            minSize = new Vector2(400, tLastRect.y + tLastRect.height);
            maxSize = new Vector2(1024, tLastRect.y + tLastRect.height);
            */
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#endif
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEPassCharset
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Name;
        public string Charset;
        public int Lenght;
        public double EntropieBase;
        public bool CaseSensible = false;
        public bool CaseUpper = false;
        public bool CaseLower = false;
        //-------------------------------------------------------------------------------------------------------------
        static List<NWEPassCharset> kList = new List<NWEPassCharset>();
        //-------------------------------------------------------------------------------------------------------------
        static public string ForceAttack(string sPassword, double sAttacksPerSecond)
        {
            NWEPassCharset tCharSetMaxUsed = FoundCharset(sPassword);
            return ForceAttack(sPassword, tCharSetMaxUsed, sAttacksPerSecond);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public double CycleTest(string sPassword, NWEPassCharset sCharSet)
        {
            double rReturn = 1.0F;
            int tI = 0;
            DateTime tA = DateTime.Now;
            if (sCharSet != NWEPassCharset.None)
            {
                sCharSet = NWEPassCharset.Binaire;
            }
            string tCharset = sCharSet.Charset;
            if (string.IsNullOrEmpty(tCharset))
            {
                tCharset = "01";
            }
            float tTestNumber = 100.0F;
            for (int tJ = 0; tJ < tTestNumber; tJ++)
            {
                StringBuilder tTest = new StringBuilder();
                while (tTest.Length < sPassword.Length)
                {
                    tTest.Append(tCharset.Substring(tI, 1));
                    tI++;
                    if (tI >= tCharset.Length)
                    {
                        tI = 0;
                    }
                }
            }
            DateTime tB = DateTime.Now;
            double tStart = NWEDateHelper.ConvertToTimestamp(tA);
            double tFinish = NWEDateHelper.ConvertToTimestamp(tB);
            rReturn = Math.Floor(tTestNumber / (tFinish - tStart));
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public string ForceAttack(string sPassword, NWEPassCharset sCharSet, double sAttacksPerSecond)
        {
            string tCylceTotal = "";
            string tFormat = "##.##";
            string tFormatMax = "0.00 E0";
            double tEstimation = Combinaison(sPassword, sCharSet) / sAttacksPerSecond;
            if (tEstimation < 1.0F)
            {
                tCylceTotal = "Immediatly";
            }
            else
            {
                // seconds
                if (tEstimation < 60)
                {
                    tCylceTotal = tEstimation.ToString(tFormat) + " seconds";
                }
                else
                {
                    // minutes
                    tEstimation = tEstimation / 60; //=> minutes
                    if (tEstimation < 60)
                    {
                        tCylceTotal = tEstimation.ToString(tFormat) + " minutes";
                    }
                    else
                    {
                        // hours
                        tEstimation = tEstimation / 60; //=> hours
                        if (tEstimation < 24)
                        {
                            tCylceTotal = tEstimation.ToString(tFormat) + " hours";
                        }
                        else
                        {
                            // days
                            tEstimation = tEstimation / 24; //=> days
                            if (tEstimation < 365)
                            {
                                tCylceTotal = tEstimation.ToString(tFormat) + " days";
                            }
                            else
                            {
                                // years
                                tEstimation = tEstimation / 365; // => years
                                if (tEstimation < 100)
                                {
                                    tCylceTotal = tEstimation.ToString(tFormat) + " years";
                                }
                                else
                                {
                                    // century
                                    tEstimation = tEstimation / 100; // => century
                                    if (tEstimation < 10)
                                    {
                                        tCylceTotal = tEstimation.ToString(tFormat) + " century";
                                    }
                                    else
                                    {
                                        tEstimation = tEstimation / 10;
                                        if (tEstimation < 1000)
                                        {
                                            tCylceTotal = tEstimation.ToString(tFormat) + " millenary";
                                        }
                                        else
                                        {
                                            tCylceTotal = tEstimation.ToString(tFormatMax) + " millenary";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return tCylceTotal;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public double Combinaison(string sPassword)
        {
            NWEPassCharset tCharSetMaxUsed = FoundCharset(sPassword);
            return Combinaison(sPassword, tCharSetMaxUsed);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public double Combinaison(string sPassword, NWEPassCharset sCharSet)
        {
            double rRetrun = 0.0F;
            if (sCharSet != null)
            {
                rRetrun = Math.Pow(sCharSet.Lenght, sPassword.Length);
            }
            return rRetrun;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public double Entropie(string sPassword)
        {
            NWEPassCharset tCharSetMaxUsed = FoundCharset(sPassword);
            return Entropie(sPassword, tCharSetMaxUsed);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public double Entropie(string sPassword, NWEPassCharset sCharSet)
        {
            double rRetrun = 0.0F;
            if (sCharSet != null)
            {
                rRetrun = sCharSet.EntropieBase * sPassword.Length;
            }
            return rRetrun;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Ereg(string sPassword)
        {
            string tChar = Regex.Escape(Charset);
            tChar = tChar.Replace("\\ ", " ");
            tChar = tChar.Replace("-", "\\-");
            tChar = tChar.Replace("]", "\\]");
            tChar = tChar.Replace("}", "\\}");
            string rEreg = "[" + tChar + "]{" + sPassword.Length + "}";
            return rEreg;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWEPassCharset FoundCharset(string sPassword)
        {
            NWEPassCharset rReturn = NWEPassCharset.None;
            if (sPassword.Length > 0)
            {
                foreach (NWEPassCharset tCharset in kList)
                {
                    if (tCharset.Charset.Length > 0)
                    {
                        string tEreg = tCharset.Ereg(sPassword);
#if UNITY_EDITOR
                        //  Debug.Log(" test  " + sPassword + " with " + tEreg + " ... ");
#endif
                        Regex tRegex = new Regex(tEreg);
                        Match tMatch = tRegex.Match(sPassword);
                        if (tMatch.Success)
                        {
#if UNITY_EDITOR
                            // Debug.Log(" test  " + sPassword + " with " + tEreg + " SUCESS  ");
#endif

                            if (rReturn == NWEPassCharset.None)
                            {
                                rReturn = tCharset;
                            }
                            else
                            {
                                if (rReturn.Lenght > tCharset.Lenght)
                                {
                                    rReturn = tCharset;
                                }
                            }
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWEPassCharset(string sName, NWEPassCharset sFrom, NWEPassCharset sAdd)
        {
            Initialize(sName, sFrom.Charset + sAdd.Charset);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWEPassCharset(string sName, NWEPassCharset sFrom, string sCharsetAdd)
        {
            Initialize(sName, sFrom.Charset + sCharsetAdd);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWEPassCharset(string sName, string sCharset)
        {
            Initialize(sName, sCharset);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Initialize(string sName, string sCharset)
        {
            Name = sName;
            List<string> tCharset = new List<string>();
            for (int tI = 0; tI < sCharset.Length; tI++)
            {
                string tC = sCharset.Substring(tI, 1);
                if (tCharset.Contains(tC) == false)
                {
                    tCharset.Add(tC);
                }
                if (tC.ToUpper() == tC)
                {
                    // Upper case 
                    CaseUpper = true;
                    if (tCharset.Contains(tC.ToLower()) == true)
                    {
                        CaseSensible = true;
                    }
                }
                if (tC.ToLower() == tC)
                {
                    // Lower case 
                    CaseLower = true;
                    if (tCharset.Contains(tC.ToUpper()) == true)
                    {
                        CaseSensible = true;
                    }
                }
            }
            Charset = string.Join(string.Empty, tCharset);
            Lenght = Charset.Length;
            EntropieBase = Math.Log(Lenght, 2);
            kList.Add(this);
#if UNITY_EDITOR
            //  Debug.Log(" add NWEPassCharset named " + Name + " (" + Charset + ")");
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWEPassCharset None = new NWEPassCharset("None", "");
        static public NWEPassCharset Binaire = new NWEPassCharset("Binaire", "01");
        static public NWEPassCharset Numeric = new NWEPassCharset("Numerical", "0123456789");

        static public NWEPassCharset Hexadecimal = new NWEPassCharset("Hexadecimal", Numeric, "ABCDEFG");

        static public NWEPassCharset AlphabeticUpper = new NWEPassCharset("Alphabetic Upper Case", "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        static public NWEPassCharset AlphabeticLower = new NWEPassCharset("Alphabetic Lower Case", "abcdefghijklmnopqrstuvwxyz");
        static public NWEPassCharset Alphabetic = new NWEPassCharset("Alphabetic", AlphabeticUpper, AlphabeticLower);

        static public NWEPassCharset AlphanumericUpper = new NWEPassCharset("Alphanumeric Upper Case", AlphabeticUpper, Numeric);
        static public NWEPassCharset AlphanumericLower = new NWEPassCharset("Alphanumeric Lower Case", AlphabeticLower, Numeric);
        static public NWEPassCharset Alphanumeric = new NWEPassCharset("Alphanumeric", Alphabetic, Numeric);

        static public NWEPassCharset AlphanumericUnix = new NWEPassCharset("Alphanumeric unix", Alphanumeric, "_-");
        static public NWEPassCharset AlphanumericEmail = new NWEPassCharset("Alphanumeric Email", AlphabeticLower, "-_.@");

        static public NWEPassCharset AlphanumericSpace = new NWEPassCharset("Alphanumeric Space", AlphanumericUnix, " ");
        static public NWEPassCharset AlphanumericPonctued = new NWEPassCharset("Alphanumeric Ponctued", AlphanumericUnix, " ()[]{}%,?;.:!&");
        static public NWEPassCharset AlphanumericCompleted = new NWEPassCharset("Alphanumeric Completed", AlphanumericPonctued, "@$£¥^");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWEPasswordTesterResult : int
    {
        Pitiful = 10,
        VeryWeak = 30,
        Weak = 40,
        Basic = 50,
        Strong = 70,
        VeryStrong = 90,
        Exceptional = 100,

        None = -1,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEPasswordTester
    {
        //-------------------------------------------------------------------------------------------------------------
        string Password;
        NWEPasswordTesterResult Result = NWEPasswordTesterResult.None;
        public int Score = -1;
        //-------------------------------------------------------------------------------------------------------------
        public int Lenght;
        public int NumberCount;
        public int LowerCharCount;
        public int UpperCharCount;
        public int SpecialCharCount;
        public int NumberSequenceCount;
        public int CharSequenceCount;
        public int UpperCharSequenceCount;
        public int LowerCharSequenceCount;
        //-------------------------------------------------------------------------------------------------------------
        public NWEPasswordTester(string sPassword)
        {
            Password = sPassword;
            Analyze();
        }
        //-------------------------------------------------------------------------------------------------------------
        //static int LenghtMin = 8;
        static int NumberMin = 2;
        static int LowerCharMin = 2;
        static int UpperCharMin = 2;
        static int SpecialCharMin = 2;
        static int NumberSequenceMax = 3;
        static int CharSequenceMax = 3;
        static int UpperCharSequenceMax = 3;
        static int LowerCharSequenceMax = 3;
        //-------------------------------------------------------------------------------------------------------------
        public NWEPasswordTesterResult Analyze()
        {
            int tScore = 0;
            // analyze number
            Regex tRegexNumber = new Regex("[0-9]{1}");
            Match tMatchNumber = tRegexNumber.Match(Password);
            if (tMatchNumber.Success)
            {
                NumberCount = tMatchNumber.Captures.Count;
                if (tMatchNumber.Captures.Count >= NumberMin)
                {
                    tScore++;
                }
            }
            else
            {
                tScore--;
            }
            // analyze LowerChar
            Regex tRegexLower = new Regex("[a-z]{1}");
            Match tMatchLower = tRegexLower.Match(Password);
            if (tMatchLower.Success)
            {
                LowerCharCount = tMatchLower.Captures.Count;
                if (tMatchLower.Captures.Count >= LowerCharMin)
                {
                    tScore++;
                }
            }
            else
            {
                tScore--;
            }
            // analyze UpperChar
            Regex tRegexUpper = new Regex("[A-Z]{1}");
            Match tMatchUpper = tRegexUpper.Match(Password);
            if (tMatchUpper.Success)
            {
                UpperCharCount = tMatchUpper.Captures.Count;
                if (tMatchUpper.Captures.Count >= UpperCharMin)
                {
                    tScore++;
                }
            }
            else
            {
                tScore--;
            }
            // analyze SpecialChar
            Regex tRegexSpecial = new Regex("[^0-9a-zA-Z]{1}");
            Match tMatchSpecial = tRegexSpecial.Match(Password);
            if (tMatchSpecial.Success)
            {
                SpecialCharCount = tMatchSpecial.Captures.Count;
                if (tMatchSpecial.Captures.Count >= SpecialCharMin)
                {
                    tScore++;
                }
            }
            else
            {
                tScore--;
            }
            // analyze char sequence
            Regex tRegexCharSequence = new Regex("[a-zA-Z]{" + CharSequenceMax + "+}");
            Match tMatchCharSequence = tRegexCharSequence.Match(Password);
            if (tMatchCharSequence.Success)
            {
                CharSequenceCount = tMatchCharSequence.Captures.Count;
                tScore--;
            }
            else
            {
                tScore++;
            }
            // analyze upper char sequence
            Regex tRegexUpperCharSequence = new Regex("[A-Z]{" + UpperCharSequenceMax + "+}");
            Match tMatchUpperCharSequence = tRegexUpperCharSequence.Match(Password);
            if (tMatchUpperCharSequence.Success)
            {
                UpperCharSequenceCount = tMatchUpperCharSequence.Captures.Count;
                tScore--;
            }
            else
            {
                tScore++;
            }
            // analyze lower char sequence
            Regex tRegexLowerCharSequence = new Regex("[a-z]{" + LowerCharSequenceMax + "+}");
            Match tMatchLowerCharSequence = tRegexLowerCharSequence.Match(Password);
            if (tMatchLowerCharSequence.Success)
            {
                LowerCharSequenceCount = tMatchLowerCharSequence.Captures.Count;
                tScore--;
            }
            else
            {
                tScore++;
            }
            // analyze number sequence
            Regex tRegexNumberSequence = new Regex("[0-9]{" + NumberSequenceMax + "+}");
            Match tMatchNumberSequence = tRegexNumberSequence.Match(Password);
            if (tMatchNumberSequence.Success)
            {
                NumberSequenceCount = tMatchNumberSequence.Captures.Count;
                tScore--;
            }
            else
            {
                tScore++;
            }
            // result * by length
            Score += tScore * Password.Length;
            // transfort in enum result
            if (Score <= (int)NWEPasswordTesterResult.Pitiful)
            {
                Result = NWEPasswordTesterResult.Pitiful;
            }
            if (Score >= (int)NWEPasswordTesterResult.VeryWeak)
            {
                Result = NWEPasswordTesterResult.VeryWeak;
            }
            if (Score >= (int)NWEPasswordTesterResult.Weak)
            {
                Result = NWEPasswordTesterResult.Weak;
            }
            if (Score >= (int)NWEPasswordTesterResult.Basic)
            {
                Result = NWEPasswordTesterResult.Basic;
            }
            if (Score >= (int)NWEPasswordTesterResult.Strong)
            {
                Result = NWEPasswordTesterResult.Strong;
            }
            if (Score >= (int)NWEPasswordTesterResult.VeryStrong)
            {
                Result = NWEPasswordTesterResult.VeryStrong;
            }
            if (Score >= (int)NWEPasswordTesterResult.Exceptional)
            {
                Result = NWEPasswordTesterResult.Exceptional;
            }
            return Result;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================