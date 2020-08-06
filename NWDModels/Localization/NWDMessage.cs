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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDMessageStyle : NWEDataTypeEnumGeneric<NWDMessageStyle>
    {
        //-------------------------------------------------------------------------------------------------------------
       public static NWDMessageStyle kPopup = NWDMessageStyle.Add(0, "PopUp");
       public static NWDMessageStyle kNotification = NWDMessageStyle.Add(1, "Notification");
       public static NWDMessageStyle kAlert = NWDMessageStyle.Add(2, "Alert");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDMessageType
    {
        Temporary = 10, // delete and can be reuse when read
        Trashable = 20, // can be delete by user
        Eternal = 30, // not deletable by user
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("MES")]
    [NWDClassDescriptionAttribute("Message descriptions Class")]
    [NWDClassMenuNameAttribute("Messages")]
    [NWDInternalKeyNotEditableAttribute]
    public partial class NWDMessage : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Informations", true, true, true)]
        [NWDTooltips("Type of message")]
        public NWDMessageStyle Style { get; set; }
        public NWDMessageType Type
        {
            get; set;
        }
        public string Domain { get; set; }
        public string Code { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Description", true, true, true)]
        
        [NWDTooltips("Replace characters to use in message by these tags" +
                      "\n •for Fistname : #F0# #F1# …" +
                      "\n •for Lastname : #L0# #L1# …" +
                      "\n •for Nickname : #N0# #N1# …" +
                      "\n"+
            "Replace Items to use in message by these tags" +
                              "\n •for item name #I0# #I1# …" +
                              "\n •for quantity and item name #xI0# #xI1# …" +
                              "\n"+
            "Replace Item Packs to use item to describe the Item Packs in message by these tags" +
                              "\n •for item to describe name #G0# #G1# …" +
                              "\n •for quantity and item to describe name #xG0# #xG1# …" +
                              "\n"+
            "Replace Packs to use item to describe the Pack in message by these tags" +
                              "\n •for item to describe name #P0# #P1# …" +
                              "\n •for quantity and item to describe name #xP0# #xP1# …" +
                              "\n"+
            ""
            )]
        public NWDLocalizableStringType Title { get; set; }
        
        [NWDTooltips("Replace characters to use in message by these tags" +
                      "\n •for Fistname : #F0# #F1# …" +
                      "\n •for Lastname : #L0# #L1# …" +
                      "\n •for Nickname : #N0# #N1# …" +
                      "\n"+
            "Replace Items to use in message by these tags" +
                              "\n •for item name #I0# #I1# …" +
                              "\n •for quantity and item name #xI0# #xI1# …" +
                              "\n"+
            "Replace Item Packs to use item to describe the Item Packs in message by these tags" +
                              "\n •for item to describe name #G0# #G1# …" +
                              "\n •for quantity and item to describe name #xG0# #xG1# …" +
                              "\n"+
            "Replace Packs to use item to describe the Pack in message by these tags" +
                              "\n •for item to describe name #P0# #P1# …" +
                              "\n •for quantity and item to describe name #xP0# #xP1# …" +
                              "\n"+
            ""
            )]
        public NWDLocalizableTextType Description { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("User choose", true, true, true)]
        //public bool HasValidButton { get; set; }
        [NWDTooltips("Replace characters to use in message by these tags" +
                      "\n •for Fistname : #F0# #F1# …" +
                      "\n •for Lastname : #L0# #L1# …" +
                      "\n •for Nickname : #N0# #N1# …" +
                      "\n"+
            "Replace Items to use in message by these tags" +
                              "\n •for item name #I0# #I1# …" +
                              "\n •for quantity and item name #xI0# #xI1# …" +
                              "\n"+
            "Replace Item Packs to use item to describe the Item Packs in message by these tags" +
                              "\n •for item to describe name #G0# #G1# …" +
                              "\n •for quantity and item to describe name #xG0# #xG1# …" +
                              "\n"+
            "Replace Packs to use item to describe the Pack in message by these tags" +
                              "\n •for item to describe name #P0# #P1# …" +
                              "\n •for quantity and item to describe name #xP0# #xP1# …" +
                              "\n"+
            ""
            )]
        public NWDLocalizableStringType Validation { get; set; }
        //public NWDReferenceType<NWDAction> ValidAction { get; set; }
        //public bool HasCancelButton { get; set; }
        [NWDTooltips("Replace characters to use in message by these tags" +
                      "\n •for Fistname : #F0# #F1# …" +
                      "\n •for Lastname : #L0# #L1# …" +
                      "\n •for Nickname : #N0# #N1# …" +
                      "\n"+
            "Replace Items to use in message by these tags" +
                              "\n •for item name #I0# #I1# …" +
                              "\n •for quantity and item name #xI0# #xI1# …" +
                              "\n"+
            "Replace Item Packs to use item to describe the Item Packs in message by these tags" +
                              "\n •for item to describe name #G0# #G1# …" +
                              "\n •for quantity and item to describe name #xG0# #xG1# …" +
                              "\n"+
            "Replace Packs to use item to describe the Pack in message by these tags" +
                              "\n •for item to describe name #P0# #P1# …" +
                              "\n •for quantity and item to describe name #xP0# #xP1# …" +
                              "\n"+
            ""
            )]
        public NWDLocalizableStringType Cancel { get; set; }
        //public NWDReferenceType<NWDAction> CancelAction { get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================