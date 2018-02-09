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
    global $NWD_LOG;
    $NWD_LOG=true;
    myLogLineReturn();
    myLogLineReturn();
    myLog('script start', __FILE__, __FUNCTION__, __LINE__);
    
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
    $ereg_PinCode = '/^([0-9]{6,18})$/';
        //--------------------
    function PinCodeRandom ()
    {
    return rand (100000000 ,999999999);
    }
        //--------------------
    if (!errorDetected())
    {
    
    errorDeclaration('RLS99', 'generic error relation ship');
    
    myLog('no error at start', __FILE__, __FUNCTION__, __LINE__);
    errorDeclaration('RLSw01', 'action empty');
    errorDeclaration('RLSw11', 'action ereg');
    if (paramValue ('action', 'action', $ereg_action, 'RLSw01', 'RLSw11')) // test if action is valid
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
            /*
             $tQuery = 'SELECT `Reference` FROM `'.$ENV.'_NWDRelationship` WHERE `PinLimit`<'.time().' AND `RelationState` <4 AND `MasterReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\'';
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
             $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `XX` = \''.time().'\', `DM` = \''.time().'\', `PinCode` = \'\', `RelationState` = 6 WHERE `Reference` LIKE \''.$SQL_CON->real_escape_string($tReference).'\' and `RelationState` > 0';
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
                // create a new pincode for this relationship
            errorDeclaration('RLSw02', 'Reference empty');
            errorDeclaration('RLSw12', 'Reference ereg');
            if (paramValue ('reference', 'reference', $ereg_reference, 'RLSw02', 'RLSw12')) // I test Reference
                {
                $tTested = false;
                $tPinCode = PinCodeRandom();
                myLog('test $tPinCode = '.$tPinCode, __FILE__, __FUNCTION__, __LINE__);
                while ($tTested == false)
                    {
                    $tTimeMax = time()-3600;
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
                                // Ok I have a good PinCode I update
                            $tTimeDm =$dico['NWDRelationship']['sync']+10;
                            $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `DM` = `DM`+1, `PinCode` = \''.$tPinCode.'\', `RelationState` = 2 WHERE `Reference` = \''.$SQL_CON->real_escape_string($reference).'\' AND `MasterReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `RelationState` = 1';
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
                            $tPinCode = PinCodeRandom();
                            }
                        }
                    }
                }
            }
        if ($action == 'Waiting')
            {
                // just return the update relationship object's
            }
        if ($action == 'RefuseFriend')
            {
                // just return the update relationship object's
            }
        if ($action == 'AcceptFriend')
            {
                // just return the update relationship object's
            }
        if ($action == 'EnterPinCode')
            {
            myLog('EnterPinCode', __FILE__, __FUNCTION__, __LINE__);
            errorDeclaration('RLSw80', 'PinCode empty');
            errorDeclaration('RLSw81', 'PinCode ereg');
            if (paramValue ('pincode', 'pincode', $ereg_PinCode, 'RLSw80', 'RLSw81')) // I test PinCode
                {
                $tTime = time();
                $tQuery = 'SELECT `Reference` FROM `'.$ENV.'_NWDRelationship` WHERE `PinCode` LIKE \''.$pincode.'\' AND `RelationState` = 2';// AND `PinLimit` > '.$tTime.'';
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
                            $tReference = $tRow['Reference'];
                            $tTimePlus = time();
                            $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDRelationship` SET `PreprodSync` = '.$tTimePlus.', `SlaveReference` = \''.$SQL_CON->real_escape_string($uuid).'\', `RelationState` = 3 WHERE `Reference` = \''.$SQL_CON->real_escape_string($tReference).'\'';
                            myLog('$tQueryUpdate', __FILE__, __FUNCTION__, __LINE__);
                            myLog($tQueryUpdate, __FILE__, __FUNCTION__, __LINE__);
                            $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                            if (!$tResultUpdate)
                                {
                                myLog('pincode NOT update', __FILE__, __FUNCTION__, __LINE__);
                                myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                                errorDeclaration('RLSw99', 'error in updtae reference object with uuid and pincode');
                                error('RLSw99');
                                }
                            else
                                {
                                myLog('pincode is OK i registered the slave user', __FILE__, __FUNCTION__, __LINE__);
                                IntegrityNWDRelationshipReevalue ($tReference);
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
        if (isset($dico['NWDRelationship']['sync']))
            {
            if (!errorDetected())
                {
                GetDatasNWDRelationship ($dico['NWDRelationship']['sync'], $uuid, $tPage, $tLimit);
                }
            }
        
        
        
        
        
        /*
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
         *//*
            }
            //--------------------
            //--------------------
            //--------------------
            //--------------------
            */
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
