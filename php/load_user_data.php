<?php
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
