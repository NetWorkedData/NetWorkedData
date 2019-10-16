//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:46
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDError : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// WEB01 no network.
        /// </summary>
        static public NWDError NWDError_WEB01;
        /// <summary>
        /// HTTP error
        /// </summary>         static public NWDError NWDError_WEB02;
        /// <summary>
        /// HTTP respond is empty
        /// </summary>
        static public NWDError NWDError_WEB03;
        /// <summary>
        /// HTTP respond is empty
        /// </summary>
        static public NWDError NWDError_WEB04;

        /// <summary>
        /// Error in unique generate
        /// </summary>
        static public NWDError NWDError_UIG00;
        /// <summary>
        /// SQL no connexion!
        /// </summary>
        static public NWDError NWDError_SQL00;

        static public NWDError NWDError_HEA01;
        static public NWDError NWDError_HEA02;
        static public NWDError NWDError_HEA03;
        static public NWDError NWDError_HEA04;
        static public NWDError NWDError_HEA05;
        static public NWDError NWDError_HEA11;
        static public NWDError NWDError_HEA12;
        static public NWDError NWDError_HEA13;
        static public NWDError NWDError_HEA14;
        static public NWDError NWDError_HEA15;
        static public NWDError NWDError_HEA90;

        static public NWDError NWDError_PAR97;
        static public NWDError NWDError_PAR98;
        static public NWDError NWDError_PAR99;

        static public NWDError NWDError_GVA00;
        static public NWDError NWDError_GVA01;
        static public NWDError NWDError_GVA02;
        static public NWDError NWDError_GVA99;

        static public NWDError NWDError_ACC01;
        static public NWDError NWDError_ACC02;
        static public NWDError NWDError_ACC03;
        static public NWDError NWDError_ACC04;
        static public NWDError NWDError_ACC05;
        static public NWDError NWDError_ACC06;
        static public NWDError NWDError_ACC10;
        static public NWDError NWDError_ACC11;
        static public NWDError NWDError_ACC12;
        static public NWDError NWDError_ACC13;
        static public NWDError NWDError_ACC14;
        static public NWDError NWDError_ACC22;
        static public NWDError NWDError_ACC24;
        static public NWDError NWDError_ACC40;
        static public NWDError NWDError_ACC41;
        static public NWDError NWDError_ACC42;
        static public NWDError NWDError_ACC43;
        static public NWDError NWDError_ACC44;
        static public NWDError NWDError_ACC55;
        static public NWDError NWDError_ACC56;
        static public NWDError NWDError_ACC71;
        static public NWDError NWDError_ACC72;
        static public NWDError NWDError_ACC73;
        static public NWDError NWDError_ACC74;
        static public NWDError NWDError_ACC75;
        static public NWDError NWDError_ACC76;
        static public NWDError NWDError_ACC77;
        static public NWDError NWDError_ACC78;
        static public NWDError NWDError_ACC81;
        static public NWDError NWDError_ACC82;
        static public NWDError NWDError_ACC83;
        static public NWDError NWDError_ACC84;
        static public NWDError NWDError_ACC85;
        static public NWDError NWDError_ACC86;
        static public NWDError NWDError_ACC87;
        static public NWDError NWDError_ACC88;
        static public NWDError NWDError_ACC90;
        static public NWDError NWDError_ACC91;
        static public NWDError NWDError_ACC92;
        static public NWDError NWDError_ACC95;
        static public NWDError NWDError_ACC97;
        static public NWDError NWDError_ACC98;
        static public NWDError NWDError_ACC99;
        static public NWDError NWDError_SGN01;         static public NWDError NWDError_SGN20;
        static public NWDError NWDError_SGN03;
        static public NWDError NWDError_SGN04;
        static public NWDError NWDError_SGN05;
        static public NWDError NWDError_SGN06;
        static public NWDError NWDError_SGN07;
        static public NWDError NWDError_SGN08;
        static public NWDError NWDError_SGN09;
        static public NWDError NWDError_SGN10;
        static public NWDError NWDError_SGN11;
        static public NWDError NWDError_SGN12;
        static public NWDError NWDError_SGN13;
        static public NWDError NWDError_SGN14;
        static public NWDError NWDError_SGN15;
        static public NWDError NWDError_SGN16;
        static public NWDError NWDError_SGN17;
        static public NWDError NWDError_SGN18;
        static public NWDError NWDError_SGN19;
        static public NWDError NWDError_SGN33;

        static public NWDError NWDError_SGN70;
        static public NWDError NWDError_SGN71;
        static public NWDError NWDError_SGN72;
        static public NWDError NWDError_SGN80;
        static public NWDError NWDError_SGN81;
        static public NWDError NWDError_SGN82;

        static public NWDError NWDError_SHS01;
        static public NWDError NWDError_SHS02;

        static public NWDError NWDError_RQT01;
        static public NWDError NWDError_RQT11;
        static public NWDError NWDError_RQT12;
        static public NWDError NWDError_RQT13;
        static public NWDError NWDError_RQT14;
        static public NWDError NWDError_RQT90;
        static public NWDError NWDError_RQT91;
        static public NWDError NWDError_RQT92;
        static public NWDError NWDError_RQT93;
        static public NWDError NWDError_RQT94;
        static public NWDError NWDError_RQT95;
        static public NWDError NWDError_RQT96;
        static public NWDError NWDError_RQT97;
        static public NWDError NWDError_RQT98;
        static public NWDError NWDError_RQT99;

        /// <summary>
        /// MODEL SQL error in creation
        /// </summary>
        static public NWDError NWDError_XXx01;
        /// <summary>
        /// MODEL SQL error in creation when add primary key
        /// </summary>
        static public NWDError NWDError_XXx02;
        /// <summary>
        /// MODEL SQL error in creation when add autoincrement 
        /// </summary>
        static public NWDError NWDError_XXx03;
        /// <summary>
        /// MODEL SQL error in creation in indexation 
        /// </summary>
        static public NWDError NWDError_XXx05;
        /// <summary>
        /// MODEL SQL error in defragmentation
        /// </summary>
        static public NWDError NWDError_XXx07;
        /// <summary>
        /// MODEL SQL error in drop 
        /// </summary>
        static public NWDError NWDError_XXx08;
        /// <summary>
        /// MODEL SQL error in flush 
        /// </summary>
        static public NWDError NWDError_XXx09;
        /// <summary>
        /// MODEL SQL error in add columns
        /// </summary>
        static public NWDError NWDError_XXx11;
        /// <summary>
        /// MODEL SQL error in alter columns
        /// </summary>
        static public NWDError NWDError_XXx12;
        /// <summary>
        /// MODEL SQL error in request insert new datas before update 
        /// </summary>
        static public NWDError NWDError_XXx31;
        /// <summary>
        ///  MODEL SQL error in request select datas to update
        /// </summary>
        static public NWDError NWDError_XXx32;
        /// <summary>
        ///  MODEL SQL error in request select updatable datas
        /// </summary>
        static public NWDError NWDError_XXx33;
        /// <summary>
        ///  MODEL SQL error in request update datas in XXX (update table?)
        /// </summary>
        static public NWDError NWDError_XXx38;
        /// <summary>
        ///  MODEL SQL error more than one row for this reference
        /// </summary>
        static public NWDError NWDError_XXx39;
        /// <summary>
        ///  MODEL SQL error flush trashed
        /// </summary>
        static public NWDError NWDError_XXx40;
        /// <summary>
        ///  MODEL SQL error in update integrity
        /// </summary>
        static public NWDError NWDError_XXx91;
        /// <summary>
        ///  MODEL SQL error in columns hedear sign
        /// </summary>
        static public NWDError NWDError_XXx98;
        /// <summary>
        ///  MODEL SQL error in columns number
        /// </summary>
        static public NWDError NWDError_XXx99;
        /// <summary>
        ///  MODEL SQL error integrity of one datas is false
        /// </summary>
        static public NWDError NWDError_XXx88;
        /// <summary>
        ///  MODEL SQL error update log in XXX
        /// </summary>
        static public NWDError NWDError_XXx77;

        static public NWDError NWDError_MAINTENANCE;
        static public NWDError NWDError_OBSOLETE;
        static public NWDError NWDError_SERVER;
        //static public NWDError NWDError_RESC01;
        //static public NWDError NWDError_RESC02;
        //static public NWDError NWDError_RESC03;          static public NWDError NWDError_IPB01;                  static public NWDError NWDError_RescueRequest;         static public NWDError NWDError_RescueAnswerLogin;         static public NWDError NWDError_RescueAnswerEmail; 
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================