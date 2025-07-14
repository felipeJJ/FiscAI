## Estrutura de pastas do projeto:
project-root/
├── data/                     # Dados brutos e processados
│   ├── raw/                  # Dados brutos obtidos pelo scraping
│   ├── processed/            # Dados pré-processados prontos para uso no treinamento
│   └── dados_produtos.json   # Arquivo consolidado com os dados de produtos
│
├── notebooks/                # Jupyter Notebooks para exploração, análise e experimentos
│   ├── ScrapingCaroneData.ipynb
│   └── ExploratoryDataAnalysis.ipynb
│
├── scripts/                  # Scripts diversos
│   ├── scraping/             # Scripts para scraping de dados
│   │   ├── script1.py
│   │   └── script2.py
│   ├── training/             # Scripts para treinamento do modelo
│   │   ├── data_preprocessing.py
│   │   └── train_ner_model.py
│   └── utils/                # Funções auxiliares e utilitários comuns
│       ├── data_cleaning.py
│       └── config.py
│
├── model/                    # Arquivos relacionados ao modelo de NER
│   ├── spaCy_model/          # Pasta do modelo spaCy salvo após o treinamento
│   │   ├── meta.json
│   │   └── ...               # Arquivos do modelo spaCy treinado
│   ├── training_data/        # Conjunto de dados de treinamento para NER
│   │   └── training_data.json
│   ├── inference.py          # Script para carregar e usar o modelo
│   └── evaluation.py         # Script para avaliação do modelo
│
├── backend/                  # Backend da aplicação em ASP.NET Core
│   ├── Controllers/          # Controladores do ASP.NET Core
│   ├── Services/             # Lógica de negócios
│   ├── Models/               # Modelos de dados
│   ├── Program.cs            # Ponto de entrada do backend
│   ├── Startup.cs            # Configuração de serviços e middleware
│   ├── appsettings.json      # Configurações da aplicação
│   └── Dockerfile            # Dockerfile para o backend
│
├── frontend/                 # Frontend da aplicação em Next.js
│   ├── public/               # Arquivos estáticos (imagens, ícones)
│   ├── src/                  # Código-fonte do frontend
│   │   ├── components/       # Componentes reutilizáveis da interface
│   │   ├── pages/            # Páginas da aplicação
│   │   └── services/         # Lógica de consumo de APIs
│   ├── package.json          # Dependências do frontend
│   └── next.config.js        # Configuração do Next.js
│
├── tests/                    # Testes automatizados
│   ├── unit/                 # Testes unitários para cada módulo
│   ├── integration/          # Testes de integração
│   └── e2e/                  # Testes de ponta a ponta (frontend e backend)
│
├── .gitignore                # Arquivos e diretórios ignorados pelo Git
├── README.md                 # Documentação inicial do projeto
└── LICENSE                   # Licença do projeto
