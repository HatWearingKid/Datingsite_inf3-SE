<?php

$id = isset($_GET['id']) ? $_GET['id'] : false;

if($id !== false && $id !== '')
{
	//firebase
	$ch = curl_init(); 
	curl_setopt($ch, CURLOPT_URL, "https://play4matc.firebaseio.com/.json"); 
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1); 
	$output = curl_exec($ch); 
	curl_close($ch);      

	$dataArray = json_decode($output, true);

	if(isset($dataArray['Users'][$id]))
	{

		
		$matches = [];
		
		// Return matches
		echo(json_encode($matches));
	}
	else
	{
		// Id not found
		echo(json_encode(false));
	}
}
else
{
	// Id not entered
	echo(json_encode(false));
}

function compareUsers($user1, $user2)
{
	
}

?>