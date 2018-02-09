<?php
		//NWD File at 2017-05-16
		//Copyright NetWorkedDatas ideMobi 2017
		//Created by Jean-François CONTART
		//--------------------
		// ERROR FUNCTIONS
		//--------------------
		// Declaration
	errorDeclaration('UIG00', 'error in unique generate');
	
		// sql error
	errorDeclaration('SQL00', 'error SQL CONNEXION IMPOSSIBLE');
	
		// header error
	errorDeclaration('HEA01', 'os is empty');
	errorDeclaration('HEA11', 'os is invalid');
	errorDeclaration('HEA02', 'version is empty');
	errorDeclaration('HEA12', 'version is invalid');
	errorDeclaration('HEA03', 'lang is empty');
	errorDeclaration('HEA13', 'lang is invalid');
	errorDeclaration('HEA04', 'uuid is empty');
	errorDeclaration('HEA14', 'uuid is invalid');
	errorDeclaration('HEA05', 'hash is empty');
	errorDeclaration('HEA15', 'hash is invalid');
	errorDeclaration('HEA90', 'hash error');
	
		// param error
	errorDeclaration('PAR97', 'not json valid');
	errorDeclaration('PAR98', 'json digest is false');
	errorDeclaration('PAR99', 'json null');
	
		// version error
	errorDeclaration('GVA00', 'error in sql select Version');
	errorDeclaration('GVA99', 'block data');
	errorDeclaration('GVA01', 'stop : update app');
	errorDeclaration('GVA02', 'stop unknow version : update app');
	
		// Account error
	errorDeclaration('ACC01', 'action is empty');
	errorDeclaration('ACC02', 'action is invalid format');

	errorDeclaration('ACC03', 'appname is empty');
	errorDeclaration('ACC04', 'appname is invalid format');
	errorDeclaration('ACC05', 'appmail is empty');
	errorDeclaration('ACC06', 'appmail is invalid format');
	
	errorDeclaration('ACC10', 'email is empty');
	errorDeclaration('ACC40', 'email is invalid format');
	
	errorDeclaration('ACC11', 'password is empty');
	errorDeclaration('ACC41', 'password is invalid format');
	
	errorDeclaration('ACC12', 'confirm password is empty');
	errorDeclaration('ACC22', 'sign-up password is different to confirm password');
	errorDeclaration('ACC42', 'confirm password is invalid format');
	
	errorDeclaration('ACC13', 'old password is empty');
	errorDeclaration('ACC43', 'old password is invalid format');
	
	errorDeclaration('ACC14', 'new password is empty');
	errorDeclaration('ACC24', 'sign-up new password is different to confirm password');
	errorDeclaration('ACC44', 'new password is invalid format');
	
	
	
	errorDeclaration('ACC15', 'auuid is empty');
	errorDeclaration('ACC45', 'auuid is invalid format');
	
	errorDeclaration('ACC16', 'password is empty');
	errorDeclaration('ACC46', 'password is invalid format');
	
	
	
	errorDeclaration('ACC55', 'email or login unknow');
	errorDeclaration('ACC56', 'multi-account');
	
	errorDeclaration('ACC71', 'GoogleID is empty');
	errorDeclaration('ACC72', 'GoogleID is invalid format');
	errorDeclaration('ACC73', 'Google Graph error');
	errorDeclaration('ACC74', 'Google SDK error');
	errorDeclaration('ACC75', 'Google sql select error');
	errorDeclaration('ACC76', 'Google sql update error');
	errorDeclaration('ACC77', 'Google multi-account');
	errorDeclaration('ACC78', 'Google singin error allready log with this account');
	
	errorDeclaration('ACC81', 'FacebookID is empty');
	errorDeclaration('ACC82', 'FacebookID is invalid format');
	errorDeclaration('ACC83', 'Facebook Graph error');
	errorDeclaration('ACC84', 'Facebook SDK error');
	errorDeclaration('ACC85', 'Facebook sql select error');
	errorDeclaration('ACC86', 'Facebook sql update error');
	errorDeclaration('ACC87', 'Facebook multi-account');
	errorDeclaration('ACC88', 'Facebook singin error allready log with this account');
	
	errorDeclaration('ACC90', 'error in request select in Account');
	errorDeclaration('ACC91', 'error in request insert anonymous Account');
	errorDeclaration('ACC92', 'error in unknow account');
	errorDeclaration('ACC95', 'user is multiple');
	
	errorDeclaration('ACC99', 'user is banned');
	errorDeclaration('ACC98', 'user is banned, no sign-in');
	
		// Account sign error
	errorDeclaration('SGN01', 'sign-up error in select valid account');
	errorDeclaration('SGN02', 'sign-up error in select account by uuid');
	errorDeclaration('SGN03', 'sign-up error in update account');
	errorDeclaration('SGN04', 'sign-up error account allready linked with another email');
	errorDeclaration('SGN05', 'sign-up error multi-account by uuid');
	errorDeclaration('SGN06', 'sign-up error account allready linked with this email');
	errorDeclaration('SGN07', 'sign-up error another account allready linked with this email');
	errorDeclaration('SGN08', 'sign-up error multi-account allready linked with this email');
	
	errorDeclaration('SGN09', 'modify error in select valid account');
	errorDeclaration('SGN10', 'modify error unknow account');
	errorDeclaration('SGN11', 'sign-up error in update account');
	errorDeclaration('SGN12', 'modify error multi-account');
	errorDeclaration('SGN13', 'modify error in select valid account');
	errorDeclaration('SGN14', 'modify error email allready use in another account');
	
	errorDeclaration('SGN15', 'singin error in request account ');
	errorDeclaration('SGN16', 'singin error no account ');
	errorDeclaration('SGN17', 'singin error allready log with this account');
	errorDeclaration('SGN18', 'singin error multi-account ');
	
	errorDeclaration('SGN19', 'delete error in update account');
	
	
	errorDeclaration('SGN25', 'signanonymous error in request account ');
	errorDeclaration('SGN26', 'signanonymous error no account ');
	errorDeclaration('SGN27', 'signanonymous error allready log with this account');
	errorDeclaration('SGN28', 'signanonymous error multi-account ');
	
	errorDeclaration('SGN33', 'signout impossible with anonymous account equal to restaured account');
	
	
	errorDeclaration('SGN70', 'rescue select error');
	errorDeclaration('SGN71', 'rescue unknow user');
	errorDeclaration('SGN72', 'rescue multi-user');
	
	errorDeclaration('SGN80', 'session select error');
	errorDeclaration('SGN81', 'impossible unknow user');
	errorDeclaration('SGN82', 'impossible multi-users');
	
		// Token error
	errorDeclaration('RQT01', 'error in request token creation');
	errorDeclaration('RQT11', 'new token is not in base');
	errorDeclaration('RQT12', 'error in token select');
	errorDeclaration('RQT13', 'error in token delete');
	errorDeclaration('RQT90', 'session not exists');
	errorDeclaration('RQT91', 'session expired');
	errorDeclaration('RQT92', 'token not in base');
	errorDeclaration('RQT93', 'too much tokens in base ... reconnect you');
	errorDeclaration('RQT94', 'too much tokens in base ... reconnect you');
	
		//--------------------
		// init error state
	$ERR_BOL = false;
	$ERR_COD = '';
	$ERR_DSC = '';
	$ERR_INF = '';
		//--------------------
		// errors declared list
	$ERR_LST;
		//--------------------
		// Use to declare an error use after
	function errorDeclaration($sCode, $sDescription)
	{
		global $ERR_LST;
		$ERR_LST[$sCode] = $sDescription;
	}
		//--------------------
		// Use to insert error pre-declare in JSON's respond
	function error($sCode, $sExit=true)
	{
		global $ERR_LST, $ERR_BOL, $ERR_COD, $ERR_DSC;
		$ERR_BOL = true;
		$ERR_COD = $sCode;
		$ERR_DSC = $ERR_LST[$sCode];
    
    myLogLineReturn();
    myLog('error with code '.$sCode,'','','');
    myLogLineReturn();
    
		if ($sExit==true)
		{
			include_once ('finish.php');
			exit;
		}
		else{
			myLog('error without exit','','','');
		}
	}
		//--------------------
		// Use to insert error in JSON's respond
	function errorInfos($sCode,$sInfos)
	{
		global $ERR_INF;
		$ERR_INF = $sInfos;
		error($sCode);
	}
		//--------------------
		// return true if error in respond
	function errorDetected()
	{
		global $ERR_BOL;
		return $ERR_BOL;
	}
		//--------------------
		// use to cancel error in respond
	function errorCancel()
	{
		global $ERR_BOL, $ERR_COD, $ERR_DSC, $ERR_INF;
		$ERR_BOL = false;
		$ERR_COD = '';
		$ERR_DSC = '';
		$ERR_INF = '';
	}
		//--------------------
		// insert keys and value in JSON's respond
	function errorResult()
	{
		global $ERR_BOL, $ERR_COD, $ERR_DSC, $ERR_INF;
		if ($ERR_BOL == true)
		{
			respondAdd('error', $ERR_BOL);
			respondAdd('error_code',$ERR_COD);
			respondAdd('error_description',$ERR_DSC);
			if ($ERR_INF!='')
			{
				respondAdd('error_infos',$ERR_INF);
			}
		}
	}
		//--------------------
		// insert keys and value in JSON's respond
	function errorPossibilities()
	{
		global $ERR_LST;
		respondAdd(errorPossibilities,$ERR_LST);
	}
		//--------------------
	?>
