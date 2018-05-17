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
		// Save user
		$user = $dataArray['Users'][$id];
		
		// Create users array excluding $user
		unset($dataArray['Users'][$id]);
		$users = $dataArray['Users'];
		
		// Filteren op basis van de gebruiker's voorkeuren
		$matches = filterUsersByUserPref($user, $users);
		
		// Verder filteren op basis van de andere gebruikers zijn/haar voorkeuren
		$matches = filterUsersByOthersPref($user, $matches);
		
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

function filterUsersByUserPref($user, $users)
{
	$result = [];
	
	foreach($users as $key => $tempUser)
	{
		if(	$tempUser['Age'] >= $user['Preferences']['Age_min'] &&
			$tempUser['Age'] <= $user['Preferences']['Age_max'] &&
			$tempUser['Gender'] == $user['Preferences']['Gender'])
		{
			// $tempUser meets specifications for $user, add to $result
			$result[$key] = $tempUser;
		}
		else
		{
			// $tempUser Does not meet specifications for $user
		}
	}
	
	return $result;
}

function filterUsersByOthersPref($user, $matches)
{
	$result = [];
	
	foreach($matches as $key => $tempUser)
	{

		if(	$tempUser['Preferences']['Age_min'] <= $user['Age'] &&
			$tempUser['Preferences']['Age_max'] >= $user['Age'] &&
			$tempUser['Preferences']['Gender'] == $user['Gender'] )
		{
			// $user meets specifications for $tempUser, add to $result
			$result[$key] = $tempUser;
		}
		else
		{
			// $user Does not meet specifications for $tempUser
		}
	}
	
	return $result;
}

function compareUsers($user1, $user2)
{
	
}

?>