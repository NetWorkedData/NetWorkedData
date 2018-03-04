<?php
		//Copyright NWD 2017
		//Created by Jean-François CONTART
		//--------------------
		// FINISH
		//--------------------
		// prevent include from function for exit (typical example: error('XXX', true);)
	global $NWD_LOG, $SQL_CON, $NWD_TMA, $RRR_LOG, $REP, $WSBUILD;
		//--------------------
		// add log
	if ($NWD_LOG==true)
	{
        myLogLineReturn();
        myLogLineReturn();
        myLogLineReturn();
    
		respondAdd('log',$RRR_LOG);
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
	$json = json_encode($REP);
		//--------------------
	header('Content-Length: '.strlen($json));
		//--------------------
		// write JSON
	echo($json);
	?>
