# WebProject
Progetto in asp.Net MVC

Ci sono tre pagine principali:

- Home
- Contact
- Table: La tabella è creata utilizzando kendo, visualizza un elenco di ordini permettendo di:
       - Aggiungere un ordine.
       - Eliminare un ordine.
       - Aggiornare un ordine.
       - esportare il contenuto su un foglio Excel o Pdf.
  I dati della tabella vengono salvati su un database.

Il tutto ha un sistema di autenticazione e registrazione.

- All'avvio del programma si viene indirizzati alla pagina di login, in caso l'utente fosse già autenticato si viene invece mandati 
  direttamente alla home.
- Se si provasse ad accedere a una qualunque pagina senza essere autenticati si viene reindirizzati invece alla pagina di login. 
- L'unica pagina alla quale è possibile accedere senza essere autenticati è la Home, (che comunque ha solo il link per la pagina di 
  login se non si è autenticati).
- Se viene creato un nuovo account, registrandosi, si viene reindirizzati alla pagina di autenticazione, e l'username viene impostato 
  in automatico.
- Sia nella pagina di autenticazione che nella pagina di registrazione vi è un sistema di validazione.
- Inoltre entrambe queste pagine sono gestite in modo che non si ricarichi la pagina se vi sono degli errori di inserimento dei dati, 
  (vengono semplicemente lanciate delle popup).
