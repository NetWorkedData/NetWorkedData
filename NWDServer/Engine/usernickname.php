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
    $ereg_action = '/^(nickname|EnterPinCode|Waiting|AcceptFriend|RefuseFriend|Sync|SyncForce)$/';
    $ereg_email = '/^([A-Z0-9a-z\.\_\%\+\-]+@[A-Z0-9a-z\.\_\%\+\-]+\.[A-Za-z]{2,6})$/';
    $ereg_password = '/^(.{24,64})$/';
    $ereg_emailHash = '/^(.{24,64})$/';
    $ereg_auuidHash = '/^([A-Za-z0-9\-]{15,48})$/';
    $ereg_reference = '/^([A-Za-z0-9\-]{15,48})$/';
    $ereg_apasswordHash = '/^(.{12,64})$/';
    $ereg_nickname = '/^([.]{3,48})$/';
        //--------------------
    function PinCodeRandom ()
    {
        rand (100000 ,999999);
    }
        //--------------------
    if (!errorDetected())
    {
    
    errorDeclaration('USF99', 'generic error relation ship');
    
    myLog('no error at start', __FILE__, __FUNCTION__, __LINE__);
    errorDeclaration('USFw01', 'action empty');
    errorDeclaration('USFw11', 'action ereg');
    if (paramValue ('action', 'action', $ereg_action, 'USFw01', 'USFw11')) // test if action is valid
        {
        if (isset($dico['date'])) { $tDate = $dico['date'];};
        
            //--------------------
        include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/NWDUserInfos/synchronization.php');
            //--------------------
        if (isset($dico['NWDUserInfos']))
            {
            if (isset($dico['NWDUserInfos']['data']))
                {
                foreach ($dico['NWDUserInfos']['data'] as $sCsvValue)
                    {
                    if (!errorDetected())
                        {
                        UpdateDataNWDUserInfos ($sCsvValue, $dico['NWDUserInfos']['sync'], $uuid, false);
                        }
                    }
                }
            }
        
            //--------------------
            //--------------------
            //--------------------
        if ($action == 'nickname')
            {
                // delete old passed relationship
            global $SQL_CON;
                // create a new pincode for this relationship
            errorDeclaration('USFw02', 'Reference empty');
            errorDeclaration('USFw12', 'Reference ereg');
            if (paramValue ('reference', 'reference', $ereg_reference, 'USFw02', 'USFw12')) // I test Reference
                {
            errorDeclaration('USFw22', 'nickname empty');
            errorDeclaration('USFw22', 'nickname ereg');
            if (paramValue ('nickname', 'nickname', $ereg_nickname, 'USFw22', 'USFw22')) // I test nickname
                {
                $tTested = false;
                $tPinCode = PinCodeRandom();
                myLog('test $tPinCode = '.$tPinCode, __FILE__, __FUNCTION__, __LINE__);
                while ($tTested == false)
                    {
                    $tTimeMax = time();
                    $tQuery = 'SELECT `UniqueNickname` FROM `'.$ENV.'_NWDUserInfos` WHERE `UniqueNickname` LIKE \''.$SQL_CON->real_escape_string($nickname).'#'.$tPinCode.'\' WHERE `Reference` != \''.$SQL_CON->real_escape_string($reference).'\' ';
                    $tResult = $SQL_CON->query($tQuery);
                    if (!$tResult)
                        {
                        myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                        errorDeclaration('USFw92', 'error in select other UniqueNickname allready install');
                        error('USFw92');
                        }
                    else
                        {
                        if ($tResult->num_rows == 0)
                            {
                            myLog('UniqueNickname is not used '.$tPinCode, __FILE__, __FUNCTION__, __LINE__);
                            $tTested = true;
                                // Ok I have a good PinCode I update
                            $tTimeDm =$dico['NWDUserInfos']['sync']+1;
                            $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDUserInfos` SET `DM` = \''.$tTimeDm.'\', `PinCode` = \''.$tPinCode.'\', `PinLimit` = \''.$tTimeLimit.'\', `RelationState` = \'2\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($reference).'\' AND `MasterReference` LIKE \''.$SQL_CON->real_escape_string($uuid).'\' AND `RelationState` = 1';
                            myLog('$tQueryUpdate', __FILE__, __FUNCTION__, __LINE__);
                            myLog($tQueryUpdate, __FILE__, __FUNCTION__, __LINE__);
                            $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                            if (!$tResultUpdate)
                                {
                                myLog('pincode NOT update', __FILE__, __FUNCTION__, __LINE__);
                                myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                                errorDeclaration('USFw93', 'error in updtae reference object  pincode');
                                error('USFw93');
                                }
                            else
                                {
                                myLog('pincode is update', __FILE__, __FUNCTION__, __LINE__);
                                IntegrityNWDUserInfosReevalue ($reference);
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
            }
            //--------------------
            //--------------------
            //--------------------
        if (isset($dico['NWDUserInfos']['sync']))
            {
            if (!errorDetected())
                {
                GetDatasNWDUserInfos ($dico['NWDUserInfos']['sync'], $uuid,);
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
