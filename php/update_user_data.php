<?php
require "db.php";

$id = $_POST['id'];
$username = $_POST['username'];
$streak = $_POST['streak'];
$money = $_POST['money'];
$health = $_POST['health'];
$levelOpened = $_POST['levelOpened'];

$query = $pdo->prepare("UPDATE users SET username = ?, streak = ?, money = ?, health = ?, levelOpened = ? WHERE id = ?");
$query->execute([$username, $streak, $money, $health, $levelOpened, $id]);

echo json_encode(["success" => true]);
?>
