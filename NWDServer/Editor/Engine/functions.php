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
		function GetLocalizableString($sString, $sLang='BASE')
		{
			$tLines = explode('•', $sString);
			foreach ($tLines as $tline)
				{
					$tKeyValues = explode(':', $tline);
					$tResult[$tKeyValues[0]] = $tKeyValues[1];
				}
			if (isset($tResult[$sLang]))
				{
					return UnprotectLocalizableString($tResult[$sLang]);
				}
			else if (isset($tResult['BASE']))
				{
					return UnprotectLocalizableString($tResult['BASE']);
				}
			else
				{
					return '';
				}
		}
		//--------------------
		function UnprotectLocalizableString($sString)
		{
			$tString =	str_replace('@1#','•',$sString);
			$tString =	str_replace('@2#',':',$tString);
			$tString =	str_replace('@3#','_',$tString);
			return $tString;
		}
		//--------------------
	function versionTest($sVersion)
	{
		global $ENV;
		global $SQL_CON;
		global $admin;
		
		$return = true;
		$tTested = false;
			//mylog('test version ' . $sVersion, __FILE__, __FUNCTION__, __LINE__);
		if ($ENV=='Dev')
		{
			$tQuery = 'SELECT * FROM `'.$ENV.'_NWDVersion` WHERE `Version` = \''.$SQL_CON->real_escape_string($sVersion).'\' AND `Buildable` = 1 AND `ActiveDev` = 1 AND `XX`= 0 AND `AC`= 1;';
		}
		else if ($ENV=='Preprod')
		{
			$tQuery = 'SELECT * FROM `'.$ENV.'_NWDVersion` WHERE `Version` = \''.$SQL_CON->real_escape_string($sVersion).'\' AND `Buildable` = 1 AND `ActivePreprod` = 1 AND `XX`= 0 AND `AC`= 1;';
		}
		else if ($ENV=='Prod')
		{
			$tQuery = 'SELECT * FROM `'.$ENV.'_NWDVersion` WHERE `Version` = \''.$SQL_CON->real_escape_string($sVersion).'\' AND `Buildable` = 1 AND `ActiveProd` = 1 AND `XX`= 0 AND `AC`= 1;';
		}
		$tResult = $SQL_CON->query($tQuery);
		if (!$tResult)
		{
			myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);
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
					if ($tRow['BlockApplication'] == 1)
					{
							//						mylog('BUT FORCE UPDATE APP' . $sVersion, __FILE__, __FUNCTION__, __LINE__);
						error('GVA01');
						
						respondAdd('AlertTitle',$tRow['AlertTitle']);
						respondAdd('AlertMessage',$tRow['AlertMessage']);
						respondAdd('AlertValidation',$tRow['AlertValidation']);
						respondAdd('OSXStoreURL',$tRow['OSXStoreURL']);
						respondAdd('IOSStoreURL',$tRow['IOSStoreURL']);
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
				respondAdd('AlertValidation',$tRow['AlertValidation']);
				respondAdd('OSXStoreURL',$tRow['OSXStoreURL']);
				respondAdd('IOSStoreURL',$tRow['IOSStoreURL']);
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
		global $TIME_SYNC;
		$tTime = $TIME_SYNC-1492711200; // Timestamp unix format
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
		// global $ACC_TMP, $ACC_NEED_USER_TRANSFERT;
		$ACC_TMP = false;
		$ACC_NEED_USER_TRANSFERT = false;
		//--------------------
	function AccountAnonymousNeeded($sUserTransfert=true)
	{
		global $ACC_TMP, $ACC_NEED_USER_TRANSFERT;
		$ACC_TMP = true;
		$ACC_NEED_USER_TRANSFERT = $sUserTransfert;
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
		global $ACC_TMP, $TIME_SYNC, $ACC_NEED_USER_TRANSFERT;
		$rReturn = false;
		if ($ACC_TMP == true)
		{
			global $SQL_CON;
			global $token, $uuid;
			global $ENV, $WSBUILD;
			global $SQL_NWDAccount_WebService;
			
			$tInternalKey = '';
			$tInternalDescription = '';
			
			$tNewUUID = referenceGenerate ('ACC', $ENV.'_NWDAccount', 'Reference');
			$tNewSecretKey = referenceGenerate ('SHS', $ENV.'_NWDAccount', 'SecretKey');
			$tInsertSQL='';
			$tInsertSQLValue='';
			//`Reference`, `DM`, `DS`, `DevSync`, `PreprodSync`, `ProdSync`, `AC`, `AppleNotificationToken`, `Ban`, `DC`, `DD`,
			// `Email`, `FacebookID`, `GoogleID`, `GoogleNotificationToken`, `InError`, 
			//`InternalDescription`, `InternalKey`, `MinVersion`, `Password`, `Preview`, `SecretKey`, `Tag`, `UseInEnvironment`, `WebServiceVersion`, `XX`
			$tInsertSQL.='INSERT INTO `'.$ENV.'_NWDAccount` (';
			$tInsertSQL.='`Reference`, '; $tInsertSQLValue.= '\''.$SQL_CON->real_escape_string($tNewUUID).'\', ';

			$tInsertSQL.='`ServerHash`, ';$tInsertSQLValue.= '\'\', ';
			$tInsertSQL.='`ServerLog`, ';$tInsertSQLValue.= '\'\', ';

			$tInsertSQL.='`DM`, ';$tInsertSQLValue.= '\''.$TIME_SYNC.'\', ';
			$tInsertSQL.='`DS`, ';$tInsertSQLValue.= '\''.$TIME_SYNC.'\', ';
			if ($ENV == 'Dev')
			{
				$tInsertSQL.='`InternalKey`, ';$tInsertSQLValue.= '\''.$SQL_CON->real_escape_string(md5($_SERVER['REMOTE_ADDR']).'-'.$WSBUILD.'-'.$TIME_SYNC).'\', ';
				$tInsertSQL.='`InternalDescription`, ';$tInsertSQLValue.= '\''.$SQL_CON->real_escape_string('Dev account').'\', ';
				$tInsertSQL.='`DevSync`, ';$tInsertSQLValue.= '\''.$TIME_SYNC.'\', ';
				$tInsertSQL.='`PreprodSync`, ';$tInsertSQLValue.= '\'-1\', ';
				$tInsertSQL.='`ProdSync`, ';$tInsertSQLValue.= '\'-1\', ';
			}
			if ($ENV == 'Preprod')
			{
				$tInsertSQL.='`InternalKey`, ';$tInsertSQLValue.= '\''.$SQL_CON->real_escape_string(md5($_SERVER['REMOTE_ADDR']).'-'.$WSBUILD.'-'.$TIME_SYNC).'\', ';
				$tInsertSQL.='`InternalDescription`, ';$tInsertSQLValue.= '\''.$SQL_CON->real_escape_string('Preprod account').'\', ';
				$tInsertSQL.='`DevSync`, ';$tInsertSQLValue.= '\'-1\', ';
				$tInsertSQL.='`PreprodSync`, ';$tInsertSQLValue.= '\''.$TIME_SYNC.'\', ';
				$tInsertSQL.='`ProdSync`, ';$tInsertSQLValue.= '\'-1\', ';
			}
			if ($ENV == 'Prod')
			{
				$tInsertSQL.='`InternalKey`, ';$tInsertSQLValue.= '\''.$SQL_CON->real_escape_string(md5($_SERVER['REMOTE_ADDR']).'-'.$WSBUILD.'-'.$TIME_SYNC).'\', ';
				$tInsertSQL.='`InternalDescription`, ';$tInsertSQLValue.= '\''.$SQL_CON->real_escape_string('Prod account').'\', ';
				$tInsertSQL.='`DevSync`, ';$tInsertSQLValue.= '\'-1\', ';
				$tInsertSQL.='`PreprodSync`, ';$tInsertSQLValue.= '\'-1\', ';
				$tInsertSQL.='`ProdSync`, ';$tInsertSQLValue.= '\''.$TIME_SYNC.'\', ';
			}
			$tInsertSQL.='`AC`, ';$tInsertSQLValue.= '\'1\', ';
			$tInsertSQL.='`AppleNotificationToken`, ';$tInsertSQLValue.= '\'\', ';
			$tInsertSQL.='`Ban`, ';$tInsertSQLValue.= '\'0\', ';
			$tInsertSQL.='`DC`, ';$tInsertSQLValue.= '\''.$TIME_SYNC.'\', ';
			$tInsertSQL.='`DD`, ';$tInsertSQLValue.= '\''.$TIME_SYNC.'\', '; 
			$tInsertSQL.='`Email`, ';$tInsertSQLValue.= '\'\', ';
			$tInsertSQL.='`FacebookID`, ';$tInsertSQLValue.= '\'\', ';
			$tInsertSQL.='`GoogleID`, ';$tInsertSQLValue.= '\'\', ';
			$tInsertSQL.='`GoogleNotificationToken`, ';$tInsertSQLValue.= '\'\', ';
			$tInsertSQL.='`InError`, ';$tInsertSQLValue.= '\'0\', ';
			$tInsertSQL.='`MinVersion`, ';$tInsertSQLValue.= '\'0.00.00\', ';
			$tInsertSQL.='`Password`, ';$tInsertSQLValue.= '\'\', ';
			$tInsertSQL.='`Preview`, ';$tInsertSQLValue.= '\'\', ';
			$tInsertSQL.='`SecretKey`, '; $tInsertSQLValue.= '\''.$SQL_CON->real_escape_string($tNewSecretKey).'\', ';
			$tInsertSQL.='`Tag`, ';$tInsertSQLValue.= '\'19\', '; // server created
			$tInsertSQL.='`UseInEnvironment`, ';$tInsertSQLValue.= '\'0\', ';
			$tInsertSQL.='`WebServiceVersion`, ';$tInsertSQLValue.= '\''.$SQL_NWDAccount_WebService.'\', ';
			$tInsertSQL.='`XX` ';$tInsertSQLValue.= '\'0\'';
			$tInsertSQL.=')';
			$tInsertSQL.=' VALUES ('.$tInsertSQLValue.');';
			$tInsert = $SQL_CON->query($tInsertSQL);
			if (!$tInsert)
			{
				respondAdd('sql', $tInsertSQL);
				error('ACC91');
			}
			else
			{
				$uuid = $tNewUUID;
				IntegrityNWDAccountReevalue ($uuid);
				respondUUID($uuid);
				respondAdd('newuser', true);
				respondAdd('usertransfert', $ACC_NEED_USER_TRANSFERT);
				respondAdd('sign', 'anonymous');
				respondAdd('signkey', $tNewSecretKey);
				respondAdd('reloaddatas', true);
				NWDRequestTokenIsValid($uuid,'');
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
function CodeRandomSizable (int $sSize)
	{
		$tMin = 1;
		while ($sSize>1)
		{
			$tMin = $tMin*10;
			$sSize--;
		}
		$tMax = ($tMin*10)-1;
		return rand ($tMin ,$tMax );
	}
	//--------------------
function RandomString($sLength = 10) {
		$tCharacters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
		$tCharactersLength = strlen($tCharacters);
		$tRandomString = '';
		for ($i = 0; $i < $sLength; $i++) {
			$tRandomString .= $tCharacters[rand(0, $tCharactersLength - 1)];
		}
		myLog('tRandomString = '.$tRandomString, __FILE__, __FUNCTION__, __LINE__);
		return $tRandomString;
	}
	//--------------------
function UniquePropertyValueFromValue($sTable, $sColumnOrign, $sColumUniqueResult, $sReference, $sNeverEmpty = true)
	{
		global $SQL_CON, $TIME_SYNC;
		$rModified = false;
		$tQuery = 'SELECT `'.$sColumnOrign.'`, `'.$sColumUniqueResult.'`, `Reference` FROM `'.$sTable.'` WHERE `Reference` = \''.$SQL_CON->real_escape_string($sReference).'\'';
		$tResult = $SQL_CON->query($tQuery);
		if (!$tResult)
			{
				//myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
				errorDeclaration('UPVFV00', 'error in select other UniqueNickname allready install');
				error('UPVFV00');
			}
		else
			{
				if ($tResult->num_rows == 1)
					{
						while($tRow = $tResult->fetch_array())
							{
								//myLog(json_encode($tRow), __FILE__, __FUNCTION__, __LINE__);
								if ($tRow[$sColumnOrign] == '' && $sNeverEmpty == true)
								{
									$tRow[$sColumnOrign] = RandomString(10);
								}
								$tOrigin = str_replace('#','',$tRow[$sColumnOrign]);
								$tOrigin = str_replace(' ','-',$tOrigin);
								$tNick = $tOrigin.'#???';
								$tNickArray = explode('#',$tRow[$sColumUniqueResult]);
								if (count($tNickArray)==2)
								{
									$tCodeAc = $tNickArray[1];
									if (preg_match ('/^([0-9]{1,12})$/', $tCodeAc))
									{
										$tNick = $tNickArray[0];
									}
								}
								else
								{
									// error 
								}
								$tTested = false;
								$tSize = 3;
								if ($tOrigin == $tNick)
									{
										//myLog('la donne est de meme nickname ', __FILE__, __FUNCTION__, __LINE__);
										// Nothing to do ? perhaps ... I test
										$tQueryTest = 'SELECT `'.$sColumUniqueResult.'` FROM `'.$sTable.'` WHERE `'.$sColumUniqueResult.'` LIKE \''.$SQL_CON->real_escape_string($tRow[$sColumUniqueResult]).'\'';
										$tResultTest = $SQL_CON->query($tQueryTest);
										if (!$tResultTest)
											{
												errorDeclaration('UPVFV01', 'error in select other UniqueNickname allready install');
												error('UPVFV01');
											}
										else
											{
												if ($tResultTest->num_rows == 1)
													{
														$tTested = true;
													}
											}
									}
								if ($tTested == false)
									{
										// I need change for an unique nickname
										while ($tTested == false)
											{
												$tPinCode = CodeRandomSizable($tSize++);
												$tQueryTestUnique = 'SELECT `'.$sColumUniqueResult.'` FROM `'.$sTable.'` WHERE `'.$sColumUniqueResult.'` LIKE \''.$SQL_CON->real_escape_string($tOrigin).'#'.$tPinCode.'\'';
												$tResultTestUnique = $SQL_CON->query($tQueryTestUnique);
												if (!$tResultTestUnique)
													{
														//myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
														errorDeclaration('UPVFV02', 'error in select other UniqueNickname allready install');
														error('UPVFV02');
													}
												else
													{
														if ($tResultTestUnique->num_rows == 0)
															{
																$tTested = true;
																$rModified = true;
																// Ok I have a good PinCode I update
																$tQueryUpdate = 'UPDATE `'.$sTable.'` SET `DM` = \''.$TIME_SYNC.'\', `'.$sColumnOrign.'` = \''.$SQL_CON->real_escape_string($tOrigin).'\', `'.$sColumUniqueResult.'` = \''.$SQL_CON->real_escape_string($tOrigin).'#'.$tPinCode.'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($sReference).'\'';
																//myLog('$tQueryUpdate', __FILE__, __FUNCTION__, __LINE__);
																//myLog($tQueryUpdate, __FILE__, __FUNCTION__, __LINE__);
																$tResultUpdate = $SQL_CON->query($tQueryUpdate);
																if (!$tResultUpdate)
																	{
																		//myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
																		errorDeclaration('UPVFV03', 'error in updtae reference object pincode');
																		error('UPVFV03');
																	}
																else
																	{
																	//myLog('pincode is update', __FILE__, __FUNCTION__, __LINE__);
																	}
															}
													}
											}
									}
							}
					}
				else
					{
					errorDeclaration('UPVFV04', 'error in select multiple reference or no reference (!=1)');
					error('UPVFV04');
					}
			}
		return $rModified;
	}
		//--------------------
	?>