<?php
        //NWD File at 2017-05-16
        //Copyright NetWorkedDatas ideMobi 2017
        //Created by Jean-François CONTART
        //--------------------
        // AUTHENTIFICATION FUNCTIONS
        //--------------------
        //   error_reporting(E_ALL);
        //   error_reporting(-1);
        //--------------------
    global $NWD_LOG, $TIME_SYNC;
    $NWD_LOG=true;
    myLogLineReturn();
    myLogLineReturn();
    myLog('script start', __FILE__, __FUNCTION__, __LINE__);
    
    $ereg_appname = '/^(.*)$/';
    $ereg_facebook = '/^(.*)$/';
    $ereg_google = '/^(.*)$/';
    $ereg_unity = '/^(.*)$/';
    $ereg_twitter = '/^(.*)$/';
    $ereg_bilateral = '/^(True|False)$/';
    $ereg_action = '/^(CreatePinCode|EnterPinCode|Waiting|AcceptFriend|RefuseFriend|BannedFriend|ChangeClassByPublisher|ChangeClassByReader|Sync|SyncForce|Clean)$/';
    $ereg_email = '/^([A-Z0-9a-z\.\_\%\+\-]+@[A-Z0-9a-z\.\_\%\+\-]+\.[A-Za-z]{2,6})$/';
    $ereg_password = '/^(.{24,64})$/';
    $ereg_emailHash = '/^(.{24,64})$/';
    $ereg_auuidHash = '/^([A-Za-z0-9\-]{15,48})$/';
    $ereg_reference = '/^([A-Za-z0-9\-]{15,96})$/';
    $ereg_apasswordHash = '/^(.{12,64})$/';
    $ereg_pincode = '/^([0-9]{4,12})$/';
    $ereg_pinsize = '/^([0-9]{1,2})$/';
    $ereg_pindelay = '/^([0-9]{1,2})$/';
    $ereg_nickname = '/^(.{3,48})$/';
    $ereg_nicknameID = '/^(.{3,48})$/';
    $ereg_classes = '/^(.*)$/';
        //--------------------
    function PinCodeRandom (int $sSize)
    {
    if ($sSize>12)
        {
        $sSize = 12;
        }
    if ($sSize<4)
        {
        $sSize = 4;
        }
    $tPin = rand (1000 ,9999);
    while ($sSize>4)
        {
        $sSize--;
        $tPin = $tPin . rand (0,9);
        }
    return $tPin;
        //return rand (100000000 ,999999999);
    }
        //--------------------
    // function HasRowValue ($sRow)
    // {
    //     Global $NWD_SLT_SRV;
    // return md5($sRow['PublisherReference'].$NWD_SLT_SRV.$sRow['ReaderReference'].$sRow['RelationState'].$sRow['PublisherClassesShared'].$sRow['ReaderClassesAccepted']);
    // }
        //--------------------
    // function HashTest ($sRow)
    // {
    // $rReturn = false;
    // if ( HasRowValue ($sRow) == $sRow['ServerHash'])
    //     {
    //     $rReturn = true;
    //     }
    // return $rReturn;
    // }


//-------------------- 
// function ServerHashReevalue ($sReference)
// {
//     global $SQL_CON;
//     global $SQL_NWDRelationship_SaltA, $SQL_NWDRelationship_SaltB;
//     $tQuery = 'SELECT * FROM `'.$ENV.'_NWDRelationship` WHERE `Reference` = \''.$SQL_CON->real_escape_string($sReference).'\';';
//     $tResult = $SQL_CON->query($tQuery);
//     if (!$tResult)
//         {
//             error('RLSx31');
//             myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);
//         }
//     else
//         {
//             if ($tResult->num_rows == 1)
//                 {
//                     // I calculate the integrity and reinject the good value
//                     $tUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `ServerHash` = \''.$SQL_CON->real_escape_string($tCalculate).'\', `ProdSync` = \''.$TIME_SYNC.'\'  WHERE `Reference` = \''.$SQL_CON->real_escape_string($sReference).'\';';
//                     $tUpdateResult = $SQL_CON->query($tUpdate);
//                     if (!$tUpdateResult)
//                         {
//                             error('RLSx91');
//                             myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);
//                         }
//                 }
//         }
// }
//-------------------- 


        //--------------------
    if (!errorDetected())
    {
    errorDeclaration('RLS99', 'generic error relation ship');
    myLog('no error at start', __FILE__, __FUNCTION__, __LINE__);
    errorDeclaration('RLSw01', 'action empty');
    errorDeclaration('RLSw11', 'action ereg');
    if (paramValue ('action', 'action', $ereg_action, 'RLSw01', 'RLSw11')) // test if action is valid
        {
            //--------------------
        // include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/NWDRelationship/synchronization.php');
        //     //--------------------
        // if (isset($dico['NWDRelationship']))
        //     {
        //     if (isset($dico['NWDRelationship']['data']))
        //         {
        //         foreach ($dico['NWDRelationship']['data'] as $sCsvValue)
        //             {
        //             if (!errorDetected())
        //                 {
        //                 UpdateDataNWDRelationship ($sCsvValue, $dico['NWDRelationship']['sync'], $uuid, false);
        //                 }
        //             }
        //         }
        //     }
        
            //--------------------
        
        
        
        
            //--------------------
            //--------------------
            //--------------------
        if ($action == 'CreatePinCode')
            {
                $action = 'Clean';
                // delete old passed relationship
            global $SQL_CON;
                // create a new pincode for this relationship
            errorDeclaration('RLSw02', 'Reference empty');
            errorDeclaration('RLSw12', 'Reference ereg');
            if (paramValue ('reference', 'reference', $ereg_reference, 'RLSw02', 'RLSw12')) // I test Reference
                {
                errorDeclaration('RLSw22', 'pincodeLength empty');
                errorDeclaration('RLSw22', 'pincodeLength ereg');
                if (paramValue ('pinsize', 'pinsize', $ereg_pinsize, 'RLSw22', 'RLSw22')) // I test pincodeLength
                    {
                    errorDeclaration('RLSw23', 'pincodeLength empty');
                    errorDeclaration('RLSw24', 'pincodeLength ereg');
                    if (paramValue ('pindelay', 'pindelay', $ereg_pindelay, 'RLSw23', 'RLSw24')) // I test pincodeLength
                        {
                            errorDeclaration('RLSw25', 'nickname empty');
                            errorDeclaration('RLSw26', 'nickname ereg');
                            if (paramValue ('nickname', 'nickname', $ereg_nickname, 'RLSw25', 'RLSw26')) // I test pincodeLength
                                {
                        $tTested = false;
                        $tPinCode = PinCodeRandom($pinsize);
                        myLog('test $tPinCode = '.$tPinCode, __FILE__, __FUNCTION__, __LINE__);
                        while ($tTested == false)
                            {
                            $tTimeMax = $TIME_SYNC-10;
                            $tQuery = 'SELECT `PinCode` FROM `'.$ENV.'_NWDRelationship` WHERE `PinCode` LIKE \''.$tPinCode.'\' AND `PinLimit` > '.$tTimeMax.' AND `RelationState` <3';
                            $tResult = $SQL_CON->query($tQuery);
                            if (!$tResult)
                                {
                                myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                                errorDeclaration('RLSw92', 'error in select other pincode allready install');
                                error('RLSw92');
                                }
                            else
                                {
                                if ($tResult->num_rows == 0)
                                    {
                                    myLog('pincode is not used '.$tPinCode, __FILE__, __FUNCTION__, __LINE__);
                                    $tTested = true;
                                    $tTimeLimit = $TIME_SYNC + $pindelay + 10; // I add 10 seconds of marge
                                                                           // Ok I have a good PinCode I updat
                                    $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `'.$ENVSYNC.'` = \''.$TIME_SYNC.'\', `PublisherNickname` = \''.$SQL_CON->real_escape_string($nickname).'\', `PinCode` = \''.$tPinCode.'\', `PinLimit` = \''.$tTimeLimit.'\', `ServerHash`= \'\', `RelationState` = \'2\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($reference).'\' AND `PublisherReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `RelationState` = 1';
                                    myLog('$tQueryUpdate', __FILE__, __FUNCTION__, __LINE__);
                                    myLog($tQueryUpdate, __FILE__, __FUNCTION__, __LINE__);
                                    $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                                    if (!$tResultUpdate)
                                        {
                                        myLog('pincode NOT update', __FILE__, __FUNCTION__, __LINE__);
                                        myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                                        errorDeclaration('RLSw93', 'error in updtae reference object  pincode');
                                        error('RLSw93');
                                        }
                                    else
                                        {
                                        myLog('pincode is update', __FILE__, __FUNCTION__, __LINE__);
                                            // I can add another test to certified the unique pincode here!
                                            // TODO :
                                            // Ok I reevaluate the integrity test
                                        IntegrityNWDRelationshipReevalue ($reference);
                                        }
                                    }
                                else
                                    {
                                    $tPinCode = PinCodeRandom($pinsize);
                                    }
                                }
                            }
                        }
                    }
                    }
                }
                //$action = 'Sync';
            }
        if ($action == 'Waiting')
            {
                // just return the update relationship object's
            // errorDeclaration('RLSw02', 'Reference empty');
            // errorDeclaration('RLSw12', 'Reference ereg');
            // if (paramValue ('reference', 'reference', $ereg_reference, 'RLSw02', 'RLSw12')) // I test Reference
            //     {
            //     }
            //$action = 'Sync';
            }
        if ($action == 'RefuseFriend')
            {
                // just return the update relationship object's
            errorDeclaration('RLSw02', 'Reference empty');
            errorDeclaration('RLSw12', 'Reference ereg');
            if (paramValue ('reference', 'reference', $ereg_reference, 'RLSw02', 'RLSw12')) // I test Reference
                {
                    $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `'.$ENVSYNC.'` = \''.$TIME_SYNC.'\', `ServerHash`= \'\', `RelationState` = \'5\', `AC` = \'0\', `XX` = \''.$TIME_SYNC.'\ , `DD` = \''.$TIME_SYNC.'\ ';
                    $tQueryUpdate.= 'WHERE `Reference` = \''.$SQL_CON->real_escape_string($reference).'\' AND `PublisherReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' ';//AND `RelationState` = 3';
                    myLog('$tQueryUpdate', __FILE__, __FUNCTION__, __LINE__);
                    myLog($tQueryUpdate, __FILE__, __FUNCTION__, __LINE__);
                    $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                    if (!$tResultUpdate)
                        {
                        errorDeclaration('RLSw93', 'error in updtae reference object  pincode');
                        error('RLSw93');
                        }
                    else
                        {
                        IntegrityNWDRelationshipReevalue ($reference);
                        }
                }
                //$action = 'Sync';
            }
        if ($action == 'AcceptFriend')
            {
                // just return the update relationship object's
            errorDeclaration('RLSw02', 'Reference empty');
            errorDeclaration('RLSw12', 'Reference ereg');
            if (paramValue ('reference', 'reference', $ereg_reference, 'RLSw02', 'RLSw12')) // I test Reference
                {
                    // $rValue = isset($dico['bilateral']) ? $dico['bilateral'] : '';// in place of  $dico[$key];
                    // myLog('bilateral='.$rValue, __FILE__, __FUNCTION__, __LINE__);

                    errorDeclaration('RLSw72', 'bilateral empty');
                    errorDeclaration('RLSw73', 'bilateral ereg');
                    if (paramValue ('bilateral', 'bilateral', $ereg_bilateral, 'RLSw72', 'RLSw73')) // I test bilateral
                        {
                            $tReferenceBilateral ='';
                            if ($bilateral=='True')
                            {
                                // TODO : I create a bilateral data

                                $tQuerySelect = 'SELECT * FROM `'.$ENV.'_NWDRelationship` WHERE `Reference` = \''.$SQL_CON->real_escape_string($reference).'\' AND `PublisherReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `RelationState` = 3';
                                $tResultSelect = $SQL_CON->query($tQuerySelect);

                                if (!$tResultSelect)
                                {
                                myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                                }
                            else
                                {
                                if ($tResultSelect->num_rows == 1)
                                    {
                                    while($tRow = $tResultSelect->fetch_array())
                                        {

                                $tReferenceBilateral = referenceGenerate ('RLS', $ENV.'_NWDRelationship', 'Reference');
                                 myLog('tReferenceBilateral='.$tReferenceBilateral, __FILE__, __FUNCTION__, __LINE__);
                                // I will insert reciprocity in database 
                                //$tQuery = 'SELECT `Reference`, `DM`, `DS`, `DevSync`, `PreprodSync`, `ProdSync`,
                                //  `AC`, `DC`, `DD`, `FirstSync`, `InternalDescription`, `InternalKey`, `MinVersion`, `PinCode`, `PinLimit`,
                                //   `Preview`, `PublisherClassesShared`, `PublisherNickname`, `PublisherReference`, `ReaderClassesAccepted`, `ReaderNickname`, `ReaderReference`,
                                //    `Reciprocity`, `RelationState`, `Tag`, `WebServiceVersion`, `XX`, `Integrity` FROM `'.$ENV.'_NWDRelationship` WHERE Reference IN ( \''.implode('\', \'', $sReferences).'\') AND `WebServiceVersion` <= '.$WSBUILD.';';
		
                                $tInsert = 'INSERT INTO `'.$ENV.'_NWDRelationship` (';
                                $tInsert.= '`Reference`, ';
                                $tInsert.= '`DM`, ';
                                $tInsert.= '`DS`, ';
                                $tInsert.= '`DevSync`, ';
                                $tInsert.= '`PreprodSync`, ';
                                $tInsert.= '`ProdSync`, ';
                                $tInsert.= '`AC`, ';
                                $tInsert.= '`ReaderClassesAccepted`,';
                                $tInsert.= '`PublisherClassesShared`,';
                                $tInsert.= '`DC`,';
                                $tInsert.= '`DD`,  ';
                                $tInsert.= '`FirstSync`, ';
                                $tInsert.= '`InternalDescription`, ';
                                $tInsert.= '`InternalKey`, ';
                                $tInsert.= '`PublisherNickname`, ';
                                $tInsert.= '`PublisherReference`,';
                                $tInsert.= '`MinVersion`, ';
                                $tInsert.= '`PinCode`, ';
                                $tInsert.= '`PinLimit`, ';
                                $tInsert.= '`Preview`, ';
                                $tInsert.= '`Reciprocity`,';
                                $tInsert.= '`ReferenceVersionned`, ';
                                $tInsert.= '`RelationState`, ';
                                $tInsert.= '`ReaderNickname`, ';
                                $tInsert.= '`ReaderReference`, ';
                                $tInsert.= '`WebServiceVersion`, ';
                                $tInsert.= '`Tag`, ';
                                $tInsert.= '`XX`, ';
                                $tInsert.= '`Integrity`) ';
                                $tInsert.= ' VALUES (';
                                $tInsert.= '\''.$SQL_CON->real_escape_string($tReferenceBilateral).'\',';
                                $tInsert.= ' \''.$TIME_SYNC.'\',';
                                $tInsert.= ' \''.$TIME_SYNC.'\',';
                                if ($ENVSYNC=='DevSync')
                                {
                                $tInsert.= ' \''.$TIME_SYNC.'\',';
                                $tInsert.= ' \'-1\',';
                                $tInsert.= ' \'-1\',';
                                }
                                else if ($ENVSYNC=='PreprodSync')
                                {
                                $tInsert.= ' \'-1\',';
                                $tInsert.= ' \''.$TIME_SYNC.'\',';
                                $tInsert.= ' \'-1\',';
                                }
                                else if ($ENVSYNC=='ProdSync')
                                {
                                $tInsert.= ' \'-1\',';
                                $tInsert.= ' \'-1\',';
                                $tInsert.= ' \''.$TIME_SYNC.'\',';
                                }
                                $tInsert.= ' \'1\', ';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['PublisherClassesShared']).'\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['ReaderClassesAccepted']).'\', ';
                                $tInsert.= ' \''.$TIME_SYNC.'\',';
                                $tInsert.= ' \'0\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['FirstSync']).'\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['InternalDescription']).'\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['InternalKey']).'\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['ReaderNickname']).'\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['ReaderReference']).'\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['MinVersion']).'\',';
                                $tInsert.= ' \'\',';
                                $tInsert.= ' \'\',';
                                $tInsert.= ' \'\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['Reference']).'\',';
                                $tInsert.= ' \'\',';
                                $tInsert.= ' \'4\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['PublisherNickname']).'\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['PublisherReference']).'\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['WebServiceVersion']).'\',';
                                $tInsert.= ' \''.$SQL_CON->real_escape_string($tRow['Tag']).'\',';
                                $tInsert.= ' \'0\',';
                                $tInsert.= ' \'\'';
                                $tInsert.= ');';
										$tInsertResult = $SQL_CON->query($tInsert);
										if (!$tInsertResult)
											{
												error('RLSx32');
												myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tInsertResult.'', __FILE__, __FUNCTION__, __LINE__);
                                            }
                                            IntegrityNWDRelationshipReevalue ($tReferenceBilateral);
                                        }
                                    }
                                }
                            }
                    $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `'.$ENVSYNC.'` = \''.$TIME_SYNC.'\', `ServerHash`= \'\', `RelationState` = \'4\', `Reciprocity` = \''.$SQL_CON->real_escape_string($tReferenceBilateral).'\' ';
                    $tQueryUpdate.= 'WHERE `Reference` = \''.$SQL_CON->real_escape_string($reference).'\' AND `PublisherReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `RelationState` = 3';
                    myLog('$tQueryUpdate', __FILE__, __FUNCTION__, __LINE__);
                    myLog($tQueryUpdate, __FILE__, __FUNCTION__, __LINE__);
                    $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                    if (!$tResultUpdate)
                        {
                        errorDeclaration('RLSw93', 'error in updtae reference object  pincode');
                        error('RLSw93');
                        }
                    else
                        {
                            IntegrityNWDRelationshipReevalue ($reference);
                            // IntegrityServerNWDRelationshipValidate ($reference);
                        }

                    }
                }
                //$action = 'Sync';
            }
        if ($action == 'BannedFriend')
            {
                // just return the update relationship object's
            errorDeclaration('RLSw02', 'Reference empty');
            errorDeclaration('RLSw12', 'Reference ereg');
            if (paramValue ('reference', 'reference', $ereg_reference, 'RLSw02', 'RLSw12')) // I test Reference
                {
                    $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `'.$ENVSYNC.'` = \''.$TIME_SYNC.'\', `ServerHash`= \'\', `RelationState` = \'98\' ';
                    $tQueryUpdate.= 'WHERE `Reference` = \''.$SQL_CON->real_escape_string($reference).'\' AND `PublisherReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `RelationState` = 3';
                    myLog('$tQueryUpdate', __FILE__, __FUNCTION__, __LINE__);
                    myLog($tQueryUpdate, __FILE__, __FUNCTION__, __LINE__);
                    $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                    if (!$tResultUpdate)
                        {
                        errorDeclaration('RLSw93', 'error in updtae reference object  pincode');
                        error('RLSw93');
                        }
                    else
                        {
                        IntegrityNWDRelationshipReevalue ($reference);
                    }
                }
                //$action = 'Sync';
            }
        if ($action == 'ChangeClassByPublisher')
            {
                // just return the update relationship object's
            errorDeclaration('RLSw02', 'Reference empty');
            errorDeclaration('RLSw12', 'Reference ereg');
            if (paramValue ('reference', 'reference', $ereg_reference, 'RLSw02', 'RLSw12')) // I test Reference
                {
                    errorDeclaration('RLSw40', 'Classes empty');
                    errorDeclaration('RLSw41', 'Classes ereg');
                    if (paramValue ('classes', 'classes', $ereg_classes, 'RLSw40', 'RLSw41')) // I test Classes
                    {

                        $tQuery = 'SELECT * FROM `'.$ENV.'_NWDRelationship` WHERE  `Reference` LIKE \''.$SQL_CON->real_escape_string($reference).'\' AND `PublisherReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `RelationState` = 4 AND AC = \'1\' AND DD = \'0\'';
                        $tResult = $SQL_CON->query($tQuery);
                        if (!$tResult)
                            {
                            myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                            errorDeclaration('RLSw101', 'error in select relationship');
                            error('RLSw101');
                            }
                        else
                            {
                            while($tRow = $tResult->fetch_array())
                                {
                                    IntegrityServerNWDRelationshipValidate($tRow['Reference']);
                                if (IntegrityServerNWDRelationshipValidateByRow ($tRow) == true)
                                    {
                                            $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `PublisherClassesShared` = \''.$SQL_CON->real_escape_string($classes).'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($tRow['Reference']).'\'';
                                            $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                                            if (!$tResultUpdate)
                                                {
                                                errorDeclaration('RLSw981', 'error in update reference object');
                                                error('RLSw981');
                                                }
                                            else
                                                {
                                                IntegrityNWDRelationshipReevalue ($tRow['Reference']);
                                                // IntegrityServerNWDRelationshipValidate ($tRow['Reference']);
                                                }
                                    }
                                }
                            }
                    }
                }
            }
        if ($action == 'ChangeClassByReader')
            {
                // just return the update relationship object's
            errorDeclaration('RLSw02', 'Reference empty');
            errorDeclaration('RLSw12', 'Reference ereg');
            if (paramValue ('reference', 'reference', $ereg_reference, 'RLSw02', 'RLSw12')) // I test Reference
                {
                    errorDeclaration('RLSw40', 'Classes empty');
                    errorDeclaration('RLSw41', 'Classes ereg');
                    if (paramValue ('classes', 'classes', $ereg_classes, 'RLSw40', 'RLSw41')) // I test Classes
                    {
                        $tQuery = 'SELECT * FROM `'.$ENV.'_NWDRelationship` WHERE  `Reference` LIKE \''.$SQL_CON->real_escape_string($reference).'\' AND `ReaderReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `RelationState` = 4 AND AC = \'1\' AND DD = \'0\'';
                        $tResult = $SQL_CON->query($tQuery);
                        if (!$tResult)
                            {
                            myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                            errorDeclaration('RLSw101', 'error in select relationship');
                            error('RLSw101');
                            }
                        else
                            {
                            while($tRow = $tResult->fetch_array())
                                {
                                    IntegrityServerNWDRelationshipValidate($tRow['Reference']);
                                if (IntegrityServerNWDRelationshipValidateByRow ($tRow) == true)
                                    {
                                            $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `ReaderClassesShared` = \''.$SQL_CON->real_escape_string($classes).'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($tRow['Reference']).'\'';
                                            $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                                            if (!$tResultUpdate)
                                                {
                                                errorDeclaration('RLSw981', 'error in update reference object');
                                                error('RLSw981');
                                                }
                                            else
                                                {
                                                IntegrityNWDRelationshipReevalue ($tRow['Reference']);
                                                // IntegrityServerNWDRelationshipValidate ($tRow['Reference']);
                                                }
                                    }
                                }
                            }
                    }
                }
                //$action = 'Sync';
            }
        if ($action == 'EnterPinCode')
            {
            myLog('EnterPinCode', __FILE__, __FUNCTION__, __LINE__);
            errorDeclaration('RLSw80', 'PinCode empty');
            errorDeclaration('RLSw81', 'PinCode ereg');
            if (paramValue ('pincode', 'pincode', $ereg_pincode, 'RLSw80', 'RLSw81')) // I test PinCode
                {

                    errorDeclaration('RLSw25', 'nickname empty');
                    errorDeclaration('RLSw26', 'nickname ereg');
                    if (paramValue ('nickname', 'nickname', $ereg_nickname, 'RLSw25', 'RLSw26')) // I test pincodeLength
                        {

                $tTime = $TIME_SYNC;
                $tQuery = 'SELECT `Reference`, `PublisherReference` FROM `'.$ENV.'_NWDRelationship` WHERE `PinCode` LIKE \''.$pincode.'\' AND `RelationState` = 2';// AND `PinLimit` > '.$tTime.'';
                $tResult = $SQL_CON->query($tQuery);
                if (!$tResult)
                    {
                    myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                    errorDeclaration('RLSw98', 'error in select pincode allready install');
                    error('RLSw98');
                    }
                else
                    {
                    if ($tResult->num_rows == 1)
                        {
                        while($tRow = $tResult->fetch_array())
                            {
                            $tPublisherReference = $tRow['PublisherReference'];
                            $tReference = $tRow['Reference'];
                                // recherche si une relation existe déjà
                            $tQueryAllready = 'SELECT `Reference` FROM `'.$ENV.'_NWDRelationship` WHERE `PublisherReference` LIKE \''.$tPublisherReference.'\' AND `ReaderReference` = \''.$SQL_CON->real_escape_string($uuid).'\' AND `RelationState` = 4 AND `AC` = 1 AND `DD` = 0 ';// AND `PinLimit` > '.$tTime.'';
                            $tResultAllready = $SQL_CON->query($tQueryAllready);
                            if ($tResultAllready->num_rows > 0)
                                {
                                $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `'.$ENVSYNC.'` = '.$TIME_SYNC.', `ReaderReference` = \''.$SQL_CON->real_escape_string($uuid).'\', `RelationState` = 7 WHERE `Reference` = \''.$SQL_CON->real_escape_string($tReference).'\'';
                                myLog('$tQueryUpdate', __FILE__, __FUNCTION__, __LINE__);
                                myLog($tQueryUpdate, __FILE__, __FUNCTION__, __LINE__);
                                $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                                if (!$tResultUpdate)
                                    {
                                    errorDeclaration('RLSw99', 'error in update reference object with uuid and pincode');
                                    error('RLSw99');
                                    }
                                else
                                    {
                                    IntegrityNWDRelationshipReevalue ($tReference);
                                    }
                                errorDeclaration('RLSw33', 'Allready relationship exists');
                                error('RLSw33', false);
                                }
                            else
                                {
                                $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `'.$ENVSYNC.'` = '.$TIME_SYNC.', `ReaderNickname` = \''.$SQL_CON->real_escape_string($nickname).'\', `ReaderReference` = \''.$SQL_CON->real_escape_string($uuid).'\', `RelationState` = 3 WHERE `Reference` = \''.$SQL_CON->real_escape_string($tReference).'\'';
                                myLog('$tQueryUpdate', __FILE__, __FUNCTION__, __LINE__);
                                myLog($tQueryUpdate, __FILE__, __FUNCTION__, __LINE__);
                                $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                                if (!$tResultUpdate)
                                    {
                                    myLog('pincode NOT update', __FILE__, __FUNCTION__, __LINE__);
                                    myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                                    errorDeclaration('RLSw99', 'error in update reference object with uuid and pincode');
                                    error('RLSw99');
                                    }
                                else
                                    {
                                    myLog('pincode is OK i registered the slave user', __FILE__, __FUNCTION__, __LINE__);
                                    IntegrityNWDRelationshipReevalue ($tReference);
                                    }
                                }
                            }
                        }
                    else
                        {
                        myLog('NO RESULT', __FILE__, __FUNCTION__, __LINE__);
                        myLog('$tQuery', __FILE__, __FUNCTION__, __LINE__);
                        myLog($tQuery, __FILE__, __FUNCTION__, __LINE__);
                        }
                    }
                }
            }
            }
        


        if ($action == 'Clean')
            {
                /*
                 $tQuery = 'SELECT `Reference` FROM `'.$ENV.'_NWDRelationship` WHERE `PinLimit`<'.$TIME_SYNC
                 .' AND `RelationState` <4 AND `PublisherReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\'';
                 $tResult = $SQL_CON->query($tQuery);
                 if (!$tResult)
                 {
                 errorDeclaration('RLSw90', 'error in deleted  old pincode allready installed');
                 error('RLSw90');
                 }
                 else
                 {
                 while($tRow = $tResult->fetch_array())
                 {
                 $tReference = $tRow['Reference'];
                 $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `XX` = \''.$TIME_SYNC.'\', `DD` = \''.$TIME_SYNC.'\', `DM` = \''.$TIME_SYNC.'\', `PinCode` = \'\', `RelationState` = 6 WHERE `Reference` LIKE \''.$SQL_CON->real_escape_string($tReference).'\' and `RelationState` > 0';
                 $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                 if (!$tResultUpdate)
                 {
                 myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                 errorDeclaration('RLSw91', 'error in update old pincode');
                 error('RLSw91');
                 }
                 IntegrityNWDRelationshipReevalue ($tReference);
                 }
                 mysqli_free_result($tResult);
                 }
                 */
            //$action = 'Sync';
            }



        $tForce = false;
        if ($action == 'SyncForce')
            {
            $action = 'Sync';
            $tForce = true;
            }

        // Anyway I Force the sync!

        if ($action == 'Sync')
            {
                // just return the update relationship object's
            $tQuery = 'SELECT * FROM `'.$ENV.'_NWDRelationship` WHERE  `ReaderReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `RelationState` = 4 AND AC = \'1\' AND DD = \'0\'';
            $tResult = $SQL_CON->query($tQuery);
            if (!$tResult)
                {
                myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                errorDeclaration('RLSw101', 'error in select relationship');
                error('RLSw101');
                }
            else
                {
                while($tRow = $tResult->fetch_array())
                    {
                        IntegrityServerNWDRelationshipValidate($tRow['Reference']);
                    if (IntegrityServerNWDRelationshipValidateByRow ($tRow) == true)
                        {

                            // todo check if account is valid


                            // force if first sync
                            if ($tRow['FirstSync']==1)
                            {
                                $tForce = true;
                                $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `FirstSync` = 0 WHERE `Reference` = \''.$SQL_CON->real_escape_string($tRow['Reference']).'\'';
                                $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                                if (!$tResultUpdate)
                                    {
                                    errorDeclaration('RLSw981', 'error in update reference object');
                                    error('RLSw981');
                                    }
                                else
                                    {
                                    IntegrityNWDRelationshipReevalue ($tRow['Reference']);
                                    // IntegrityServerNWDRelationshipValidate ($tRow['Reference']);
                                    }
                            }

                        myLog('Must add object from '.$tRow ['PublisherClassesShared']. ' from '.$tRow ['PublisherReference'], __FILE__, __FUNCTION__, __LINE__);
                        myLog('Must filter object by '.$tRow ['ReaderClassesAccepted']. ' from '.$tRow ['PublisherReference'], __FILE__, __FUNCTION__, __LINE__);
                        $tArrayClassesMaster = explode(',',$tRow ['PublisherClassesShared']);
                        $tArrayClassesSlave = explode(',',$tRow ['ReaderClassesAccepted']);
                        $tArrayClasse = array_intersect($tArrayClassesMaster, $tArrayClassesSlave );
                        foreach ($tArrayClasse as $sClass)
                        {
                            if ($sClass!='')
                            {
                            include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/'.$sClass.'/synchronization.php');
                            $tFunction = 'GetDatas'.$sClass;
                            if ($tForce==true)
                                { 
                                    myLog('get FORCE ALL objects '.$sClass.' from '.$tRow ['PublisherReference'], __FILE__, __FUNCTION__, __LINE__);
                                    $tFunction(0, $tRow ['PublisherReference']);
                                }
                            else
                                {
                                    myLog('get objects '.$sClass.' from '.$tRow ['PublisherReference'], __FILE__, __FUNCTION__, __LINE__);
                                    $tFunction($dico['NWDRelationship']['sync'], $tRow ['PublisherReference']);
                                }
                            }
                        }
                        }
                    else
                        {
                        myLog('ERRRO FOR RLS  '.$tRow ['Reference'], __FILE__, __FUNCTION__, __LINE__);
                        errorDeclaration('RLSw999', 'error in sync security relationship');
                        error('RLSw999');
                        }
                    }
                }
            }
        if (isset($dico['NWDRelationship']['sync']))
            {
            if (!errorDetected())
                {
                GetDatasNWDRelationship ($dico['NWDRelationship']['sync'], $uuid);
                }
            }
        }
    }
    else
    {
    myLog('error detected', __FILE__, __FUNCTION__, __LINE__);
    }
        //--------------------
    if ($ban == true)
    {
    error('ACC99');
    }
    myLogLineReturn();
    myLogLineReturn();
        //--------------------
    ?>
 