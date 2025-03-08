<?php
header("Access-Control-Allow-Origin: *"); 
header("Access-Control-Allow-Methods: GET, POST, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type, Authorization");

header("Content-Type: application/json");
require "db.php";

$id = $_GET['id'];
$query = $pdo->prepare("SELECT username, regdate, streak, money, health, levelOpened FROM users WHERE id = ?");
$query->execute([$id]);
$data = $query->fetch(PDO::FETCH_ASSOC);

if ($data) {
    echo json_encode(["success" => true, "data" => $data]);
} else {
    echo json_encode(["success" => false, "error" => "User not found"]);
}
?>
