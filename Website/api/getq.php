<?php

$id = isset($_GET['id']) ? $_GET['id'] : 'No id';

//firebase

//kijken welke vragen de user al gedaan heeft

// selecteer 5 vragen @random die de user nog niet gedaan heeft

//return deze als json string

echo('{"Response": "Hello world!", "id": "'.$id.'"}');

?>