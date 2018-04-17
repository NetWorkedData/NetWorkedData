<?PHP
    
    header ('Access-Control-Allow-Origin: *');
//    header ('Access-Control-Allow-Origin: itunes.apple.com');
//    header ('Access-Control-Allow-Origin: linkmaker.itunes.apple.com');
    
    $myAppStoreID = '343200656';  // test angry birds
   $myGooglePlayID = 'com.rovio.angrybirds'; // test angry birds
    
    function ip_info($ip = NULL, $purpose = "location", $deep_detect = TRUE) {
        $output = NULL;
        if (filter_var($ip, FILTER_VALIDATE_IP) === FALSE) {
            $ip = $_SERVER["REMOTE_ADDR"];
            if ($deep_detect) {
                if (filter_var(@$_SERVER['HTTP_X_FORWARDED_FOR'], FILTER_VALIDATE_IP))
                    $ip = $_SERVER['HTTP_X_FORWARDED_FOR'];
                if (filter_var(@$_SERVER['HTTP_CLIENT_IP'], FILTER_VALIDATE_IP))
                    $ip = $_SERVER['HTTP_CLIENT_IP'];
            }
        }
        $purpose    = str_replace(array("name", "\n", "\t", " ", "-", "_"), NULL, strtolower(trim($purpose)));
        $support    = array("country", "countrycode", "state", "region", "city", "location", "address");
        $continents = array(
                            "AF" => "Africa",
                            "AN" => "Antarctica",
                            "AS" => "Asia",
                            "EU" => "Europe",
                            "OC" => "Australia (Oceania)",
                            "NA" => "North America",
                            "SA" => "South America"
                            );
        if (filter_var($ip, FILTER_VALIDATE_IP) && in_array($purpose, $support)) {
            $ipdat = @json_decode(file_get_contents("http://www.geoplugin.net/json.gp?ip=" . $ip));
            if (@strlen(trim($ipdat->geoplugin_countryCode)) == 2) {
                switch ($purpose) {
                    case "location":
                        $output = array(
                                        "city"           => @$ipdat->geoplugin_city,
                                        "state"          => @$ipdat->geoplugin_regionName,
                                        "country"        => @$ipdat->geoplugin_countryName,
                                        "country_code"   => @$ipdat->geoplugin_countryCode,
                                        "continent"      => @$continents[strtoupper($ipdat->geoplugin_continentCode)],
                                        "continent_code" => @$ipdat->geoplugin_continentCode
                                        );
                        break;
                    case "address":
                        $address = array($ipdat->geoplugin_countryName);
                        if (@strlen($ipdat->geoplugin_regionName) >= 1)
                            $address[] = $ipdat->geoplugin_regionName;
                        if (@strlen($ipdat->geoplugin_city) >= 1)
                            $address[] = $ipdat->geoplugin_city;
                        $output = implode(", ", array_reverse($address));
                        break;
                    case "city":
                        $output = @$ipdat->geoplugin_city;
                        break;
                    case "state":
                        $output = @$ipdat->geoplugin_regionName;
                        break;
                    case "region":
                        $output = @$ipdat->geoplugin_regionName;
                        break;
                    case "country":
                        $output = @$ipdat->geoplugin_countryName;
                        break;
                    case "countrycode":
                        $output = @$ipdat->geoplugin_countryCode;
                        break;
                }
            }
        }
        return $output;
    }
    
    
    $mylanguage = $_SERVER['HTTP_ACCEPT_LANGUAGE'];
    
    
    if ( isset( $_SERVER['HTTP_USER_AGENT'] ) )
    {
        $iPod = stripos( $_SERVER['HTTP_USER_AGENT'], "iPod" );
        $iPhone = stripos( $_SERVER['HTTP_USER_AGENT'], "iPhone" );
        $iPad = stripos( $_SERVER['HTTP_USER_AGENT'], "iPad" );
        $Android = stripos( $_SERVER['HTTP_USER_AGENT'], "Android" );
        $webOS = stripos( $_SERVER['HTTP_USER_AGENT'], "webOS" );
        // detect os version
        if ( $iPod || $iPhone || $iPad ) {
            if ($myAppStoreID!='')
            {
            // redirect to ipa file download on app store
            //header('Location: https://geo.itunes.apple.com/fr/app/'.$myAppStoreName.'/id'.$myAppStoreID.'?mt=8');
            $myAppStoreIDMeta = "\n".'   <meta name="apple-itunes-app" content="app-id='.$myAppStoreID.'"/>';
            }
            //die();
        }
        else if ( $Android )
        {
            if ($myGooglePlayID!='')
            {
            // redirect to apk file download
           // header('Location: https://play.google.com/store/apps/details?id='.$myGooglePlayID.'');
            //die();
            }
        }
    }
    else
    {
        // add links to each version here for generic Browser support
    }
    
    ?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="fr">
<head>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <meta http-equiv="Content-Language" content="fr" /><?php echo($myAppStoreIDMeta);?>
    <link rel="stylesheet" href="./FlashMyApp.css" type="text/css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
</head>
<body>
    <h1>Choose your OS</h1>
    <div class="choose">
<?php
    if ($myAppStoreID!='')
    {
?>
        <div class="choose_cell">
            <h2>iOS</h2>
            <div class="image_link">
<a href="https://geo.itunes.apple.com/fr/app/id<?php echo($myAppStoreID);?>?mt=8">
<div id="imgAppleID"></div>
<div id="imgAppleBadge">
<img alt="Get it on AppStore" src="http://linkmaker.itunes.apple.com/images/badges/<?php echo($mylanguage);?>/badge_appstore-lrg.svg" />
</div></a>
            </div>
        </div>
    <?php
        }
    if ($myGooglePlayID!='')
        {
            ?>
        <div class="choose_cell">
            <h2>Android</h2>
            <div class="image_link">
<a href="https://play.google.com/store/apps/details?id=<?php echo($myGooglePlayID);?>">
<div id="imgAndroidID"></div>
<div id="imgAndroidBadge">
<img alt="Get it on Google Play" src="https://play.google.com/intl/<?php echo($mylanguage);?>/badges/images/apps/en-play-badge.png" />
</div></a>
            </div>
        </div><?php
    }
?></div>

<div class="cookies_info">Ce site utilise des cookies pour collecter des informations.</div>
<div class="langue_info"><?php echo($mylanguage);?></div>

</body>
<script>
var iTunesLink = "https://itunes.apple.com/lookup?id=<?php echo($myAppStoreID);?>";
$.getJSON(iTunesLink + '&callback=?', function(data) {
//         alert('iTunesLink ' + iTunesLink + ' data = ' + data);
    if (data["resultCount"])
          {
          //          alert('result ok');
          img = data["results"][0].artworkUrl100;
          $('#imgAppleID').html('<img class="rounded" src="' + img + '"/>');
    }
          else
          {
          //          alert('result KO');
    }
          });

var androidLink = "https://play.google.com/store/apps/details?id=<?php echo($myGooglePlayID);?>";

$.get( androidLink, function( data) {
      $( ".result" ).html( data );
      alert( "Load was performed." );
      });

$.ajax({ datatype:"xml", url :androidLink,}).done(function( data ) {
               alert('googleLink ' + androidLink + ' data = ' + data);
    $(data).find('div.details-info img.cover-image').each(function(){
    $('#imgAndroidID').append('<img class="rounded" src="https:' + $(this).attr('src') + '"/>');
    });
    });
</script>

<script>
(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
 (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
 m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
 })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

ga('create', 'UA-72327916-1', 'auto');
ga('send', 'pageview');

</script>
</html>