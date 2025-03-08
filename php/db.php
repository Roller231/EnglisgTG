<?php
$host = "lelyim7e.beget.tech";
$dbname = "lelyim7e_nixzord";
$user = "lelyim7e_nixzord";
$pass = "141722A!a";

try {
    $pdo = new PDO("mysql:host=$host;dbname=$dbname;charset=utf8", $user, $pass);
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
    die("Ошибка подключения к БД: " . $e->getMessage());
}
?>
