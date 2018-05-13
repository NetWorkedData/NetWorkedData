<?php
		//NWD File at 2017-05-16
		//Copyright NetWorkedDatas ideMobi 2017
		//Created by Jean-François CONTART
		//--------------------
		// AUTHENTIFICATION FUNCTIONS
		//--------------------
		//	error_reporting(E_ALL);
		//	error_reporting(-1);
		//--------------------
	$ereg_appname = '/^(.*)$/';
	$ereg_facebook = '/^(.*)$/';
	$ereg_google = '/^(.*)$/';
	$ereg_unity = '/^(.*)$/';
	$ereg_twitter = '/^(.*)$/';
	$ereg_social_force = '/^(yes|no)$/';
	$ereg_action = '/^(signanonymous|rescue|session|signin|signup|signout|modify|reinitialize|delete|facebook|twitter|google|unity|joker|google_remove|facebook_remove)$/';
	$ereg_email = '/^([A-Z0-9a-z\.\_\%\+\-]+@[A-Z0-9a-z\.\_\%\+\-]+\.[A-Za-z]{2,6})$/';
	$ereg_password = '/^(.{24,64})$/';
	$ereg_emailHash = '/^(.{24,64})$/';
	$ereg_auuidHash = '/^([A-Za-z0-9\-]{15,48})$/';
	$ereg_apasswordHash = '/^(.{12,64})$/';
		//--------------------
	if (!errorDetected())
	{
		if (paramValue ('action', 'action', $ereg_action, 'ACC01', 'ACC02')) // test if action is valid
		{
			if ($action == 'rescue')
			{
				if (paramValue ('emailrescue', 'emailrescue', $ereg_email, 'ACC10', 'ACC40')) // I test emailrescue
				{
					$emailhash = sha1 ($emailrescue.$NWD_SLT_STR);
					
					$tQuery = 'SELECT * FROM `'.$ENV.'_NWDAccount` WHERE `Email` = \''.$SQL_CON->real_escape_string($emailhash).'\' AND `AC` = 1;';
					$tResult = $SQL_CON->query($tQuery);
					if (!$tResult)
					{
						error('SGN70');
					}
					else
					{
						if ($tResult->num_rows == 0)
						{
								// unknow user
							error('SGN71');
						}
						else if ($tResult->num_rows == 1)
						{
							while($tRow = $tResult->fetch_array())
							{
							respondAdd('rescue',true);
							$s = sha1($TIME_SYNC.$emailrescue.$NWD_SLT_SRV);
								// ok I have one user
								//TODO: send an email and process to change the password
							$tQueryC = 'UPDATE `'.$ENV.'_NWDAccount` SET `ServerHash` = \''.$s.'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($tRow['Reference']).'\' AND `AC` = 1;';
							$tResultC = $SQL_CON->query($tQueryC);
							if (!$tResultC)
							{
								error('SGN03');
							}
							$tResetURL = $HTTP_URL.'/Environment/'.$ENV.'/rescue.php?lang='.$lang.'&s='.$s.'&emailrescue='.$emailrescue;
							$tError = errorReference('ERR-RESCUE-01');
							if (isset($tError['Title']))
							{
								$subject = str_replace("{APP}",$NWD_APP_NAM, GetLocalizableString($tError['Title'], $lang));
							}
								else
							{
									$subject = $NWD_APP_NAM.': forgotten password';
							}
							if (isset($tError['Description']))
							{
								$message = str_replace("{URL}",$tResetURL,str_replace("{APP}",$NWD_APP_NAM,GetLocalizableString($tError['Description'], $lang)));
							}
							else
							{
								$message = "Hello,\n\r\n\r";
								$message.= "You have requested to reset your password for the ".$NWD_APP_NAM." application.\n\r";
								$message.= "If this request does not emanate from your action, ignore this email.\n\r";
								$message.= "If you wish to reset your password, click on the link below:\n\r";
								$message.= "\n\r".$tResetURL."\n\r\n\r";
								$message.= "Best regards,\n\r";
								$message.= "The ".$NWD_APP_NAM." team";
							}
							include('Mail.php');
							$headers['From'] = $SMTP_REPLY;
							$headers['To'] = $emailrescue;
							$headers['Subject'] =$subject;
							$params['sendmail_path'] = '/usr/lib/sendmail';
							// Create the mail object using the Mail::factory method
							$mail_object = Mail::factory('smtp', array ('host' => $SMTP_HOST, 'auth' => true, 'username' => $SMTP_USER, 'password' => $SMTP_PSW));
							$mail_object->send($emailrescue, $headers, $message);
							}
						}
						else //or more than one user with this email … strange… I push an error, user must be unique
						{
								// to much users ...
							error('SGN72');
						}
						mysqli_free_result($tResult);
					}
				}
			}
				//---- SESSION TEST ----
				// I test session is ok
			if ($action == 'session')
			{
					// Ok I create a permanent account if temporary before
				// AccountAnonymeGenerate(); // Did in request file
					// I have a valid account anyway now
				$tQuery = 'SELECT * FROM `'.$ENV.'_NWDAccount` WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\' AND `AC` = 1;';
				$tResult = $SQL_CON->query($tQuery);
				if (!$tResult)
				{
					error('SGN80');
				}
				else
				{
					if ($tResult->num_rows == 0)
					{
							// unknow user
						error('SGN81');
					}
					else if ($tResult->num_rows == 1)
					{
							// ok I have one user
						while($tRow = $tResult->fetch_array())
						{
							respondAdd('session',true);
							if ($tRow['Ban'] > 0)
							{
								$ban=true;
								error('ACC99');
							}
							if ($tRow['Email']!='')
							{
								respondAdd('sign','loginpassword');
							}
							else if ($tRow['FacebookID']!='')
							{
								respondAdd('sign','facebook');
							}
							else if ($tRow['GoogleID']!='')
							{
								respondAdd('sign','google');
							}
							else
							{
								respondAdd('sign','anonymous');
							}
							
						}
					}
					else //or more than one user with this email … strange… I push an error, user must be unique
					{
							// to much users ...
						error('SGN82');
					}
					mysqli_free_result($tResult);
				}
			}
				//---- SIGN IN ----
				// I sign in with the good value
			if ($action == 'signin')
			{
				if (paramValue ('email', 'email', $ereg_emailHash, 'ACC10', 'ACC40')) // I test hashsec
				{
					if (paramValue ('password', 'password', $ereg_password, 'ACC11', 'ACC41')) // I test hashsec
					{
							// I get the password uuid hash
						$tQuery = 'SELECT `Reference`, `Ban` FROM `'.$ENV.'_NWDAccount` WHERE `Email` = \''.$SQL_CON->real_escape_string($email).'\' AND `Password` = \''.$SQL_CON->real_escape_string($password).'\' AND `AC` = 1;';
						$tResult = $SQL_CON->query($tQuery);
						if (!$tResult)
						{
							myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);
							error('SGN15');
						}
						else
						{
							if ($tResult->num_rows == 0)
							{
									// unknow user
								error('SGN16');
							}
							else if ($tResult->num_rows == 1)
							{
									// ok I have one user
								while($tRow = $tResult->fetch_array())
								{
									
									if ($tRow['Ban'] > 0)
									{
										$ban = true;
										error('ACC98');
									}
									else
									{
										AccountIsSigned();
										if ($uuid != $tRow['Reference'])
										{
											respondNeedReloadData();
											respondUUID($tRow['Reference']);
											respondAdd('signin',true);
											respondAdd('sign','loginpassword');
										}
										else
										{
											error('SGN17');
										}
										$uuid = $tRow['Reference'];
										$token = NWDRequestTokenReset ($uuid); // reset connexion to zero
									}
								}
							}
							else //or more than one user with this email … strange… I push an error, user must be unique
							{
									// to much users ...
								error('SGN18');
							}
							mysqli_free_result($tResult);
						}
					}
				}
			}
				//---- SIGN UP ----
				// I sign up with the good value
			if ($action == 'signup')
			{
				if (paramValue ('email', 'email', $ereg_emailHash, 'ACC10', 'ACC40')) // I test hashsec
				{
					if (paramValue ('password', 'password', $ereg_password, 'ACC11', 'ACC41')) // I test hashsec
					{
						if (paramValue ('password_confirm', 'password_confirm', $ereg_password, 'ACC12', 'ACC42')) // I test hashsec
						{
							if ($password == $password_confirm)
							{
									// Ok I create a permanent accpunt if temporary before
									// AccountAnonymeGenerate(); // Did in request file
									// I have a valid account anyway now
								
								if ($ban == false)
								{
										// Ok I register this account
									$tQuery = 'SELECT `Reference` FROM `'.$ENV.'_NWDAccount` WHERE `Email` = \''.$SQL_CON->real_escape_string($email).'\' AND `AC` = 1;';
									$tResult = $SQL_CON->query($tQuery);
									if (!$tResult)
									{
										error('SGN01');
									}
									else
									{
										if ($tResult->num_rows == 0)
										{
											$tQueryB = 'SELECT `Reference`,`Email`,`Password` FROM `'.$ENV.'_NWDAccount` WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\' AND `AC` = 1;';
											$tResultB = $SQL_CON->query($tQueryB);
											if (!$tResultB)
											{
												errorInfos('SGN02',$tQueryB);
											}
											else
											{
												if ($tResultB->num_rows == 1)
												{
													$tRow = $tResultB->fetch_array();
													if ($tRow['Email']=='' && $tRow['Password']=='')
													{
														$tInternalKey = '';
														$tInternalDescription = '';
														if ($ENV == 'Dev')
														{
															$tInternalKey = ''.$email.'';
															$tInternalDescription = 'dev account';
														}
														$tQueryC = 'UPDATE `'.$ENV.'_NWDAccount` SET `DM` = \''.$TIME_SYNC.'\', `Email` = \''.$SQL_CON->real_escape_string($email).'\', `Password` = \''.$SQL_CON->real_escape_string($password).'\', `InternalKey` = \''.$SQL_CON->real_escape_string($tInternalKey).'\', `InternalDescription` = \''.$SQL_CON->real_escape_string($tInternalDescription).'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\' AND `AC` = 1;'; // AND `Email` = \'\' AND `Password` = \'\'
														$tResultC = $SQL_CON->query($tQueryC);
														if (!$tResultC)
														{
															error('SGN03');
														}
														else
														{
															IntegrityNWDAccountReevalue ($uuid);
															$token = NWDRequestTokenReset ($tRow['Reference']); // reset connexion to zero
															respondAdd('signup',true);
															respondAdd('sign','loginpassword');
														}
													}
													else
													{
														error('SGN04');
													}
												}
												else
												{
													error('SGN05');
												}
											}
										}
										else if ($tResult->num_rows == 1)
										{
											while($tRow = $tResult->fetch_array())
											{
												if ($tRow['Reference'] == $uuid)
												{
													error('SGN06');
												}
												else
												{
													error('SGN07');
												}
											}
										}
										else //or more than one user with this email … strange… I push an error, user must be unique
										{
											error('SGN08');
										}
										mysqli_free_result($tResult);
									}
								}
							}
							else
							{
								error('ACC22');
							}
						}
					}
				}
			}
				//---- MODIFY ----
				// I modifiy with the good value
			if ($action == 'modify')
			{
				if (paramValue ('email', 'email', $ereg_emailHash, 'ACC10', 'ACC40')) // I test hashsec
				{
					if (paramValue ('old_password', 'old_password', $ereg_password, 'ACC13', 'ACC43')) // I test hashsec
					{
						if (paramValue ('new_password', 'new_password', $ereg_password, 'ACC14', 'ACC44')) // I test hashsec
						{
							if (paramValue ('password_confirm', 'password_confirm', $ereg_password, 'ACC12', 'ACC42')) // I test hashsec
							{
								if ($new_password == $password_confirm)
								{
									
										// Ok I create a permanent accpunt if temporary before
									// AccountAnonymeGenerate();
										// I have a valid account anyway now
									
									if ($ban == false)
									{
										
										$tQuery = 'SELECT `Reference` FROM `'.$ENV.'_NWDAccount` WHERE `Email` = \''.$SQL_CON->real_escape_string($email).'\' AND `Reference` != \''.$SQL_CON->real_escape_string($uuid).'\' AND `AC` = 1;';
										$tResult = $SQL_CON->query($tQuery);
										if (!$tResult)
										{
											error('SGN13');
										}
										else
										{
											if ($tResult->num_rows > 0)
											{
												error('SGN14');
											}
											else
											{
												$tQueryB = 'SELECT `Reference` FROM `'.$ENV.'_NWDAccount` WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\' AND `Password` = \''.$SQL_CON->real_escape_string($old_password).'\' AND `AC` = 1;';
												$tResultB = $SQL_CON->query($tQueryB);
												if (!$tResultB)
												{
													error('SGN09');
												}
												else
												{
													if ($tResultB->num_rows == 0)
													{
														error('SGN10');
													}
													else if ($tResultB->num_rows == 1)
													{
														$tInternalKey = '';
														$tInternalDescription = '';
														if ($ENV == 'Dev')
														{
															$tInternalKey = ''.$email.'';
															$tInternalDescription = 'dev account';
														}
														$tQueryC = 'UPDATE `'.$ENV.'_NWDAccount` SET `DM` = \''.$TIME_SYNC.'\', `Email` = \''.$SQL_CON->real_escape_string($email).'\', `Password` = \''.$SQL_CON->real_escape_string($new_password).'\', `InternalKey` = \''.$SQL_CON->real_escape_string($tInternalKey).'\', `InternalDescription` = \''.$SQL_CON->real_escape_string($tInternalDescription).'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\' AND `AC` = 1;';
														$tResultC = $SQL_CON->query($tQueryC);
														if (!$tResultC)
														{
															error('SGN11');
														}
														else
														{
															IntegrityNWDAccountReevalue ($uuid);
															respondAdd('modify', true);
														}
													}
													else
													{
														error('SGN12');
													}
												}
											}
										}
									}
								}
								else
								{
									error('ACC24');
								}
							}
						}
					}
				}
			}
				//---- ADD ACCOUNT FACEBOOK ID ----
				// I connect this account with facebook ID
			if ($action == 'facebook')
			{
				if (paramValue ('id', 'id', $ereg_facebook, 'ACC81', 'ACC82')) // I test facebook id
				{
					
					require_once($PATH_BASE.'/Facebook/autoload.php');
					require_once($PATH_BASE.'/Facebook/Facebook.php');
					
					myLog( '$sAppKey: ' . $NWD_FCB_AID, __FILE__, __FUNCTION__, __LINE__ );
					myLog( '$sAppSecret: ' . $NWD_FCB_SRT, __FILE__, __FUNCTION__, __LINE__ );
					myLog( '$sFacebookToken: ' . $id, __FILE__, __FUNCTION__, __LINE__ );
					
					$fb = new Facebook\Facebook(['app_id' => $NWD_FCB_AID,'app_secret' => $NWD_FCB_SRT,'default_graph_version' => 'v2.2',]);
					
					try {
							// Returns a `Facebook\FacebookResponse` object
						$response = $fb->get('/me?fields=id,name', $id);
					} catch(Facebook\Exceptions\FacebookResponseException $e) {
						errorInfos('ACC83','Graph returned an error: ' . $e->getMessage());
					} catch(Facebook\Exceptions\FacebookSDKException $e) {
						errorAdd('ACC84','Facebook SDK returned an error: ' . $e->getMessage());
					}
					
					$user = $response->getGraphUser();
						// myLog ( 'Name: ' . $user['name'], __FILE__, __FUNCTION__, __LINE__);
						// myLog ( 'ID: ' . $user['id'], __FILE__, __FUNCTION__, __LINE__);
					$tFacebookID = $user['id'];
						// Ok I test the facebook ID in my database
						// I need to find the facebookID in my database and check if the UUID is the good
					$tQuery = 'SELECT `Reference`, `ban` FROM `'.$ENV.'_NWDAccount` WHERE `FacebookID` LIKE \''.$SQL_CON->real_escape_string($tFacebookID).'\' AND `AC` = 1;';
					$tResult = $SQL_CON->query($tQuery);
					if (!$tResult)
					{
						myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);
						error('ACC85');
					}
					else
					{
						if ($tResult->num_rows == 0)
						{
							if ($ban==false)
							{
									// Ok I create a permanent account if temporary before
								// AccountAnonymeGenerate();
									// Ok no facebookID in my Database
								$tInternalKey = '';
								$tInternalDescription = '';
								if ($ENV == 'Dev')
								{
									$tInternalKey = 'facebook ID';
									$tInternalDescription = 'dev account';
								}
									// I had the social value to this UUID and return the UUID
								$tQueryB = 'UPDATE `'.$ENV.'_NWDAccount` SET `DM` = \''.$TIME_SYNC.'\', `FacebookID` = \''.$SQL_CON->real_escape_string($tFacebookID).'\', `InternalKey` = \''.$SQL_CON->real_escape_string($tInternalKey).'\', `InternalDescription` = \''.$SQL_CON->real_escape_string($tInternalDescription).'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\';';
								$tResultB = $SQL_CON->query($tQueryB);
								if (!$tResultB)
								{
									myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryB.'', __FILE__, __FUNCTION__, __LINE__);
									error('ACC86');
								}
								else
								{
										// Good
									IntegrityNWDAccountReevalue ($uuid);
									myLog('User is modified and social facebook was add', __FILE__, __FUNCTION__, __LINE__);
									$token = NWDRequestTokenReset ($tRow['Reference']); // reset connexion to zero
									respondAdd('signup',true);
									respondAdd('facebook_signup',true);
									respondAdd('sign','facebook');
								}
							}
							
						}
						else if ($tResult->num_rows == 1)
						{
								// Oh ... facebookID allready present
							while($tRow = $tResult->fetch_array())
							{
								if ($tRow['Reference'] == $uuid)
								{
										// Good It's the good uuid ... do nothing, all is ok!
								}
								else
								{
									if ($tRow['Ban'] > 0)
									{
										$ban = true;
										error('ACC98');
									}
									else
									{
											// I have a valid account
										AccountIsSigned();
										
										if ($uuid != $tRow['Reference'])
										{
											respondNeedReloadData();
											respondUUID($tRow['Reference']);
											respondAdd('signin',true);
											respondAdd('facebook_signin',true);
											respondAdd('sign','facebook');
										}
										else
										{
											error('ACC88');
										}
										$uuid = $tRow['Reference'];
										$token = NWDRequestTokenReset ($uuid); // reset connexion to zero
									}
								}
							}
						}
						else //or more than one user with this facebook id … strange… I push an error, user must be unique
						{
							error('ACC87');
						}
						mysqli_free_result($tResult);
					}
					
				}
			}
				// I connect this account with google ID
			if ($action == 'google')
			{
				if (paramValue ('id', 'id', $ereg_google, 'ACC71', 'ACC72')) // I test google id
				{
					$tGoogleID = $id;
						// Ok I test the facebook ID in my database
						// I need to find the GoogleID in my database and check if the UUID is the good
					$tQuery = 'SELECT `Reference`, `ban` FROM `'.$ENV.'_NWDAccount` WHERE `GoogleID` LIKE \''.$SQL_CON->real_escape_string($tGoogleID).'\' AND `AC` = 1;';
					$tResult = $SQL_CON->query($tQuery);
					if (!$tResult)
					{
						myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);
						error('ACC75');
					}
					else
					{
						if ($tResult->num_rows == 0)
						{
							if ($ban==false)
							{
									// Ok I create a permanent account if temporary before
								// AccountAnonymeGenerate();
									// Ok no $tGoogleID in my Database
								$tInternalKey = '';
								$tInternalDescription = '';
								if ($ENV == 'Dev')
								{
									$tInternalKey = 'Google ID';
									$tInternalDescription = 'dev account';
								}
									// I had the social value to this UUID and return the UUID
								$tQueryB = 'UPDATE `'.$ENV.'_NWDAccount` SET `DM` = \''.$TIME_SYNC.'\', `GoogleID` = \''.$SQL_CON->real_escape_string($tGoogleID).'\', `InternalKey` = \''.$SQL_CON->real_escape_string($tInternalKey).'\', `InternalDescription` = \''.$SQL_CON->real_escape_string($tInternalDescription).'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\';';
								$tResultB = $SQL_CON->query($tQueryB);
								if (!$tResultB)
								{
									myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryB.'', __FILE__, __FUNCTION__, __LINE__);
									error('ACC76');
								}
								else
								{
										// Good
									IntegrityNWDAccountReevalue ($uuid);
									myLog('User is modified and social google was add', __FILE__, __FUNCTION__, __LINE__);
									$token = NWDRequestTokenReset ($tRow['Reference']); // reset connexion to zero
									respondAdd('signup',true);
									respondAdd('google_signup',true);
									respondAdd('sign','google');
								}
							}
							
						}
						else if ($tResult->num_rows == 1)
						{
								// Oh ... googleID allready present
							while($tRow = $tResult->fetch_array())
							{
								if ($tRow['Reference'] == $uuid)
								{
										// Good It's the good uuid ... do nothing, all is ok!
								}
								else
								{
									if ($tRow['ban'] > 0)
									{
										$ban = true;
										error('ACC98');
									}
									else
									{
											// I have a valid account
										AccountIsSigned();
										if ($uuid != $tRow['Reference'])
										{
											respondNeedReloadData();
											respondUUID($tRow['Reference']);
											respondAdd('signin',true);
											respondAdd('google_signin',true);
											respondAdd('sign','google');
										}
										else
										{
											error('ACC78');
										}
										$uuid = $tRow['Reference'];
										$token = NWDRequestTokenReset ($uuid); // reset connexion to zero
									}
								}
							}
						}
						else //or more than one user with this google id … strange… I push an error, user must be unique
						{
							error('ACC77');
						}
						mysqli_free_result($tResult);
					}
				}
			}


				//---- ADD ACCOUNT FACEBOOK ID ----
				// I connect this account with facebook ID
				if ($action == 'facebook_remove')
				{
					if (paramValue ('id', 'id', $ereg_facebook, 'ACC81', 'ACC82')) // I test facebook id
					{
						
						require_once($PATH_BASE.'/Facebook/autoload.php');
						require_once($PATH_BASE.'/Facebook/Facebook.php');
						
						myLog( '$sAppKey: ' . $NWD_FCB_AID, __FILE__, __FUNCTION__, __LINE__ );
						myLog( '$sAppSecret: ' . $NWD_FCB_SRT, __FILE__, __FUNCTION__, __LINE__ );
						myLog( '$sFacebookToken: ' . $id, __FILE__, __FUNCTION__, __LINE__ );
						
						$fb = new Facebook\Facebook(['app_id' => $NWD_FCB_AID,'app_secret' => $NWD_FCB_SRT,'default_graph_version' => 'v2.2',]);
						
						try {
								// Returns a `Facebook\FacebookResponse` object
							$response = $fb->get('/me?fields=id,name', $id);
						} catch(Facebook\Exceptions\FacebookResponseException $e) {
							errorInfos('ACC83','Graph returned an error: ' . $e->getMessage());
						} catch(Facebook\Exceptions\FacebookSDKException $e) {
							errorAdd('ACC84','Facebook SDK returned an error: ' . $e->getMessage());
						}
						
						$user = $response->getGraphUser();
							// myLog ( 'Name: ' . $user['name'], __FILE__, __FUNCTION__, __LINE__);
							// myLog ( 'ID: ' . $user['id'], __FILE__, __FUNCTION__, __LINE__);
						$tFacebookID = $user['id'];
							// Ok I test the facebook ID in my database
							// I need to find the facebookID in my database and check if the UUID is the good
						$tQuery = 'SELECT `Reference`, `ban` FROM `'.$ENV.'_NWDAccount` WHERE `FacebookID` LIKE \''.$SQL_CON->real_escape_string($tFacebookID).'\' AND `AC` = 1;';
						$tResult = $SQL_CON->query($tQuery);
						if (!$tResult)
						{
							myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);
							error('ACC85');
						}
						else
						{
							if ($tResult->num_rows == 1)
							{
									// Oh ... facebookID allready present
								
								// Oh ... googleID allready present
							while($tRow = $tResult->fetch_array())
							{
								if ($tRow['Reference'] == $uuid)
								{
					
									$tQueryD = 'UPDATE `'.$ENV.'_NWDAccount` SET `DM` = \''.$TIME_SYNC.'\', `FacebookID` = \'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\';';
									$tResultD = $SQL_CON->query($tQueryD);
									if (!$tResultD)
									{
										myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryD.'', __FILE__, __FUNCTION__, __LINE__);
										//error('ACC76');
									}
									else
									{
											// Good
										IntegrityNWDAccountReevalue ($uuid);
										myLog('User is modified and social google was remove', __FILE__, __FUNCTION__, __LINE__);
										respondAdd('facebook_remove',true);
									}
								}
							}
							}
							else //or more than one user with this facebook id … strange… I push an error, user must be unique
							{
								error('ACC87');
							}
							mysqli_free_result($tResult);
						}
						
					}
				}


			// REMOVE SOCIAL CONNECT
			if ($action == 'google_remove')
			{
				if (paramValue ('id', 'id', $ereg_google, 'ACC71', 'ACC72')) // I test google id
				{
					$tGoogleID = $id;
						// Ok I test the facebook ID in my database
						// I need to find the GoogleID in my database and check if the UUID is the good
					$tQuery = 'SELECT `Reference`, `ban` FROM `'.$ENV.'_NWDAccount` WHERE `GoogleID` LIKE \''.$SQL_CON->real_escape_string($tGoogleID).'\' AND `AC` = 1;';
					$tResult = $SQL_CON->query($tQuery);
					if (!$tResult)
					{
						myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);
						error('ACC75');
					}
					else
					{
						if ($tResult->num_rows == 1)
						{
								// Oh ... googleID allready present
							while($tRow = $tResult->fetch_array())
							{
								if ($tRow['Reference'] == $uuid)
								{
					
									$tQueryD = 'UPDATE `'.$ENV.'_NWDAccount` SET `DM` = \''.$TIME_SYNC.'\', `GoogleID` = \'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\';';
									$tResultD = $SQL_CON->query($tQueryD);
									if (!$tResultD)
									{
										myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryD.'', __FILE__, __FUNCTION__, __LINE__);
										//error('ACC76');
									}
									else
									{
											// Good
										IntegrityNWDAccountReevalue ($uuid);
										myLog('User is modified and social google was remove', __FILE__, __FUNCTION__, __LINE__);
										respondAdd('google_remove',true);
									}
								}
							}
						}
						else //or more than one user with this google id … strange… I push an error, user must be unique
						{
							error('ACC77');
						}
						mysqli_free_result($tResult);
					}
				}
			}





				//---- SIGN OUT ----
				// I sign in with the good value
			if ($action == 'signout')
			{
					//NWDRequestTokenReset ($uuid); // reset connexion to zero
				respondAdd('signout', true);
				AccountAnonymousNeeded(false);
				
				if (paramValue ('auuid', 'auuid', $ereg_auuidHash, 'ACC15', 'ACC45')) // I test hashsec
				{
					if ($auuid == $uuid)
					{
						error('SGN33');
					}
					else
					{
					$action = 'signanonymous';
					}
				}
			}
				//---- SIGN ANONYMOUS ----
				// I sign in with the good value
			if ($action == 'signanonymous')
			{
				if (paramValue ('auuid', 'auuid', $ereg_auuidHash, 'ACC15', 'ACC45')) // I test hashsec
				{
					if (paramValue ('apassword', 'apassword', $ereg_apasswordHash, 'ACC16', 'ACC46')) // I test hashsec
					{
							// if user is temporary user I must find the last letter equal to 'T'
						if (substr($auuid, -1) == 'T')
						{
								// I put order to create anonymous account if account is not resolve before action (sync, etc)
							AccountAnonymousNeeded(false);
							respondAdd('create-anonymous', true);
						}
						else
						{
								// I get the password uuid hash
							$tQuery = 'SELECT * FROM `'.$ENV.'_NWDAccount` WHERE `Reference` = \''.$SQL_CON->real_escape_string($auuid).'\' AND `SecretKey` = \''.$SQL_CON->real_escape_string($apassword).'\' AND `AC` = 1;';
							$tResult = $SQL_CON->query($tQuery);
							if (!$tResult)
							{
								myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);
								error('SGN25');
							}
							else
							{
								if ($tResult->num_rows == 0)
								{
										// unknow user
										//error('SGN26');
										//create new anonymous account
										AccountAnonymeGenerate();
								}
								else if ($tResult->num_rows == 1)
								{
										// ok I have one user
									while($tRow = $tResult->fetch_array())
									{
										
										if ($tRow['Ban'] > 0)
										{
											$ban = true;
											error('ACC98');
										}
										else
										{
											if ($tRow['Email'] == '' && $tRow['Password'] == '' && $tRow['FacebookID'] == '' && $tRow['GoogleID'] == '')
											{
												AccountIsSigned();
												if ($uuid != $tRow['Reference'])
												{
													respondNeedReloadData();
													respondUUID($tRow['Reference']);
													respondAdd('sign-anonymous',true);
													respondAdd('sign','anonymous');
												}
												else
												{
													error('SGN27');
												}
												$uuid = $tRow['Reference'];
												$token = NWDRequestTokenReset ($uuid); // reset connexion to zero
											}
											else
											{
												respondAdd('create-anonymous', true);
											}
										}
									}
								}
								else //or more than one user with this email … strange… I push an error, user must be unique
								{
										// to much users ...
									error('SGN28');
								}
								mysqli_free_result($tResult);
							}
						}
					}
				}
			}
				//---- DELETE ----
				// I delete the account (ban)
			if ($action == 'delete')
			{
				if (paramValue ('password', 'password', $ereg_password, 'ACC11', 'ACC41')) // I test hashsec
				{
					if (paramValue ('password_confirm', 'password_confirm', $ereg_password, 'ACC12', 'ACC42')) // I test hashsec
					{
						if ($password == $password_confirm)
						{
								// Ok I delete this account
							$tQueryC = 'UPDATE `'.$ENV.'_NWDAccount` SET `DM` = \''.$TIME_SYNC.'\', `DD` = \''.$TIME_SYNC.'\', `AC` = \'0\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($uuid).'\' AND `Password` = \''.$SQL_CON->real_escape_string($password).'\';';
							$tResultC = $SQL_CON->query($tQueryC);
							if (!$tResultC)
							{
								error('SGN19');
							}
							else
							{
								AccountAnonymousNeeded(false);
								respondAdd('usertransfert', false);
								respondAdd('delete', true);
							}
						}
					}
				}
			}
		}
	}
		//--------------------
		// Ok I create a permanent account if temporary
	AccountAnonymeGenerate();
		//--------------------
	if ($ban == true)
	{
		error('ACC99');
	}
		//--------------------
	?>
