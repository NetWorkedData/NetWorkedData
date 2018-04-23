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

    $SQL_CON = new mysqli($SQL_HOT,$SQL_USR,$SQL_PSW, $SQL_BSE);
    if ($SQL_CON->connect_errno)
	{
		exit;
    }
	else
	{
		if (getValue ('s', 's', $ereg_emailHash, 'ACC10', 'ACC40')) // I test emailrescue
		{
			if (getValue ('emailrescue', 'emailrescue', $ereg_email, 'ACC10', 'ACC40')) // I test emailrescue
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
					$tQueryC = 'UPDATE `'.$ENV.'_NWDAccount` SET `ServerHash` = \'\', `Password` = \''.$NewPassWordHash.'\', `DM` = \''.$TIME_SYNC.'\', `DS` = \''.$TIME_SYNC.'\', `'.$ENV.'Sync` = \''.$TIME_SYNC.'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($tRow['Reference']).'\' AND `AC` = 1;';
					//echo $tQueryC;
					$tResultC = $SQL_CON->query($tQueryC);
					if (!$tResultC)
					{
						// error('SGN03');
					}
					IntegrityNWDAccountReevalue ($tRow['Reference']);
				$to      = $emailrescue;
				$subject = $NWD_APP_NAM . ' : new password';
				$message = "Hello,\r\n Your password for the App $NWD_APP_NAM's account was resetted to : \r\n\r\n$NewPassWord\r\n\r\n Best regards,\r\n The $NWD_APP_NAM's team.";
				$headers = 'From: '.$NWD_RES_MAIL.'' . "\r\n" .
				'Reply-To: '.$NWD_RES_MAIL.'' . "\r\n" .
				'X-Mailer: NetWorkedData-PHP/' . phpversion();
				mail($to, $subject, $message, $headers);
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

	else{
	}

}
else{
}
	}



		//--------------------
	?>
