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
	$ereg_bilateral = '/^(yes|no)$/';
	$ereg_action = '/^(CreatePinCode|EnterPinCode|Waiting|AcceptFriend|RefuseFriend|Sync|SyncForce)$/';
	$ereg_email = '/^([A-Z0-9a-z\.\_\%\+\-]+@[A-Z0-9a-z\.\_\%\+\-]+\.[A-Za-z]{2,6})$/';
	$ereg_password = '/^(.{24,64})$/';
	$ereg_emailHash = '/^(.{24,64})$/';
	$ereg_auuidHash = '/^([A-Za-z0-9\-]{15,48})$/';
    $ereg_reference = '/^([A-Za-z0-9\-]{15,48})$/';
	$ereg_apasswordHash = '/^(.{12,64})$/';
    $ereg_PinCode = '/^([0-9\]{6,18})$/';
        //--------------------
    function PinCodeRandom ()
    {
        return rand ( 100000000 , 999999999 );
    }
    //--------------------
	if (!errorDetected())
	{
		if (paramValue ('action', 'action', $ereg_action, 'ACC01', 'ACC02')) // test if action is valid
		{
            
            $tPage = 0;
            if (isset($dico['page'])) { $tPage = $dico['page'];};
            $tLimit = 100000;
            if (isset($dico['limit'])) { $tLimit = $dico['limit'];};
            $tDate = time()-36000000; // check just one hour by default
            $tDate = 0; // check just one hour by default
            if (isset($dico['date'])) { $tDate = $dico['date'];};
            
            
            //--------------------
            include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/NWDRelationship/synchronization.php');
            //--------------------
            if (isset($dico['NWDRelationship']))
            {
                if (isset($dico['NWDRelationship']['data']))
                {
                    foreach ($dico['NWDRelationship']['data'] as $sCsvValue)
                    {
                        if (!errorDetected())
                        {
                            UpdateDataNWDRelationship ($sCsvValue, $tDate, $uuid, false);
                        }
                    }
                }
            }
            
            //--------------------
            
            
            
            
            //--------------------
            //--------------------
            //--------------------
			if ($action == 'CreatePinCode')
			{
                // delete old passed relationship
                global $SQL_CON;
                
                $tQuery = 'SELECT `Reference` FROM `'.$ENV.'_NWDRelationship` WHERE `PinLimit`<'.time().' AND `RelationState` <3 AND `MasterReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\'';
                $tResult = $SQL_CON->query($tQuery);
                if (!$tResult)
                {
                    error('FRDx33');            }
                else
                {
                    while($tRow = $tResult->fetch_array())
                    {
                        $tReference = $tRow['Reference']
                        $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `XX` = \''.time().'\', `DM` = \''.time().'\', `PinCode` = \'\', `RelationState` = 4 WHERE `Reference` LIKE \''.$SQL_CON->real_escape_string($tReference).'\'';
                        $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                        if (!$tResultUpdate)
                            {
                               error('RLS09');
                            }
                        IntegrityNWDRelationshipReevalue ($tReference);
                    }
                    mysqli_free_result($tResult);
                }
                
                // create a new pincode for this relationship
                if (paramValue ('Reference', 'Reference', $ereg_reference, 'RLS10', 'RLS40')) // I test emailrescue
                {
                    $tTested = false;
                    $tPinCode = PinCodeRandom();
                    while ($tTested == false)
                    {
                        $tQuery = 'SELECT `PinCode` FROM `'.$ENV.'_NWDRelationship` WHERE `PinCode` LIKE \''.$tPinCode.'\' PinLimit>'.time()-3600.' AND `RelationState` <3';
                        $tResult = $SQL_CON->query($tQuery);
                        if (!$tResult)
                        {
                            error('RLS10');
                        }
                        else
                        {
                            if ($tResult->num_rows == 0)
                            {
                                $tTested = true;
                                // Ok I have a good PinCode I update
                                $tUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `DM` = \''.time().'\', `PinCode` = \''.$tPinCode.'\', `PinLimit` = \''.(time()+180).'\', `RelationState` = 2, WHERE `Reference` = \''.$SQL_CON->real_escape_string($tReference).'\ AND `MasterReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `PinCode` = 1';
                                // I can add another test to certified the unique pincode here!
                                // TODO :
                                // Ok I reevaluate the integrity test
                                IntegrityNWDRelationshipReevalue ($Reference);
                            }
                            else
                            {
                                $tPinCode = PinCodeRandom();
                            }
                        }
                    }
                    // Ok I have a good PinCode I update
                    IntegrityNWDRelationshipReevalue ($Reference);
                }
			}
			if ($action == 'EnterPinCode')
			{
                if (paramValue ('PinCode', 'PinCode', $ereg_PinCode, 'RLS11', 'RLS41')) // I test PinCode
                {
                    
                }
			}
			if ($action == 'Waiting')
			{
				// just return the update relationship object's
			}
			if ($action == 'AcceptFriend')
			{
                if (paramValue ('Reference', 'Reference', $ereg_reference, 'RLS10', 'RLS40')) // I test reference
                {
                    if (paramValue ('bilateral', 'bilateral', $ereg_bilateral, 'RLS20', 'RLS30')) // I test bilateral
                    {
                        // valide only MasterSlave
                        
                        // Is bilateral souscription?
                        if (Bilateral == 'yes')
                        {
                            // create SlaveMaster too
                            
                        }
                    }
                }
                
			}
            if ($action == 'RefuseFriend')
            {
                if (paramValue ('Reference', 'Reference', $ereg_reference, 'RLS10', 'RLS40')) // I test reference
                {
                    $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `XX` = \''.time().'\', `DM` = \''.time().'\', `PinCode` = \'\', `RelationStatere` = 4 WHERE `Reference` LIKE \''.$SQL_CON->real_escape_string($tReference).'\' AND `MasterReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `PinCode` = 2';
                            $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                            if (!$tResultUpdate)
                            {
                                error('RLS09');
                            }
                            IntegrityNWDRelationshipReevalue ($tReference);
                        }
                        mysqli_free_result($tResult);
                    }
                }
            }
            //--------------------
            //--------------------
            //--------------------
            //--------------------
            
            
            
            //--------------------
            if (isset($dico['NWDRelationship']['sync']))
            {
                if (!errorDetected())
                {
                    GetDatasNWDRelationship ($dico['NWDRelationship']['sync'], $uuid, $sPage, $sLimit);
                }
            }
            
            //--------------------
            
            
            //--------------------
            //--------------------
            //--------------------
            //--------------------
    
            $tTimeStamp = $dico['NWDRelationship']['sync'];
    
            if ($action == 'SyncForce')
            {
                $tTimeStamp = 0;
                $action = 'Sync';
            }
    
            if ($action == 'Sync')
            {
                // if first sync I change the NWDRelationship from the SlaveRefeerence to 1
                // else I use standard timestamp of Relationsship sync
                
                // pour chaque classes autoeisée :
                /*
                 $ClassesShared  = split(',',ClassesSharedByMaster);
                 $ClassesAccepted  = split(',',ClassesAcceptedBySlave);
                 $Classes = array_intersect($ClassesShared, $ClassesAccepted);
                foreach ($Classes as $ClassName)
                 {
                    include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/'.$ClassName.'/synchronization.php');
                    if ($isfirst==true)
                        {
                            $tTimeStamp = 0;
                        }
                 $tFunction = 'GetDatas'.$ClassName;
                 $tFunction($tTimeStamp, $SlaveReference, 0, 10000000);
                 }
                 */
            }
            //--------------------
            //--------------------
            //--------------------
            //--------------------
            
		}
	}
		//--------------------
	if ($ban == true)
	{
		error('ACC99');
	}
		//--------------------
	?>
