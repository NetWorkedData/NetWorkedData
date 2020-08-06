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
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UIM")]
    [NWDClassDescriptionAttribute("Post message to user to user ")]
    [NWDClassMenuNameAttribute("User Inter Message")]
    public partial class NWDUserInterMessage : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Sender")]
        [NWDTooltips("The Sender account reference")]
        [NWDNeedUserAvatarAttribute()]
        [NWDNeedAccountNicknameAttribute()]
        public NWDReferenceType<NWDAccount> Sender {get; set;}
        public NWDReferenceType<NWDCharacter> NPCSender {get; set;}
        [NWDTooltips("The publishing date")]
        public NWDDateTimeType PublicationDate {get; set;}
        [NWDTooltips("Receipt Acknowledgment : publisher see the meessage was reading")]
        public bool ReceiptAcknowledgment { get; set; }
        [NWDInspectorGroupEnd]
       
        [NWDInspectorGroupStart("Receiver")]
        [NWDTooltips("The Receiver account reference")]
        [NWDNeedUserAvatarAttribute()]
        [NWDNeedAccountNicknameAttribute()]
        public NWDReferenceType<NWDAccount> Receiver {get; set;}
        [NWDTooltips("The message was show at date")]
        public NWDDateTimeType DistributeDate {get; set;}
        [NWDTooltips("The message was show ")]
        public bool Distribute { get; set; }
        [NWDTooltips("The message was read")]
        public bool Read {get; set;}
        [NWDTooltips("Don't trash message, just archived to hide the message")]
        public bool Archived { get; set;}
        public NWDMessageStyle Style { get; set; }
        public NWDMessageType Type
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Message")]
        [NWDTooltips("The Message")]
        public NWDReferenceType<NWDMessage> Message { get; set; }

        [NWDTooltips("Select nickname to use in message by these tags" +
                      "\n •for Nickname : #SenderNickname# …" +
                      "")]
        public string ReplaceSenderNickname { get; set; }

        [NWDTooltips("Select characters to use in message by these tags" +
                      "\n •for Fistname : #F0# #F1# …" +
                      "\n •for Lastname : #L0# #L1# …" +
                      "\n •for Nickname : #N0# #N1# …" +
                      "\n")]
        public NWDReferencesListType<NWDCharacter> ReplaceCharacters { get; set; }

        [NWDTooltips("Select Items to use in message by these tags" +
                              "\n •for item name #I0# #I1# …" +
                              "\n •for quantity and item name #xI0# #xI1# …" +
                              "\n")]
        public NWDReferencesQuantityType<NWDItem> ReplaceItems { get; set; }

        [NWDTooltips("Select Item Packs to use item to describe the Item Packs in message by these tags" +
                              "\n •for item to describe name #G0# #G1# …" +
                              "\n •for quantity and item to describe name #xG0# #xG1# …" +
                              "\n")]
        public NWDReferencesQuantityType<NWDItemPack> ReplaceItemPacks { get; set; }

        [NWDTooltips("Select Packs to use item to describe the Pack in message by these tags" +
                              "\n •for item to describe name #P0# #P1# …" +
                              "\n •for quantity and item to describe name #xP0# #xP1# …" +
                              "\n")]
        public NWDReferencesQuantityType<NWDPack> ReplacePacks { get; set; }
        //public bool AttachmentReceived { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Push system")]
        [NWDTooltips("The published message template")]
        public NWDReferenceType<NWDMessage> PushMessage { get; set; }
        [NWDTooltips("The message in jsons for Android pushing")]
        public string PushAndroid { get; set; }
        [NWDTooltips("The message in jsons for Apple pushing")]
        public string PushApple { get; set; }
        [NWDTooltips("The ultimate date to push the message")]
        public NWDDateTimeType PushDate { get; set; }
        [NWDTooltips("The message was pushed")]
        public bool Push { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Original Inter User Message")]
        public NWDReferenceType<NWDUserInterMessage> OriginalMessage { get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================