<?php
$json = file_get_contents("https://play4matc.firebaseio.com/Questions.json");
$json_array = json_decode($json);
$count = count($json_array);

$new = array();
$range = range(0, ($count-1));
shuffle($range);

foreach($range as $number){
  $new[] = $json_array[$number];
}

echo json_encode($new);