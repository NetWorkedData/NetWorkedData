<?php
		//NWD Autogenerate File at 2017-05-16
		//Copyright NetWorkedDatas ideMobi 2017
		//Created by Jean-François CONTART
		//--------------------
		// WEBSERVICES FUNCTIONS
		//--------------------
		// Determine the file tree path
	$PATH_BASE = dirname(dirname(__DIR__));
		// include all necessary files
	include_once ($PATH_BASE.'/Environment/prod/Engine/constants.php');
		// start the generic process
	include_once ($PATH_BASE.'/Engine/start.php');
		// start the script
		//--------------------
    global $dico, $uuid;
    //--------------------
    // Ok I create a permanent account if temporary before
    AccountAnonymeGenerate();
    //--------------------
	include_once ($PATH_BASE.'/Engine/relationship.php');
		//--------------------
		// finish the generic process
	include_once ($PATH_BASE.'/Engine/finish.php');
		//--------------------
	?>
