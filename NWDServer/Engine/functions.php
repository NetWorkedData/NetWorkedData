<?php
		//Copyright NWD 2017
		//Created by Jean-François CONTART
		//--------------------
		// FUNCTIONS
		//--------------------
	include_once ('respond.php');
	include_once ('error.php');
	include_once ('values.php');
		//--------------------
	include_once ($PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/NWDAccount/synchronization.php');
		//--------------------
		// admin ?
	$admin = false;
		//--------------------
		// ban account ?
	$ban = false;
		//--------------------
	function versionTest($sVersion)
	{
		global $ENV;
		global $SQL_CON;
		global $admin;
		
		$return = true;
		$tTested = false;
			//mylog('test version ' . $sVersion, __FILE__, __FUNCTION__, __LINE__);
		if ($ENV=='dev')
		{
			$tQuery = 'SELECT * FROM `'.$ENV.'_NWDVersion` WHERE `Version` = \''.$SQL_CON->real_escape_string($sVersion).'\' AND `BuildActive` = 1 AND `ActiveDev` = 1 AND `XX`= 0 AND `AC`= 1;';
		}
		else if ($ENV=='preprod')
		{
			$tQuery = 'SELECT * FROM `'.$ENV.'_NWDVersion` WHERE `Version` = \''.$SQL_CON->real_escape_string($sVersion).'\' AND `BuildActive` = 1 AND `ActivePreprod` = 1 AND `XX`= 0 AND `AC`= 1;';
		}
		else if ($ENV=='prod')
		{
			$tQuery = 'SELECT * FROM `'.$ENV.'_NWDVersion` WHERE `Version` = \''.$SQL_CON->real_escape_string($sVersion).'\' AND `BuildActive` = 1 AND `ActiveProd` = 1 AND `XX`= 0 AND `AC`= 1;';
		}
		$tResult = $SQL_CON->query($tQuery);
		if (!$tResult)
		{
			error('GVA00');
		}
		else
		{
			if ($tResult->num_rows == 1)
			{
					//				mylog('OK ONE RESULT FOR VERSION REQUEST ' . $sVersion, __FILE__, __FUNCTION__, __LINE__);
				while($tRow = $tResult->fetch_array())
				{
					if ($tRow['BlockDataUpdate'] == 1)
					{
						
							//						mylog('BUT BLOCK UPDATE DATAS' . $sVersion, __FILE__, __FUNCTION__, __LINE__);
						error('GVA99');
						$return = false;
					}
					if ($tRow['ForceApplicationUpdate'] == 1)
					{
							//						mylog('BUT FORCE UPDATE APP' . $sVersion, __FILE__, __FUNCTION__, __LINE__);
						error('GVA01');
						
						respondAdd('AlertTitle',$tRow['AlertTitle']);
						respondAdd('AlertMessage',$tRow['AlertMessage']);
						respondAdd('AlertButtonOK',$tRow['AlertButtonOK']);
						respondAdd('AppleStoreURL',$tRow['AppleStoreURL']);
						respondAdd('GooglePlayURL',$tRow['GooglePlayURL']);
						$return = false;
					}
				}
			}
			else
			{
					//				mylog('ERROR NO RESULT OR TOO MUCH RESULT FOR VERSION REQUEST ' . $sVersion, __FILE__, __FUNCTION__, __LINE__);
				$return = false;
				error('GVA02');
				respondAdd('AlertTitle',$tRow['AlertTitle']);
				respondAdd('AlertMessage',$tRow['AlertMessage']);
				respondAdd('AlertButtonOK',$tRow['AlertButtonOK']);
				respondAdd('AppleStoreURL',$tRow['AppleStoreURL']);
				respondAdd('GooglePlayURL',$tRow['GooglePlayURL']);
				$return = false;
			}
		}
		return $return;
	}
	
		//--------------------
		// log function
	$RRR_LOG = '';
	$RRR_LOG_CNT = 0;
		//--------------------
    function myLogLineReturn()
    {
    global $RRR_LOG,$RRR_LOG_CNT;
    $RRR_LOG_CNT++;
    $RRR_LOG.='\r'.$RRR_LOG_CNT;
    }
        //--------------------
	function myLog($sString, $sfile, $sfunction, $sline)
	{
		global $RRR_LOG,$RRR_LOG_CNT;
		$RRR_LOG_CNT++;
		$sfile = basename($sfile);
		$t = round(strlen($sfile)/4);
		$r = 20-strlen($sfile);
		
		$RRR_LOG.='\r'.$RRR_LOG_CNT.' - '.$sfile;
		for ($i=$r;$i>0;$i--)
		{
			$RRR_LOG.=' ';
		}
		if ($sfunction!='')
		{
			$eeee = $sfunction.'() line '.$sline;
		}
		else
		{
			$eeee = 'line '.$sline;
		}
		$r = 40-strlen($eeee);
		$RRR_LOG.=$eeee;
		for ($i=$r;$i>0;$i--)
		{
			$RRR_LOG.=' ';
		}
		$RRR_LOG.=$sString;
	}
		//--------------------
	function adminHashTest ($sAdminHash, $sAdminKey, $sFrequence)
	{
		$rReturn = false;
		$temporalSalt = saltTemporal($sFrequence, 0);
		$tHash = sha1($sAdminKey.$temporalSalt);
		if ($sAdminHash == $tHash)
		{
			$rReturn = true;
		}
		$temporalSaltMinor = saltTemporal($sFrequence, -1);
		$tHashMinor = sha1($sAdminKey.$temporalSaltMinor);
		if ($sAdminHash == $tHashMinor)
		{
			$rReturn = true;
		}
		$temporalSaltMajor = saltTemporal($sFrequence, +1);
		$tHashMajor = sha1($sAdminKey.$temporalSaltMajor);
		if ($sAdminHash == $tHashMajor)
		{
			$rReturn = true;
		}
		return $rReturn;
	}
		//--------------------
	function referenceRandom ($sPrefix)
	{
		$tTime = time()-1492711200; // Timestamp unix format
		return $sPrefix.'-'.$tTime.'-'.rand ( 100000 , 999999 ).'C'; // C for Certify
	}
		//--------------------
	function referenceGenerate ($sPrefix, $sTable, $sColumn)
	{
		global $SQL_CON;
		$tReference = referenceRandom($sPrefix);
		$tTested = false;
		while ($tTested == false)
		{
			$tQuery = 'SELECT `'.$sColumn.'` FROM `'.$sTable.'` WHERE `'.$sColumn.'` LIKE \''.$SQL_CON->real_escape_string($tReference).'\';';
			$tResult = $SQL_CON->query($tQuery);
			if (!$tResult)
			{
				myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);
				error('UIG00');
			}
			else
			{
				if ($tResult->num_rows == 0)
				{
					$tTested = true;
				}
				else
				{
					$tReference = referenceRandom();
				}
			}
		}
		return $tReference;
	}
		//--------------------
		// temporary account ?
		$ACC_TMP = false;
		//--------------------
	function AccountAnonymousNeeded()
	{
		global $ACC_TMP;
		$ACC_TMP = true;
		
	}
		//--------------------
	function AccountIsSigned()
	{
		global $ACC_TMP;
		$ACC_TMP = false;
	}
	
//	function OldAccountRescue($sUuid, $sSecretKey)
//	{
//		
//	}
//		//--------------------
//	function AccountAnonymeCancel()
//	{
//		respondRemove('newuser');
//		respondRemove('usertransfert');
//		respondRemove('sign');
//		respondRemove('signkey');
//		respondRemove('reloaddatas');
//	}
		//--------------------
	function AccountAnonymeGenerate($sExit=true)
	{
		global $ACC_TMP;
		$rReturn = false;
		if ($ACC_TMP == true)
		{
			global $SQL_CON;
			global $token, $uuid;
			global $ENV;
			
			$tInternalKey = '';
			$tInternalDescription = '';
			if ($ENV == 'dev')
			{
				$tInternalKey = 'Anonymous';
				$tInternalDescription = 'dev account';
			}
			
			$tNewUUID = referenceGenerate ('ACC', $ENV.'_NWDAccount', 'Reference');
			$tNewSecretKey = referenceGenerate ('SHS', $ENV.'_NWDAccount', 'SecretKey');
			$tInsert = $SQL_CON->query('INSERT INTO `'.$ENV.'_NWDAccount` (`Reference`, `SecretKey`, `DC`, `DM`, `AC`, `Ban`, `DevSync`, `PreprodSync`, `ProdSync`, `InternalKey`, `InternalDescription`) VALUES (\''.$SQL_CON->real_escape_string($tNewUUID).'\', \''.$SQL_CON->real_escape_string($tNewSecretKey).'\', \''.time().'\', \''.time().'\', \'1\', \'0\', \''.time().'\', \''.time().'\', \''.time().'\',\''.$SQL_CON->real_escape_string($tInternalKey).'\',\''.$SQL_CON->real_escape_string($tInternalDescription).'\');');
			if (!$tInsert)
			{
				error('ACC91');
			}
			else
			{
				$uuid = $tNewUUID;
				IntegrityNWDAccountReevalue ($uuid);
				respondUUID($uuid);
				respondAdd('newuser', true);
				respondAdd('usertransfert', true);
				respondAdd('sign', 'anonymous');
				respondAdd('signkey', $tNewSecretKey);
				respondAdd('reloaddatas', true);
				NWDRequestTokenIsValid($tNewUUID,'');
				$rReturn = true;
				$ACC_TMP = false;
				if ($sExit==true)
				{
					include_once ('finish.php');
					exit;
				}
			}
		}
		return $rReturn;
	}
		//--------------------
	?>
