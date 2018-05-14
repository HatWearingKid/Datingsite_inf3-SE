<?php

$id = isset($_GET['id']) : $_GET['id'] ? 'No id';

return "{'Response': {'Hello world!'} 'id': {'".$id."'}}";


?>