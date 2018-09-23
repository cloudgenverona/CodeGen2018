#!/bin/bash 
curl -X GET "https://api.telegram.org/bot460033867:AAGOos1Gke6axOFBQix2UKTPb-valaBsWhk/sendMessage?chat_id=-1001304444437&text=Backup%20Start!"

echo "Send Start"

mysqldump -u root -proot sportsdb_qa > backup.sql
sleep 20

curl -d '{"chat_id":-1001304444437, "text":"Backup Completed!", \
"reply_markup": \
{"inline_keyboard": \
[[{"text":"by #CodeGen 2018", "url": "https://codegen2018.azurewebsites.net/"}]]} }' \
-H "Content-Type: application/json" \
-X POST https://api.telegram.org/bot460033867:AAGOos1Gke6axOFBQix2UKTPb-valaBsWhk/sendMessage

echo "Send Completed"

echo "endScript"
