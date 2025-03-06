<?php
$host = "lelyim7e.beget.tech"; // или другой адрес сервера MySQL
$dbname = "lelyim7e_nixzord";
$user = "lelyim7e_nixzord";
$pass = "141722A!a";

$conn = new mysqli($host, $user, $pass, $dbname);
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT * FROM levels";
$result = $conn->query($sql);

$levels = [];
while ($row = $result->fetch_assoc()) {
    $levels[] = $row;
}

header('Content-Type: application/json');
echo json_encode($levels);

$conn->close();
?>
