<?php
		//NWD File at 2017-06-16
		//Copyright NetWorkedDatas ideMobi 2017
		//Created by Jean-François CONTART
		//--------------------
	function NWDRequestTokenCreate($sUUIDHash)
	{
		global $SQL_CON, $ENV;
		
		$tToken = NWDRequestTokenGenerateToken($sUUIDHash);
		$tInsert = $SQL_CON->query('INSERT INTO `'.$ENV.'_NWDRequestToken` (`DC`, `DM`, `DD`, `AC`, `Token`, `UUIDHash`, `Integrity`) VALUES ( \''.time().'\', \''.time().'\', \'0\', \'1\', \''.$SQL_CON->real_escape_string($tToken).'\', \''.$SQL_CON->real_escape_string($sUUIDHash).'\', \'???????\' );');
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
	function NWDRequestTokenGenerateToken ($sUUIDHash)
	{
		$tRandom =  $sUUIDHash.'-'.time().'-'.rand ( 1000000000 , 9999999999 ).'-0';
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
		global $SQL_CON, $ENV;
		global $REP;
		global $token;
		$rReturn = false;
		if ($sToken=='')
		{
			$rReturn = true;
			$token = NWDRequestTokenCreate($sUUIDHash);
			$REP['token'] = $token;
		}
		else
		{
			$tQuery = 'SELECT `Token`,`DM` FROM `'.$ENV.'_NWDRequestToken` WHERE `UUIDHash` = \''.$SQL_CON->real_escape_string($sUUIDHash).'\' AND `DD` = \'0\';';
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
				else if ($tResult->num_rows == 1)
				{
						// ok I have only one token !
					while($tRow = $tResult->fetch_array())
					{
						if ($tRow['Token'] == $sToken)
						{
								// If the token is unique and equal to the reccord => valid
								// must update with the next Token
							
							$token = NWDRequestTokenGenerateToken ($sUUIDHash);
							$tQueryNewToken = 'UPDATE `'.$ENV.'_NWDRequestToken` SET `DM` = \''.time().'\', `Token` = \''.$SQL_CON->real_escape_string($token).'\' WHERE `UUIDHash` = \''.$SQL_CON->real_escape_string($sUUIDHash).'\';';
							$tResultNewToken = $SQL_CON->query($tQueryNewToken);
							if (!$tResultNewToken)
							{
								error('RQT11');
							}
							else
							{
								$REP['token'] = $token;
							}
							$rReturn = true;
						}
						else
						{
								//the token is too old and the base was purged since the last connexion ?
							$rReturn = false;
							error('RQT91');
						}
					}
				}
				else
				{
					$TokenDate = '';
					$LastTokenDate = '';
					$TokenInConflict; // only one by one;
					$TokenMajor;
					while($tRow = $tResult->fetch_array())
					{
						if ($tRow['Token'] == $sToken)
						{
							$TokenDate = $tRow['Token'];
						}
						else
						{
							$TokenInConflict[] = $tRow['Token'];
						}
						if ($tRow['Token'] > $LastTokenDate)
						{
							$LastTokenDate = $tRow['Token'];
							$TokenMajor = $tRow['Token'];
						}
					}
					if ($LastTokenDate == $TokenDate)
					{
							// ok I have the last token but another session is working …
						error('RQT93');
					}
					else
					{
						error('RQT94');
					}
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
