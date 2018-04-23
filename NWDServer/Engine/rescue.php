<?php
		//NWD Autogenerate File at 2017-05-16
		//Copyright NetWorkedDatas ideMobi 2017
		//Created by Jean-François CONTART
		//--------------------
		// WEBSERVICES FUNCTIONS
		//--------------------
		// CONSTANTS 
		//-------------------- 
	$NWD_TMA = microtime(true);
	$TIME_SYNC = $NWD_TMA;
	include_once ($PATH_BASE.'/Environment/'.$ENV.'/Engine/constants.php');
	include_once ($PATH_BASE.'/Engine/error.php');
	include_once ($PATH_BASE.'/Engine/functions.php');
	include_once ($PATH_BASE.'/Engine/values.php');
	include_once ($PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/NWDAccount/synchronization.php');
	$ereg_email = '/^([A-Z0-9a-z\.\_\%\+\-]+@[A-Z0-9a-z\.\_\%\+\-]+\.[A-Za-z]{2,6})$/';
	$ereg_password = '/^(.{24,64})$/';
	$ereg_emailHash = '/^(.{24,64})$/';
	$ereg_lang = '/^([A-Z\_\-a-z]{2,7})$/';

    $SQL_CON = new mysqli($SQL_HOT, $SQL_USR, $SQL_PSW, $SQL_BSE);
    if ($SQL_CON->connect_errno)
	{
		exit;
    }
	else
	{
		if (getValue ('lang', 'lang', $ereg_lang, 'RES02', 'RES12')) // I test emailrescue
		{
		if (getValue ('s', 's', $ereg_emailHash, 'RES03', 'RES13')) // I test emailrescue
		{
			if (getValue ('emailrescue', 'emailrescue', $ereg_email, 'RES01', 'RES11')) // I test emailrescue
				{
					$emailhash = sha1 ($emailrescue.$NWD_SLT_STR);
					$tQuery = 'SELECT * FROM `'.$ENV.'_NWDAccount` WHERE `ServerHash` = \''.$SQL_CON->real_escape_string($s).'\' AND `Email` = \''.$SQL_CON->real_escape_string($emailhash).'\' AND `AC` = 1;';
					$tResult = $SQL_CON->query($tQuery);
		if (!$tResult)
		{
			// error('SGN70');
		}
		else
		{
			if ($tResult->num_rows == 0)
			{
				// unknow user
			}
			else if ($tResult->num_rows == 1)
			{
				while($tRow = $tResult->fetch_array())
				{
					// respondAdd('rescue',true);
					// ok I have one user
					//TODO: send an email and process to change the password
					$tSeed = str_split('ACDEFHJKLMNPRTUVWXY3479'); // and any other characters
					shuffle($tSeed); // probably optional since array_is randomized; this may be redundant
					$NewPassWord = '';
					foreach (array_rand($tSeed, 12) as $k) 
					{
					$NewPassWord.= $tSeed[$k];
					}
					$NewPassWordHash = sha1 ($NewPassWord.$NWD_SLT_END);
					//$tQueryC = 'UPDATE `'.$ENV.'_NWDAccount` SET `ServerHash` = \'\', `Password` = \''.$NewPassWordHash.'\', `DM` = \''.$TIME_SYNC.'\', `DS` = \''.$TIME_SYNC.'\', `'.$ENV.'Sync` = \''.$TIME_SYNC.'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($tRow['Reference']).'\' AND `AC` = 1;';
					// //echo $tQueryC;
					// $tResultC = $SQL_CON->query($tQueryC);
					// if (!$tResultC)
					// {
					// 	// error('SGN03');
					// }
					// IntegrityNWDAccountReevalue ($tRow['Reference']);
					$tError = errorReference('ERR-RESCUE-03');
					if (isset($tError['Title']))
						{
							$subject = str_replace("{APP}",$NWD_APP_NAM,GetLocalizableString($tError['Title'], $lang));
						}
					else
						{
							$subject = $NWD_APP_NAM." : Password resetted";
						}
					if (isset($tError['Description']))
						{
							$message = str_replace("{PASSWORD}",$NewPassWord,str_replace("{APP}",$NWD_APP_NAM,GetLocalizableString($tError['Description'], $lang)));
						}
					else
						{
							$message ="Your password was resseted to : $NewPassWord";
						}
					include('Mail.php');
					$headers['From'] = $SMTP_REPLY;
					$headers['To'] = $emailrescue;
					$headers['Subject'] =$subject;
					$params['sendmail_path'] = '/usr/lib/sendmail';
					// Create the mail object using the Mail::factory method
					$mail_object = Mail::factory('smtp', array ('host' => $SMTP_HOST, 
					'auth' => true, 
					'username' => $SMTP_USER, 
					'password' => $SMTP_PSW));
					$mail_object->send($emailrescue, $headers, $message);

					$tHTML = errorReference('ERR-RESCUE-02');
					?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
					<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="fr">
					<head>
					<title><?php
					if (isset($tHTML['Title']))
						{
							echo(GetLocalizableString($tHTML['Title'], $lang));
						}
						else
						{
							echo('Title');
						}?></title>
						<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
						<meta http-equiv="Content-Language" content="en" />

					<head>
					<body>
					<div>
					<?php 
					if (isset($tHTML['Description']))
						{
							echo(GetLocalizableString($tHTML['Description'], $lang));
						}
						else
						{
							echo('Description');
						}?>
					</div>
					</body>
					</html><?php
				}
			}
			else //or more than one user with this email … strange… I push an error, user must be unique
			{
					// to much users ...
				// error('SGN72');
			}
			mysqli_free_result($tResult);
		}
	}
		}
	else{
	}

}
else{
}
	}



		//--------------------
	?>
