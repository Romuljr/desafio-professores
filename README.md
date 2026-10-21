# Desafio Técnico: Encontre um Professor (API .NET 8)

Solução desenvolvida para o processo seletivo, implementando uma API em C# (.NET 8) para busca de horários de professores com arquitetura limpa e testes automatizados.

---

## 🛠️ Tecnologias Utilizadas
- **Backend:** .NET 8 (C#), ASP.NET Core Web API, Arquitetura baseada em Serviços e xUnit para testes unitários.

---

## 🚀 Como Rodar o Projeto

### 1. Backend (API .NET 8)
1. Abra a solução `backend/DesafioProfessores.sln` no **Visual Studio**.
2. Certifique-se de que o arquivo `professores.json` está localizado na pasta de dados do projeto (`backend/src/DesafioProfessores/Data/`).
3. Clique no botão verde de execução (Play / IIS Express ou perfil da API) na parte superior do Visual Studio para iniciar a API e abrir o Swagger.

---

## 🧪 Executando os Testes Unitários

O projeto possui testes automatizados em **xUnit** cobrindo as principais regras de negócio da busca.

1. No Visual Studio, abra o menu **Testes** > **Gerenciador de Testes** (Test Explorer).
2. Clique no botão **Executar Todos** (Run All) para rodar a suíte de testes.

---

## 🏛️ Decisões Tomadas (Conforme orientações do desafio)

- **Dados em Memória (Singleton):** Os professores são carregados do arquivo `professores.json` e mantidos em memória na inicialização, garantindo alta performance e simplicidade sem a necessidade de banco de dados.
- **Isolamento de Regras:** Toda a lógica pesada de validação de sobreposição, encostamento de horários, regras de horários fixos e montagem da lista de `naoElegiveis` foi isolada na camada de serviço (`BuscadorProfessoresService`), mantendo as Controllers limpas e totalmente testáveis.
- **Validações e Tratamento de Erros:** A API retorna `400 Bad Request` com mensagens descritivas caso o payload venha fora dos padrões exigidos (duração inválida, horários mal formatados, campos vazios, etc.).

---