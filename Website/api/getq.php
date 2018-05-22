<?php

$id = isset($_GET['id']) ? $_GET['id'] : false;
$qAmount = isset($_GET['qamount']) ? $_GET['qamount'] : false;

if($id !== false && $id !== '' &&
	$qAmount !== false && $qAmount !== '' && $qAmount !== 0)
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
		// Select 5 new questions for user
		$newQuestions = [];

		$questionCount = 0;

		for($i = 0; $i < count($dataArray['Questions']); $i++)
		{
			// Check which questions the user has already dones
			if($dataArray['Users'][$id]['Answered'][$i] === NULL)
			{
				$newQuestions[$i] = $dataArray['Questions'][$i];
				$newQuestions[$i]['Id'] = $i;
			
				$questionCount++;
			}
			
			if($questionCount == $qAmount)
			{
				break;
			}
		}
		
		// Return questions
		echo(json_encode($newQuestions, JSON_FORCE_OBJECT));
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

?>