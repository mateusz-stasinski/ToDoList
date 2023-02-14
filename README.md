# ToDoList

Ay uruchomić aplikację należy:
1. W katalogu ...ToDoList\ToDoListApi, w pliku appsettings.json skonfigurować połączenie do istniejącej bazy danych w SQL Server - "ConnectionString": "Twój connection string".
2. W tym samym pliku skonfigurować ustawienia klienta poczty email - "EmailConfiguration" (Jeśli mają się wysyłać powiadomienia).
3. Uruchomić aplikację ToDoListApi.
4. Jeśli nie masz zainstalowanego Angular CLI - Uruchomić konsolę i wykonać polecenie: npm install -g @angular/cli
5. Uruchomić konsolę w katalogu "...\ToDoList\ClientApp\ToDoList" i wykonać polecenie: npm install
6. Następnie wykonać polecenie: ng sevre --open

Powinna uruchmić się okno przeglądarki pod adresem http://localhost:4200/