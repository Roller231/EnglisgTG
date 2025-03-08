<?php
header("Access-Control-Allow-Origin: *"); 
header("Access-Control-Allow-Methods: GET, POST, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type, Authorization");

require "db.php";

$id = $_GET['id'];
$query = $pdo->prepare("SELECT COUNT(*) FROM users WHERE id = ?");
$query->execute([$id]);
$exists = $query->fetchColumn() > 0;

echo json_encode(["exists" => $exists]);
?>
