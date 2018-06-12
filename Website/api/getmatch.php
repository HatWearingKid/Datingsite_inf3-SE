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

		// Create users array excluding $user and any already liked users
		unset($dataArray['Users'][$id]);

		foreach($user['Liked'] as $likedId => $value)
		{
			unset($dataArray['Users'][$likedId]);
		}

		$users = $dataArray['Users'];

		// Filter based on user's preferences
		$users = filterUsersByUserPref($user, $users);

		// Filter based on the other users preferences
		$users = filterUsersByOthersPref($user, $users);

		// Compare $user answers given with the $matches answers
		$matches = compareAnswers($user, $users);

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

	foreach($users as $userId => $tempUser)
	{
		$userAge = getAge($tempUser['DateOfBirth']);

		if(	$userAge >= $user['Preferences']['AgeMin'] &&
			$userAge <= $user['Preferences']['AgeMax'] &&
			$tempUser['Gender'] == $user['Preferences']['Gender'])
		{
			// $tempUser meets specifications for $user, add to $result
			$result[$userId] = $tempUser;
		}
		else
		{
			// $tempUser Does not meet specifications for $user
		}
	}

	return $result;
}

function filterUsersByOthersPref($user, $users)
{
	$result = [];

	foreach($users as $userId => $tempUser)
	{
		$userAge = getAge($tempUser['DateOfBirth']);

		if(	$tempUser['Preferences']['AgeMin'] <= $userAge &&
			$tempUser['Preferences']['AgeMax'] >= $userAge &&
			$tempUser['Preferences']['Gender'] == $user['Gender'] )
		{
			// $user meets specifications for $tempUser, add to $result
			$result[$userId] = $tempUser;
		}
		else
		{
			// $user Does not meet specifications for $tempUser
		}
	}

	return $result;
}

function compareAnswers($user, $users)
{
	$result = false;

	foreach($users as $userId => $tempUser)
	{
		//Save $tempUser in $result
		$result[$userId] = $tempUser;
		$result[$userId]['Id'] = $userId;
		$result[$userId]['UsedAnswers'] = 0;
		$result[$userId]['points'] = 0;

		foreach($user['Answered'] as $questionIndex => $array)
		{
			// Only evaluate the answer if $user && $tempUser has it answered and it's not null
			if(	isset($user['Answered'][$questionIndex]) &&
				$user['Answered'][$questionIndex] !== null &&
				isset($tempUser['Answered'][$questionIndex]) &&
				$tempUser['Answered'][$questionIndex] !== null)
			{
				$result[$userId]['UsedAnswers']++;

				// The answer of $tempUser does not have to match $user
				if($array['Value'] == 0)
				{
					if($array['Answer'] == $tempUser['Answered'][$questionIndex]['Answer'])
					{
						$result[$userId]['points'] += 50;
					}
					else
					{
						$result[$userId]['points'] += 50;
					}
				}
				// The answer of $tempUser can or can't match
				elseif($array['Value'] == 1)
				{
					if($array['Answer'] == $tempUser['Answered'][$questionIndex]['Answer'])
					{
						$result[$userId]['points'] += 75;
					}
					else
					{
						$result[$userId]['points'] += 25;
					}
				}
				// The answer of $tempUser has to match $user
				elseif($array['Value'] == 2)
				{
					if($array['Answer'] == $tempUser['Answered'][$questionIndex]['Answer'])
					{
						$result[$userId]['points'] += 100;
					}
					else
					{
						$result[$userId]['points'] += 0;
					}
				}
			}
		}

		// Turn DateOfBirth into age
		$result[$userId]['Age'] = getAge($result[$userId]['DateOfBirth']);

		// Let's calculate the match percentage
		if($result[$userId]['points'] != 0 && $result[$userId]['UsedAnswers']!= 0)
		{
			$result[$userId]['MatchRate'] = round($result[$userId]['points'] / $result[$userId]['UsedAnswers']);
		}
		else
		{
			// Remove match from result
			unset($result[$userId]);
		}

		// Unset some unneeded data
		unset($result[$userId]['Answered']);
		unset($result[$userId]['Preferences']);
		unset($result[$userId]['DateOfBirth']);
		unset($result[$userId]['CompleteProfile']);
		unset($result[$userId]['Chatrooms']);
		unset($result[$userId]['Notifications']);
	}

	return $result;
}

function getAge($date)
{
	$tz  = new DateTimeZone('Europe/Brussels');
	$result = DateTime::createFromFormat('d/m/Y', $date, $tz)
			 ->diff(new DateTime('now', $tz))
			 ->y;

	return $result;
}

?>
