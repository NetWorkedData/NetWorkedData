//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNodeEditorContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string K_NODE_EDITOR_LAST_TYPE_KEY = "K_NODE_EDITOR_LAST_TYPE_KEY_5fdshjktr";
        public const string K_NODE_EDITOR_LAST_REFERENCE_KEY = "K_NODE_EDITOR_LAST_REFERENCE_KEY_ed5f5dtr";
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        bool DragDetect = false;
        //-------------------------------------------------------------------------------------------------------------
        public Vector2 ScrollPosition = Vector2.zero;
        public Vector2 ScrollPositionMarge = Vector2.zero;
        Vector2 LastMousePosition = new Vector2(-1.0F, -1.0F);
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The document.
        /// </summary>
        public NWDNodeDocument Document = new NWDNodeDocument();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDNodeEditorContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDNodeEditorContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDNodeEditorContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SaveObjectInEdition(NWDTypeClass sSelection)
        {
            //NWDBenchmark.Start();
            if (sSelection == null)
            {
                NWDProjectPrefs.SetString(K_NODE_EDITOR_LAST_TYPE_KEY, string.Empty);
                NWDProjectPrefs.SetString(K_NODE_EDITOR_LAST_REFERENCE_KEY, string.Empty);
            }
            else
            {
                NWDProjectPrefs.SetString(K_NODE_EDITOR_LAST_TYPE_KEY, NWDBasisHelper.FindTypeInfos(sSelection.GetType()).ClassNamePHP);
                NWDProjectPrefs.SetString(K_NODE_EDITOR_LAST_REFERENCE_KEY, sSelection.Reference);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set selection.
        /// </summary>
        /// <param name="sSelection">S selection.</param>
        public void SetSelection(NWDTypeClass sSelection)
        {
            //NWDBenchmark.Start();
            if (NWDBasisHelper.FindTypeInfos(sSelection.GetType()).AllDatabaseIsLoaded())
            {
                Document.SetData(sSelection);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetLanguage()
        {
            return Document.Language;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            base.OnPreventGUI(sRect);
            NWDBenchmark.Start();
            float tX = Document.DocumentMarge + 15;
            if (Document.FixeMargePreference == false)
            {
                tX = 0;
            }
            else
            {
                Rect tScrollViewRectB = new Rect(0, 0, tX, sRect.height);
                ScrollPositionMarge = GUI.BeginScrollView(tScrollViewRectB, ScrollPositionMarge, Document.DimensionB());
                Document.DrawPreferences();
                GUI.EndScrollView();
            }
            Rect tScrollViewRect = new Rect(tX, 0, sRect.width - tX, sRect.height);
            //EditorGUI.DrawRect(tScrollViewRect, new Color (0.5F,0.5F,0.5F,1.0F));
            ScrollPosition = GUI.BeginScrollView(tScrollViewRect, ScrollPosition, Document.Dimension());
            Rect tVisibleRect = new Rect(ScrollPosition.x, ScrollPosition.y, sRect.width + ScrollPosition.x, sRect.height + ScrollPosition.y);
            
            Document.Draw(tScrollViewRect, tVisibleRect);
            GUI.EndScrollView();
            // Check if the mouse is above our scrollview.
            if (tScrollViewRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    Vector2 currPos = Event.current.mousePosition;
                    LastMousePosition = currPos;
                    DragDetect = true;
                    Event.current.Use();
                }
            }
            if (DragDetect == true)
            {
                // Only move if we are hold down mouse button, and the mouse is moving.
                if (Event.current.type == EventType.MouseDrag)
                {
                    // Current position
                    Vector2 currPos = Event.current.mousePosition;

                    // Only move if the distance between the last mouse position and the current is less than 50.
                    // Without this it jumps during the drag.
                    if (Vector2.Distance(currPos, LastMousePosition) < 5000)
                    {
                        // Calculate the delta x and y.
                        float x = LastMousePosition.x - currPos.x;
                        float y = LastMousePosition.y - currPos.y;
                        // Add the delta moves to the scroll position.
                        ScrollPosition.x += x;
                        ScrollPosition.y += y;
                        Event.current.Use();
                    }
                    // Set the last mouse position to the current mouse position.
                    LastMousePosition = currPos;
                }
                if (Event.current.type == EventType.MouseUp)
                {
                    DragDetect = false;
                }
            }
            NWDBenchmark.Finish();
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD Node Editor. This editor can edit data as nodal card.
    /// </summary>
    public class NWDNodeEditor : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "nodal-view/";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The node editor shared instance.
        /// </summary>
        public static NWDNodeEditor _kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Shared instance.
        /// </summary>
        public static NWDNodeEditor SharedInstance()
        {
            //NWDBenchmark.Start();
            if (_kSharedInstance == null)
            {
                _kSharedInstance = EditorWindow.GetWindow(typeof(NWDNodeEditor)) as NWDNodeEditor;
                _kSharedInstance.Show();
               RestaureObjectInEdition();
                ReAnalyzeAll();
                _kSharedInstance.Focus();
            }
            //NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of Editor Configuration Manager Window and focus on.
        /// </summary>
        /// <returns></returns>
        public static void SharedInstanceFocus()
        {
            //NWDBenchmark.Start();
            //SharedInstance().ShowUtility();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Redraw.
        /// </summary>
        public static void Refresh()
        {
            //NWDBenchmark.Start();
            //if (kNodeEditorSharedInstance != null)
            //{
            //    kNodeEditorSharedInstance.Repaint();
            //}
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDNodeEditor));
            foreach (NWDNodeEditor tWindow in tWindows)
            {
                tWindow.Repaint();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On destroy.
        /// </summary>
        void OnDestroy()
        {
            //Debug.Log("Destroyed...");
            _kSharedInstance = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetWorkedData.NWDNodeEditor"/> class.
        /// </summary>
        public NWDNodeEditor()
        {
            this.autoRepaintOnSceneChange = false;
            this.wantsMouseEnterLeaveWindow = false;
            this.wantsMouseMove = false;
            //Document.SetData(null, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the enable event.
        /// </summary>
        public void OnEnable()
        {
            //NWDBenchmark.Start();
            // set ideal size
            NormalizeWidth = 2000;
            NormalizeHeight = 900;
            // set title
            TitleInit(NWDConstants.K_EDITOR_NODE_WINDOW_TITLE, typeof(NWDNodeEditor));
            NWDNodeEditorContent.SharedInstance().Document.EditorWindow = this;
            NWDNodeEditorContent.SharedInstance().Document.LoadClasses();
            Repaint();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set selection.
        /// </summary>
        /// <param name="sSelection">S selection.</param>
        public void SetSelection(NWDTypeClass sSelection)
        {
            //NWDBenchmark.Start();
            //NWDBenchmark.Trace();
            //Repaint();
            NWDNodeEditorContent.SharedInstance().SetSelection(sSelection);
            //Repaint();
            //Refresh();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the OnGUI event. Create the interface to enter a new class.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDNodeEditorContent.SharedInstance().Document.EditorWindow = this;
            NWDNodeEditorContent.SharedInstance().OnPreventGUI(position);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Updates the node window but prevent.
        /// </summary>
        /// <param name="sSelection">S selection.</param>
        public static void UpdateNodeWindow(NWDTypeClass sSelection)
        {
            //NWDBenchmark.Start();
            if (_kSharedInstance != null)
            {
                NWDNodeEditorContent.SharedInstance().Document.EditorWindow = _kSharedInstance;
                NWDNodeEditorContent.SharedInstance().Document.ReAnalyze();
                _kSharedInstance.Repaint();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RestaureObjectInEdition()
        {
            //NWDBenchmark.Start();
            string tTypeEdited = NWDProjectPrefs.GetString(NWDNodeEditorContent.K_NODE_EDITOR_LAST_TYPE_KEY);
            string tLastReferenceEdited = NWDProjectPrefs.GetString(NWDNodeEditorContent.K_NODE_EDITOR_LAST_REFERENCE_KEY);

            if (!string.IsNullOrEmpty(tTypeEdited) && !string.IsNullOrEmpty(tLastReferenceEdited))
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tTypeEdited);
                if (tHelper != null)
                {
                    NWDTypeClass tData = tHelper.GetDataByReference(tLastReferenceEdited);
                    SetObjectInNodeWindow(tData);
                }
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the object in node window.
        /// </summary>
        /// <param name="sSelection">S selection.</param>
        public static void SetObjectInNodeWindow(NWDTypeClass sSelection)
        {
            //NWDBenchmark.Start();
            if (sSelection != null)
            {
                if (NWDBasisHelper.FindTypeInfos(sSelection.GetType()).AllDatabaseIsLoaded())
                {
                    SharedInstanceFocus();
                    SharedInstance().SetSelection(sSelection);
                    NWDNodeEditorContent.SaveObjectInEdition(sSelection);
                    SharedInstance().Repaint();
                    NWDNodeEditorContent.SharedInstance().Document.ReEvaluateLayout();
                    SharedInstance().Repaint();
                }
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Redraw.
        /// </summary>
        public static void ReAnalyzeIfNecessary(object sObjectModified)
        {
            //NWDBenchmark.Start();
            if (_kSharedInstance != null)
            {
                NWDNodeEditorContent.SharedInstance().Document.EditorWindow = _kSharedInstance;
                NWDNodeEditorContent.SharedInstance().Document.ReAnalyzeIfNecessary(sObjectModified);
                _kSharedInstance.Repaint();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReAnalyzeAll()
        {
            //NWDBenchmark.Start();
            if (_kSharedInstance != null)
            {
                NWDNodeEditorContent.SharedInstance().Document.EditorWindow = _kSharedInstance;
                NWDNodeEditorContent.SharedInstance().Document.ReAnalyze();
                _kSharedInstance.Repaint();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
