<?php
		//NWD File at 2017-06-16
		//Copyright NetWorkedDatas ideMobi 2017
		//Created by Jean-François CONTART
		//--------------------
	function NWDRequestTokenCreate($sUUIDHash)
	{
		global $SQL_CON, $ENV, $TIME_SYNC;
		
		$tToken = NWDRequestTokenGenerateToken($sUUIDHash);
		$tInsert = $SQL_CON->query('INSERT INTO `'.$ENV.'_NWDRequestToken` (`DC`, `DM`, `DD`, `AC`, `Token`, `UUIDHash`, `Integrity`) VALUES ( \''.$TIME_SYNC.'\', \''.$TIME_SYNC.'\', \'0\', \'1\', \''.$SQL_CON->real_escape_string($tToken).'\', \''.$SQL_CON->real_escape_string($sUUIDHash).'\', \'???????\' );');
		if (!$tInsert)
		{
			error('RQT01');
			myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tInsert.'', __FILE__, __FUNCTION__, __LINE__);
		}
		else
		{
		}
		return $tToken;
	}
		//--------------------
	function NWDRequestTokenDeleteOldToken ($sUUIDHash, $sTimestamp, $sToken)
	{
		global $SQL_CON, $ENV;
		myLog('delete old token', __FILE__, __FUNCTION__, __LINE__);
		$tQuery = 'DELETE FROM `'.$ENV.'_NWDRequestToken` WHERE `UUIDHash` = \''.$SQL_CON->real_escape_string($sUUIDHash).'\' AND `DM` <= \''.$SQL_CON->real_escape_string($sTimestamp).'\' AND `Token` != \''.$SQL_CON->real_escape_string($sToken).'\';';
		$tResult = $SQL_CON->query($tQuery);
		if (!$tResult)
		{
			error('RQT14');
			myLog('ERROR IN '.$tQuery);
		}
	}
		//--------------------
	function NWDRequestTokenGenerateToken ($sUUIDHash)
	{
		global $TIME_SYNC;
		$tRandom =  $sUUIDHash.'-'.$TIME_SYNC.'-'.rand ( 1000000000 , 9999999999 ).'-0';
		return md5($tRandom);
	}
		//--------------------
	function NWDRequestTokenDeleteAllToken ($sUUIDHash)
	{
		global $SQL_CON, $ENV;
		$tQuery = 'DELETE FROM `'.$ENV.'_NWDRequestToken` WHERE `UUIDHash` = \''.$SQL_CON->real_escape_string($sUUIDHash).'\';';
		$tResult = $SQL_CON->query($tQuery);
		if (!$tResult)
		{
			error('RQT13');
			myLog('ERROR IN '.$tQuery);
		}
	}
		//--------------------
	function NWDRequestTokenReset ($sUUIDHash)
	{
		global $REP;
		global $token;
		NWDRequestTokenDeleteAllToken ($sUUIDHash);
		$token = NWDRequestTokenCreate ($sUUIDHash);
		$REP['token']=$token;
	}
		//--------------------
	function NWDRequestTokenIsValid ($sUUIDHash, $sToken)
	{
		global $SQL_CON, $ENV, $TIME_SYNC;
		global $REP;
		global $token;
		global $token_FirstUse;
		global $RTH;
		$rReturn = false;
		if ($sToken=='')
		{
			$rReturn = true;
			$token = NWDRequestTokenCreate($sUUIDHash);
			$REP['token'] = $token;
		}
		else
		{
			$tQuery = 'SELECT `Token`,`DM`, `AC` FROM `'.$ENV.'_NWDRequestToken` WHERE `UUIDHash` = \''.$SQL_CON->real_escape_string($sUUIDHash).'\' AND `DD` = \'0\';';
			$tResult = $SQL_CON->query($tQuery);
			if (!$tResult)
			{
				error('RQT12');
			}
			else
			{
				if ($tResult->num_rows == 0)
				{
						// not possible ... the token is too old and the base was purged since the last connexion
					$rReturn = false;
					error('RQT90');
				}
				else if ($tResult->num_rows <= $RTH)
				{
						// ok I have some token for this user ...
					$tTokenIsValid = false;
					$tTimestamp = 0;
					$tToken = '';
					while($tRow = $tResult->fetch_array())
					{
						myLog('find token : '.$tRow['Token'], __FILE__, __FUNCTION__, __LINE__);
						if ($tRow['Token'] == $sToken)
						{
							if ($tRow['AC'] == 0)
							{
								myLog('find OLD token reused: '.$tRow['Token'], __FILE__, __FUNCTION__, __LINE__);
								$token_FirstUse = false;
							}
							else
							{
								$token_FirstUse = true;
								$tQueryUseToken = 'UPDATE `'.$ENV.'_NWDRequestToken` SET `AC` = \'0\' WHERE `Token` = \''.$SQL_CON->real_escape_string($tRow['Token']).'\';';
								$tResultUseToken = $SQL_CON->query($tQueryUseToken);
								if (!$tResultUseToken)
									{
										error('RQT11');
									}
								else
									{
										myLog('find token, Use IT: '.$tRow['Token'], __FILE__, __FUNCTION__, __LINE__);
									}
							}
							$tTokenIsValid = true;
							$tTimestamp = $tRow['DM'];
							$tToken = $tRow['Token'];
						}
						else
						{
							// Not the good token ... newest or oldest ... don't use it to analyze
						}
					}
					if ($tTokenIsValid==true)
					{
						$rReturn = true;
						NWDRequestTokenDeleteOldToken ($sUUIDHash, $tTimestamp, $tToken);
						$token = NWDRequestTokenCreate($sUUIDHash);
						$REP['token'] = $token;
					}
					else
					{
						$rReturn = false;
						error('RQT91');
					}
					
				}
				else
				{
					// not possible ... the token are too number
					myLog('not possible ... the token are too number', __FILE__, __FUNCTION__, __LINE__);
					error('RQT93');
					
//					$TokenDate = '';
//					$LastTokenDate = '';
//					$TokenInConflict; // only one by one;
//					$TokenMajor;
//					while($tRow = $tResult->fetch_array())
//					{
//						if ($tRow['Token'] == $sToken)
//						{
//							$TokenDate = $tRow['Token'];
//						}
//						else
//						{
//							$TokenInConflict[] = $tRow['Token'];
//						}
//						if ($tRow['Token'] > $LastTokenDate)
//						{
//							$LastTokenDate = $tRow['Token'];
//							$TokenMajor = $tRow['Token'];
//						}
//					}
//					if ($LastTokenDate == $TokenDate)
//					{
//							// ok I have the last token but another session is working …
//						error('RQT93');
//					}
//					else
//					{
//						error('RQT94');
//					}
				}
				mysqli_free_result($tResult);
			}
				// If no token for this UUID : new UUID connexion => valid
				// If the token is unique (of course the last) => valid
				// If token is not unique => not valid
				// If token is the last : error close the other session or delete this session
				// If token is not the last : error close the other session
		}
	}
		//--------------------
	?>
