<?php
	//NWD File at 2017-05-16
	//Copyright NetWorkedDatas ideMobi 2017
	//Created by Jean-François CONTART
	//--------------------
	// VALUES FUNCTIONS
	//--------------------
	// datas input
	$dico;
	//--------------------
	// aes128 Encrypt
	function aes128Encrypt($sData, $sKey, $sVector) {
		return  base64_encode(openssl_encrypt($sData,  'AES-128-ECB', $sKey, OPENSSL_RAW_DATA));
	}
	//--------------------
	// aes128 Decrypt
	function aes128Decrypt($sData, $sKey, $sVector) {
		return  openssl_decrypt(base64_decode($sData),  'AES-128-ECB', $sKey, OPENSSL_RAW_DATA);
	}
	//--------------------
	// create salt temporal for hash analyze
	function saltTemporal($sFrequence, $sIndex) {
		if ($sFrequence < 0 || $sFrequence >= 3600)
		{
			$sFrequence = 600;
		}
		$unixTime = time()+$sIndex*$sFrequence;
		return ($unixTime-($unixTime%$sFrequence));
	}
	//--------------------
	// get value of key in JSON dico and create variable with this name
	function paramValue ($varName, $key, $ereg, $errStringIfempty, $errStringifInvalid)
	{
		global $$varName,$dico;
		$return = true;
		$$varName = NULL;
		$rValue = isset($dico[$key]) ? $dico[$key] : '';// in place of  $dico[$key];
		if (valueValidity($key, $rValue, $ereg, $errStringIfempty, $errStringifInvalid))
		{
			$$varName = $rValue;
		}
		else
		{
			$return = false;
		}
		return $return;
	}
	//--------------------
	// get POST JSON value by key
	function getParams($sKey, $sDigest,$sBase64, $sCrypted) {
		global $dico, $NWD_SHA_SEC, $NWD_SHA_VEC, $NWD_SLT_STR, $NWD_SLT_END;
		$rReturn = true;
		$tParam = isset($_POST[$sKey]) ? $_POST[$sKey] : '';
		$tDigest = isset($_POST[$sDigest]) ? $_POST[$sDigest] : '';
		if ($tParam!='')
		{
			if (sha1($NWD_SLT_STR.$tParam.$NWD_SLT_END) == $tDigest)
			{
				
				if ($sCrypted==true)
				{
					$tParam = aes128Decrypt( $tParam, $NWD_SHA_SEC, $NWD_SHA_VEC);
					if ( $tParam == NULL)
					{
						errorInfos('PAR97','Data '.$sKey.' is not an json valid!');
					}
				}
				else
				{
					if ($sBase64==true)
					{
						$tParam = urldecode(base64_decode($tParam));
					}
				}
				if (!errorDetected())
				{
					$tDico = json_decode($tParam, true);
					if ($tDico == NULL)
					{
						$rReturn = false;
						errorInfos('PAR99','Data '.$sKey.' is not an json valid!');
					}
					else
					{
						$dico = $tDico;
					}
				}
			}
			else
			{
				$rReturn = false;
				errorInfos('PAR98','Digest for '.$sKey.' is false');
			}
		}
		else
		{
			$rReturn = false;
		}
		return $rReturn;
	}
	//--------------------
	// get HEADER value brutal
	function headerBrutalValue ($sVarName, $sKey)
	{
		global $$sVarName;
		$$sVarName = isset($_SERVER['HTTP_'.strtoupper($sKey)]) ? $_SERVER['HTTP_'.strtoupper($sKey)] : '';// in place of  $_SERVER[$sKey];
	}
	//--------------------
	// get HEADER value
	function headerValue ($sVarName, $sKey, $sEreg, $sErrStringIfempty, $sErrStringifInvalid)
	{
		global $$sVarName;
		$tReturn = true;
		$$sVarName = NULL;
		$tReturn = isset($_SERVER['HTTP_'.strtoupper($sKey)]) ? $_SERVER['HTTP_'.strtoupper($sKey)] : '';// in place of  $_SERVER[$sKey];
		if (valueValidity($sKey, $tReturn, $sEreg, $sErrStringIfempty, $sErrStringifInvalid))
		{
			$$sVarName = $tReturn;
		}
		else
		{
			$tReturn = false;
		}
		return $tReturn;
	}
	//--------------------
	// get POST value
	function postValue ($varName, $key, $ereg, $errStringIfempty, $errStringifInvalid)
	{
		global $$varName;
		$return = true;
		$$varName = NULL;
		$return = isset($_POST[$key]) ? $_POST[$key] : '';// in place of  $_POST[$key];
		if (valueValidity($key, $return, $ereg, $errStringIfempty, $errStringifInvalid))
		{
			$$varName = $return;
		}
		else
		{
			$return = false;
		}
		return $return;
	}
	//--------------------
	// get GET value
	function getValue ($varName, $key, $ereg, $errStringIfempty, $errStringifInvalid)
	{
		global $$varName;
		$return = true;
		$$varName = NULL;
		$return = isset($_GET[$key]) ? $_GET[$key] : '';// in place of  $_GET[$key];
		if (valueValidity($key, $return, $ereg, $errStringIfempty, $errStringifInvalid))
		{
			$$varName = $return;
		}
		else
		{
			$return = false;
		}
		return $return;
	}
	// -----------------
	// validity of value by ereg
	function valueValidity ($key, $value, $ereg, $errStringIfempty, $errStringifInvalid)
	{
		$return = true;
		if ($value == '' && $errStringIfempty != '')
		{
			$return = false;
			if ($errStringIfempty != '')
			{
				errorInfos($errStringIfempty,'Value validity of `'.$key.'` (=`'.$value.'`) is empty and it is not possible');
			}
		}
		else
		{
			if ($ereg!='' && $errStringifInvalid!='')
			{
				if (!preg_match ($ereg, $value))
				{
					$return = false;
					if ($errStringifInvalid != '')
					{
						errorInfos($errStringifInvalid,'Value validity of `'.$key.'` (=`'.$value.'`) is not complicent with regular expression rules');
					}
				}
			}
		}
		return $return;
	}
	//--------------------
?>
