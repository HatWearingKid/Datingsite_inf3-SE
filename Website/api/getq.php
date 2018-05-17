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
		// Select 5 new questions for user
		$newQuestions = [];

		$questionCount = 0;

		for($i = 0; $i < count($dataArray['Questions']); $i++)
		{
			// Check which questions the user has already dones
			if($dataArray['Users'][$id]['Answered'][$i] === NULL)
			{
				$newQuestions[$i] = $dataArray['Questions'][$i];
			
				$questionCount++;
			}
			
			if($questionCount == 5)
			{
				break;
			}
		}
		
		// Return questions
		echo(json_encode($newQuestions));
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