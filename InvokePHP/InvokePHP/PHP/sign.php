<?php

error_reporting(E_ALL);
ini_set('display_errors','on');

$source = $argv[1];

// Получаем приватный ключ для подписи запроса
$key_file = '\project1074.ppk';
$dir = dirname($_SERVER["SCRIPT_FILENAME"]);
$path = $dir.$key_file;
           
$fp = fopen($path, 'r');
$cert = fread($fp, 8192);
fclose($fp);
$private_key = openssl_pkey_get_private($cert);
           
openssl_sign($source, $sign, $private_key);
$sign = unpack('H*', $sign);
$sign = array_shift($sign);

echo $sign;

?>