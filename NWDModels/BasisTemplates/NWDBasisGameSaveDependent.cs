//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    public partial class NWDBasisGameSaveDependent : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Data is editable by this account. This account can read, write (insert, update) and trash this data.
        /// </summary>
        [NWDInspectorGroupReset()]
        [NWDInspectorGroupStart("GameSave Informations")]
        public NWDReferenceType<NWDGameSave> GameSave { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor basic.
        /// </summary>
        public NWDBasisGameSaveDependent()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor from database.
        /// </summary>
        public NWDBasisGameSaveDependent(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            base.AddonInsertMe();
            AssignGameSave();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            base.AddonUpdateMe();
            AssignGameSave();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            base.AddonDuplicateMe();
            AssignGameSave();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Assign the good <see cref="RangeAccess"/> for this data for this <see cref="Account"/>
        /// </summary>
        private void AssignGameSave()
        {
            //only if data was not sync ... else it need to use the define GameSave
            if (DevSync <= 1 && ProdSync <= 1 && PreprodSync <= 1)
            {
                GameSave.SetData(NWDGameSave.CurrentData());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================