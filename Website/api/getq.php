<?php

$id = isset($_GET['id']) ? $_GET['id'] : 'No id';

//firebase
// create curl resource 
$ch = curl_init(); 

// set url 
curl_setopt($ch, CURLOPT_URL, "https://play4matc.firebaseio.com/.json"); 

//return the transfer as a string 
curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1); 

// $output contains the output string 
$output = curl_exec($ch); 

// close curl resource to free up system resources 
curl_close($ch);      

$dataArray = json_decode($output, true);
		
//kijken welke vragen de user al gedaan heeft

$questionsDone = [];

if(isset($dataArray['Users'][$id]))
{
	for($i = 0; $i < count($dataArray['Users'][$id]['Answered']); $i++)
	{
		array_push($questionsDone, $dataArray['Users'][$id]['Answered'][$i]);
	}
}

// selecteer 5 vragen @random die de user nog niet gedaan heeft
$newQuestions = [];

$questionCount = 0;

for($i = 0; $i < count($dataArray['Questions']); $i++)
{
	if(!isset($questionsDone[$i]))
	{
		array_push($newQuestions, $dataArray['Questions'][$i]);
		array_push($questionsDone, $rand);
		
		$questionCount++;
	}
	
	if($questionCount == 5)
	{
		break;
	}
}

//return deze als json string

echo(json_encode($newQuestions));

//echo('{"Response": "Hello world!", "id": "'.$id.'"}');

?>