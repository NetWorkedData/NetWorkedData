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
    $ereg_nickname = '/^([A-Za-z0-9\-]{3,48})$/';
        //--------------------
    function CodeRandom (int $sSize)
    {
        $tMin = 10;
        while ($sSize>1)
        {
            $tMin = $tMin*10;
            $sSize--;
        }
        $tMax = ($tMin*10)-1;
        return rand ($tMin ,$tMax );
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
        include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/NWDUserNickname/synchronization.php');
            //--------------------
        if (isset($dico['NWDUserNickname']))
            {
            if (isset($dico['NWDUserNickname']['data']))
                {
                foreach ($dico['NWDUserNickname']['data'] as $sCsvValue)
                    {
                    if (!errorDetected())
                        {
                        UpdateDataNWDUserNickname ($sCsvValue, $dico['NWDUserNickname']['sync'], $uuid, false);
                        }
                    }
                }
            }
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
                errorDeclaration('USFw23', 'nickname ereg');
                if (paramValue ('nickname', 'nickname', $ereg_nickname, 'USFw22', 'USFw23')) // I test nickname
                    {
                        // I search if nickname is the same
                    $tQuery = 'SELECT `UniqueNickname`, `Nickname`, `Reference` FROM `'.$ENV.'_NWDUserNickname` WHERE `Reference` = \''.$SQL_CON->real_escape_string($reference).'\' AND `AccountReference` = \''.$SQL_CON->real_escape_string($uuid).'\'';
                    $tResult = $SQL_CON->query($tQuery);
                    if (!$tResult)
                        {
                        myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                        errorDeclaration('USFw92', 'error in select other UniqueNickname allready install');
                        error('USFw92');
                        }
                    else
                        {
                        if ($tResult->num_rows == 1)
                            {
                            while($tRow = $tResult->fetch_array())
                                {
                                    // if (isset($tRow['Nickname']) == false)
                                    // {
                                    //     $tRow['Nickname'] = '';
                                    // }
                                    myLog($tRow['Nickname'].' == '.$nickname, __FILE__, __FUNCTION__, __LINE__);
                                    $tNick = explode('#',$tRow['UniqueNickname'])[0];

                                    $tTested = false;
                                    $tSize = 2;

                                if ($tRow['Nickname'] == $nickname && $tNick == $nickname)
                                    {
                                        // Nothing to do ? perhaps ... I test
                                        $tQuery = 'SELECT `UniqueNickname` FROM `'.$ENV.'_NWDUserNickname` WHERE `UniqueNickname` LIKE \''.$SQL_CON->real_escape_string($tRow['UniqueNickname']).'\'';
                                        $tResult = $SQL_CON->query($tQuery);
                                        if (!$tResult)
                                            {
                                            myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                                            errorDeclaration('USFw92', 'error in select other UniqueNickname allready install');
                                            error('USFw92');
                                            }
                                        else
                                            {
                                            if ($tResult->num_rows == 1)
                                                {
                                                    $tTested = true;
                                                }
                                            }
                                    }



                                if ($tTested == false)
                                    {
                                        // I need change for an unique nickname
                                    while ($tTested == false)
                                        {
                                        $tPinCode = CodeRandom($tSize++);

                                        $tTimeMax = time();
                                        $tQuery = 'SELECT `UniqueNickname` FROM `'.$ENV.'_NWDUserNickname` WHERE `UniqueNickname` LIKE \''.$SQL_CON->real_escape_string($nickname).'#'.$tPinCode.'\' AND `AccountReference` != \''.$SQL_CON->real_escape_string($uuid).'\'';
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
                                                $tTested = true;
                                                    // Ok I have a good PinCode I update
                                                $tTimeDm =$dico['NWDUserNickname']['sync']+1;
                                                $tQueryUpdate = 'UPDATE `'.$ENV.'_NWDUserNickname` SET `DM` = \''.$tTimeDm.'\', `UniqueNickname` = \''.$SQL_CON->real_escape_string($nickname).'#'.$tPinCode.'\', `Nickname` = \''.$nickname.'\' WHERE `Reference` = \''.$SQL_CON->real_escape_string($reference).'\' AND `AccountReference` = \''.$SQL_CON->real_escape_string($uuid).'\'';
                                                myLog('$tQueryUpdate', __FILE__, __FUNCTION__, __LINE__);
                                                myLog($tQueryUpdate, __FILE__, __FUNCTION__, __LINE__);
                                                $tResultUpdate = $SQL_CON->query($tQueryUpdate);
                                                if (!$tResultUpdate)
                                                    {
                                                    myLog($SQL_CON->error, __FILE__, __FUNCTION__, __LINE__);
                                                    errorDeclaration('USFw93', 'error in updtae reference object pincode');
                                                    error('USFw93');
                                                    }
                                                else
                                                    {
                                                    myLog('pincode is update', __FILE__, __FUNCTION__, __LINE__);
                                                    IntegrityNWDUserNicknameReevalue ($reference);
                                                    }
                                                }
                                                else
                                                {
                                                    $tPinCode = CodeRandom($tSize++); 
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        else
                            {
                            errorDeclaration('USFw94', 'error in select multiple reference or no reference (!=1)');
                            error('USFw94');
                            }
                        }
                    $tTested = false;
                    }
                }
            }
            //--------------------
            //--------------------
            //--------------------
        if (isset($dico['NWDUserNickname']['sync']))
            {
            if (!errorDetected())
                {
                GetDatasNWDUserNickname ($dico['NWDUserNickname']['sync'], $uuid);
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
