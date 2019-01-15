<?php
		//Copyright NWD 2017
		//Created by Jean-François CONTART
		//--------------------
		// REQUEST MANAGEMENT
		//--------------------
	include_once ('requesttoken.php');
	$ereg_os = '/^(editor|unity|ios|osx|android|web|win|wp8|ps3|ps4|psp|switch)$/';
	$ereg_version = '/^([0-9]{1,2})+(\.[0-9]{1,3})*$/';
	$ereg_lang = '/^([A-Z\_\-a-z]{2,7})$/';
	$ereg_UUID = '/^([A-Za-z0-9\-]{15,48})$/';
	$ereg_hash = '/^([A-Za-z0-9\-]{3,48})$/';
	$ereg_hash = '/^(.*)$/';
	$ereg_token = '/^(.*)$/';
		//--------------------
	if (headerValue ('os', 'os', $ereg_os, 'HEA01', 'HEA11')) // test if os infos is valid
	{
		if (headerValue ('version', 'version', $ereg_version, 'HEA02', 'HEA12')) // test if version is ok
		{
				// I must prevent admin mode in table creation
			global $admin;
			headerBrutalValue ('adminHash', 'adminHash');
			$admin = adminHashTest ($adminHash, $NWD_ADM_KEY, $NWD_SLT_TMP);
			if ($admin==true)
			{
				$versionValid = true;
			}
			else
			{
				$versionValid = versionTest($version);
			}
			if ($versionValid == true)
			{
				if (headerValue ('lang', 'lang', $ereg_lang, 'HEA03', 'HEA13')) // test if lang is ok
				{
					if (headerValue ('uuid', 'uuid', $ereg_UUID, 'HEA04', 'HEA14')) // test UUID of headers
					{
						if (headerValue ('hash', 'hash', $ereg_hash, 'HEA05', 'HEA15')) // test hash of headers
						{
							headerBrutalValue ('token', 'token');
							$temporalSalt = saltTemporal($NWD_SLT_TMP, 0);
							$tHash = sha1($os.$version.$lang.$temporalSalt.$uuid.$token);
							$temporalSaltMinor = saltTemporal($NWD_SLT_TMP, -1);
							$tHashMinor = sha1($os.$version.$lang.$temporalSaltMinor.$uuid.$token);
							$temporalSaltMajor = saltTemporal($NWD_SLT_TMP, +1);
							$tHashMajor = sha1($os.$version.$lang.$temporalSaltMajor.$uuid.$token);
							$Verif = false;
							if ($tHashMinor == $hash || $tHash ==$hash || $tHashMajor == $hash)
							{
								getParams('prm', 'prmdgt', true, false);
								if(getParams('scr', 'scrdgt', false, true)==true)
								{
									respondAdd('securePost',true);
								}
								if ($SQL_MNG == false)
								{
									$tQuery = 'SELECT `Reference`,`Ban` FROM `'.$ENV.'_NWDAccount` WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\' AND `AC` = 1;';
									$tResult = $SQL_CON->query($tQuery);
									if (!$tResult)
									{
										error('ACC90');
									}
									else
									{
										if ($tResult->num_rows == 0)
										{
											// if user is temporary user I must find the last letter equal to 'T'
											if (substr($uuid, -1) == 'T')
											{
													// I put order to create anonymous account if account is not resolve before action (sync, etc)
												AccountAnonymousNeeded(true);
											}
											else
											{
													// strange… an unknow account but not temporary … it's not possible
												error('ACC92');
											}
										}
										else if ($tResult->num_rows == 1)
										{
											while($tRow = $tResult->fetch_array())
											{
												if ($tRow['Ban'] > 0)
												{
													$ban = true;
												}
											}
										}
										else //or more than one user with this UUID … strange… I push an error, user must be unique
										{
											error('ACC95');
										}
										mysqli_free_result($tResult);
									}
										// I test the request token
									NWDRequestTokenIsValid($uuid,$token);
								}
							}
							else
							{
								error('HEA90');
							}
						}
					}
				}
			}
			else
			{
				
			}
		}
	}
//--------------------
// Ok I create a permanent account if temporary before
AccountAnonymeGenerate();
//--------------------
if ($ban == true)
	{
		error('ACC99', true);
	}
//--------------------
	?>
