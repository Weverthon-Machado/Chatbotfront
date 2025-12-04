Pacote ML-2025 Frontend + Log
Arquivos gerados:

Pages/Index.cshtml -> Interface de Chat (Razor)
Pages/Index.cshtml.cs -> PageModel que lida com previsões e registro (OnPost, OnPostFeedback)
Models/ChatLogRecord.cs -> Modelo de registro de bate-papo
Models/ChatMessage.cs -> Modelo de mensagem (opcional)
Models/PredictionEngineHolder.cs -> Holder para injeção de dependência do PredictionEngine (se você o registrar)
Data/logs.json -> Armazena todas as solicitações/respostas (inicialmente vazio)
Data/useful.json -> Armazena apenas os registros marcados como úteis pelo usuário (inicialmente vazio)
Data/pergunta.csv -> Exemplo de arquivo CSV de entrada (você deve substituir este arquivo pelo seu arquivo real)
expand_dataset.py -> Script em Python para expandir o arquivo pergunta.csv para N linhas (padrão: 3000)
README.md -> Este arquivo
Instruções:

Copie os arquivos 'Pages' e 'Models' para o seu projeto Razor Pages.
Certifique-se de que a pasta Data exista na raiz do projeto. O código grava os arquivos logs.json e useful.json nessa pasta.
Se você já configurou o ML.NET PredictionEngine como um singleton/serviço, registre-o e passe-o para o PredictionEngineHolder (ou ajuste o código do IndexModel).
Para expandir seu conjunto de dados, execute: python expand_dataset.py 4000(para gerar 4000 linhas) a partir da raiz do projeto.
O frontend envia POST /Index com JSON { inputText: '...' } e espera JSON { id, botText, prediction, probability }.
Os botões de feedback chamam o método POST /Index?handler=Feedback com o JSON { id, useful } para marcar registros como úteis.
Notas:

O código de registro lê/grava arrays JSON completos (abordagem simples). Para grande escala, considere logs somente de acréscimo ou um banco de dados.
Substitua o arquivo de exemplo Data/pergunta.csv pelo seu arquivo real antes de executar o script expand_dataset.py.
