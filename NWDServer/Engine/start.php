<?php
	// Copyright NWD 2017
	// Created by Jean-François CONTART
	//--------------------
	// START
	//--------------------
	// use functions library
	include_once ('functions.php');
	//--------------------
	// connect MYSQL
    $SQL_CON = new mysqli($SQL_HOT,$SQL_USR,$SQL_PSW, $SQL_BSE);
    if ($SQL_CON->connect_errno)
	{
		error('SQL00');
		include_once ('finish.php');
		exit;
    }
	else
	{
		// analyze request
		include_once ('request.php');
	}
	//--------------------
	// continue script
?>
