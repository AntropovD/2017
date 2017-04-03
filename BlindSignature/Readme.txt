Задача: Реализовать протокол слепой подписи

Ос: Windows
Среда разработки: Visual Studio 2010
Запуск: BlindSignature\BlindSignature\bin\Release\BlindSignature.exe

В файле key.txt указать privateKey publicKey и P - простое число, модуль по которому производятся действия.

Пример работы: 

Enter message:
123
Your message numbers:
49 50 51
Alice sends Bob masked message
11907 12150 12393
Bob signs message and sends back to Alice
20717 34292 61114
Alice receives signed message and unmasks it
70525 75050 52181
