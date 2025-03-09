<?php
header("Access-Control-Allow-Origin: *"); 
header("Access-Control-Allow-Methods: GET, POST, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type, Authorization");

header("Content-Type: application/json");
require "db.php";

$id = $_POST['id'];
$username = $_POST['username'];
$streak = $_POST['streak'];
$money = $_POST['money'];
$health = $_POST['health'];
$levelOpened = $_POST['levelOpened'];
$last_login = $_POST['last_login'];
$lastStreakUpdate = $_POST['lastStreakUpdate'];

$query = $pdo->prepare("UPDATE users SET username = ?, streak = ?, money = ?, health = ?, levelOpened = ?, last_login = ?, lastStreakUpdate = ? WHERE id = ?");
$query->execute([$username, $streak, $money, $health, $levelOpened, $last_login, $lastStreakUpdate, $id]);

echo json_encode(["success" => true]);
?>
