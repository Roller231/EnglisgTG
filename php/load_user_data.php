<?php
header("Access-Control-Allow-Origin: *"); 
header("Access-Control-Allow-Methods: GET, POST, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type, Authorization");

header("Content-Type: application/json");
require "db.php";

// Проверяем, передан ли параметр id
if (!isset($_GET['id'])) {
    echo json_encode(["success" => false, "error" => "Missing user ID"]);
    exit;
}

$id = $_GET['id'];

try {
    // Подготавливаем SQL-запрос для получения данных пользователя
    $query = $pdo->prepare("SELECT username, regdate, streak, money, health, levelOpened, last_login, lastStreakUpdate FROM users WHERE id = ?");
    $query->execute([$id]);
    $data = $query->fetch(PDO::FETCH_ASSOC);

    if ($data) {
        // Если данные найдены, возвращаем их
        echo json_encode(["success" => true, "data" => $data]);
    } else {
        // Если пользователь не найден, возвращаем ошибку
        echo json_encode(["success" => false, "error" => "User not found"]);
    }
} catch (PDOException $e) {
    // Обработка ошибок базы данных
    echo json_encode(["success" => false, "error" => $e->getMessage()]);
}
?>