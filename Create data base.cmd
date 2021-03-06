:: отключить вывод команд
@echo off

@echo Create role and data base
"C:\Program Files\PostgreSQL\13\bin\psql" -U postgres -h localhost -p 5432 -d postgres -c "create role masterServer" -c "alter role masterServer with password 'masterServer'" -c "alter role masterServer with login" -c "create database masterServer owner masterServer"
pause