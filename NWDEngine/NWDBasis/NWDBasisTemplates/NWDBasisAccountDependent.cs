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
    public partial class NWDBasisAccountDependent : NWDBasisBundled
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Data is editable by this account. This account can read, write (insert, update) and trash this data.
        /// </summary>
        [NWDInspectorGroupReset()]
        [NWDInspectorGroupStart("Account Informations")]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        //[NWDNotEditable]
        //[NWDCertified]
        //[NWDHidden]
        //public int RangeAccess
        //{
        //    get; set;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor basic.
        /// </summary>
        public NWDBasisAccountDependent()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor from database.
        /// </summary>
        public NWDBasisAccountDependent(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            base.AddonInsertMe();
            AssignRange();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            base.AddonUpdateMe();
            AssignRange();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            base.AddonDuplicateMe();
            AssignRange();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Assign the good <see cref="RangeAccess"/> for this data for this <see cref="Account"/>
        /// </summary>
        private void AssignRange()
        {
            //only if data was not sync ... else it need to use the define RangeAccess
            if (DevSync <= 1 && ProdSync <= 1 && PreprodSync <= 1)
            {
                int tRange = 0;
                if (Account != null)
                {
                    string[] tAccountExplode = Account.GetValue().Split(new char[] { '-' });
                    if (tAccountExplode.Length > 1)
                    {
                        int.TryParse(tAccountExplode[1], out tRange);
                        RangeAccess = tRange;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================