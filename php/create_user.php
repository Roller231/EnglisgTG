<?php
header("Access-Control-Allow-Origin: *"); 
header("Access-Control-Allow-Methods: GET, POST, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type, Authorization");

header("Content-Type: application/json");
require_once 'db.php';

if (!isset($_POST["id"])) {
    echo json_encode(["success" => false, "error" => "Missing user ID"]);
    exit;
}

if (!isset($_POST["username"])) {
    echo json_encode(["success" => false, "error" => "Missing user name"]);
    exit;
}

$id = $_POST["id"];
$username = $_POST["username"]; // Здесь можно заменить на то значение, которое вам нужно

try {
    $stmt = $pdo->prepare("INSERT INTO users (id, username, regdate, streak, money, health, levelOpened) 
                           VALUES (:id, :username, NOW(), 0, 0, 3, 1)");
    $stmt->bindParam(":id", $id);
    $stmt->bindParam(":username", $username);
    $stmt->execute();
    
    echo json_encode(["success" => true, "message" => "Пользователь создан"]);
} catch (PDOException $e) {
    echo json_encode(["success" => false, "error" => $e->getMessage()]);
}
?>
