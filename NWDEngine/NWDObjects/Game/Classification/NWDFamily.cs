//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDFamilyConnection : NWDConnection<NWDFamily>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("FAM")]
    [NWDClassDescriptionAttribute("This class is used to reccord the family available in the game")]
    [NWDClassMenuNameAttribute("Family")]
    public partial class NWDFamily : NWDBasis<NWDFamily>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Informations", true, true, true)]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDTooltips("The description item. Usable to be ownershipped")]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupStartAttribute("Properties associated", true, true, true)]
        public NWDReferencesListType<NWDParameter> ParameterList
        {
            get; set;
        }
















        // !!!!!!!!!!!!!
        [NWDGroupEndAttribute]
        [NWDGroupStartAttribute("Old - REMOVE", true, true, true)]
        [NWDNotEditable]
        [Obsolete]
        public NWDReferencesListType<NWDWorld> WorldList
        {
            get; set;
        }
        [NWDNotEditable]
        [Obsolete]
        public NWDReferencesListType<NWDCategory> CategoryList
        {
            get; set;
        }
        [NWDNotEditable]
        [Obsolete]
        public NWDReferencesListType<NWDFamily> FamilyList
        {
            get; set;
        }
        [NWDNotEditable]
        [Obsolete]
        public NWDReferencesListType<NWDKeyword> KeywordList
        {
            get; set;
        }

        // !!!!!!!!!!!!!





        //-------------------------------------------------------------------------------------------------------------
        public NWDFamily()
        {
            //Debug.Log("NWDFamily Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDFamily(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDFamily Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
