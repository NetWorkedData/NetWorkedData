<?php
	//Copyright NWD 2017
	//Created by Jean-François CONTART
	//--------------------
	// FINISH
	//--------------------
	// prevent include from function for exit (typical example: error('XXX', true);)
	global $NWD_LOG, $NWD_SLT_TMP, $SQL_CON, $NWD_TMA, $RRR_LOG, $REP, $WSBUILD, $TIME_SYNC, $REF_NEEDED, $ACC_NEEDED, $ENV, $NWD_SHA_VEC, $NWD_SHA_SEC, $NWD_SLT_STR, $NWD_SLT_END;
	//--------------------
	// add log
	if ($NWD_LOG==true)
	{
        myLogLineReturn();
        myLogLineReturn();
        myLogLineReturn();
    	respondAdd('log',$RRR_LOG);
		respondAdd('Addon Ref',$REF_NEEDED);
		respondAdd('Addon Acc',$ACC_NEEDED);
		respondAdd('environment',$ENV);
	}
	//--------------------
	// web-services build
	respondAdd('wsbuild',$WSBUILD);
	//--------------------
	//disconnect mysql
	mysqli_close($SQL_CON);
	//--------------------
	// Insert error if necessary
	errorResult();
	//--------------------
	// server benchmark
	respondAdd('perform',microtime(true)-$NWD_TMA);
	respondAdd('performRequest',microtime(true)-$_SERVER['REQUEST_TIME_FLOAT']);
	//--------------------
	//transform respond in JSON file
	//--------------------
	$temporalSalt = saltTemporal($NWD_SLT_TMP, 0);
	if (isset($REP['token']))
	{
		header('hash: '.sha1($temporalSalt.$NWD_SHA_VEC.$REP['token']));
		header('token: '.$REP['token']);
	}
	//--------------------
	$json = json_encode($REP);
	//--------------------
	if (respondIsset('securePost'))
	{
		header('scr: scrdgt');
		$REPSCR['scr'] =  aes128Encrypt( $json, $NWD_SHA_SEC, $NWD_SHA_VEC);
		$REPSCR['scrdgt'] = sha1($NWD_SLT_STR.$REPSCR['scr'].$NWD_SLT_END);
		$json = json_encode($REPSCR);
	}
	//--------------------
	// write JSON
	echo($json);
?>