# Desafio Técnico: Encontre um Professor

API .NET 8 + Frontend React

Solução desenvolvida para um desafio técnico, com o objetivo de localizar professores disponíveis em determinados horários, utilizando uma API REST e uma interface web para interação com o usuário.

## 🛠️ Tecnologias Utilizadas

- **Backend:** .NET 8, C#, ASP.NET Core Web API.
- **Testes:** xUnit para testes automatizados das regras de negócio.
- **Frontend:** React, Node.js e Vite.
- **Gerenciamento de dependências:** NuGet e NPM.

## 🚀 Como Executar o Projeto

### 1. Backend — API .NET 8

1. Abra a solução `DesafioProfessores.sln`, localizada na pasta `backend`, no Visual Studio.
2. Verifique se o arquivo `professores.json` está no caminho esperado pelo projeto: `backend/src/DesafioProfessores/Data/`.
3. Defina o projeto da API como **Projeto de Inicialização** (*Startup Project*).
4. Execute a aplicação pelo botão verde de inicialização do Visual Studio.
5. Se configurado, acesse a documentação Swagger pelo endereço exibido durante a execução da API.

### 2. Frontend — React

Abra um terminal na raiz do repositório e execute os seguintes comandos:

```bash
cd frontend
npm install
npm run dev
```

Após iniciar o servidor, acesse o endereço informado no terminal. Por padrão, o Vite utiliza:

`http://localhost:5173`

**Observação:** a API precisa estar em execução para que o frontend consiga consumir seus endpoints. Verifique também se a URL configurada no frontend corresponde ao endereço da API.

## 🧪 Executando os Testes Automatizados

Os testes automatizados utilizam o xUnit para verificar as principais regras de negócio da aplicação.

Para executá-los pelo Visual Studio:

1. Abra o **Gerenciador de Testes** (*Test Explorer*).
2. Aguarde a descoberta dos testes.
3. Clique em **Executar Todos** (*Run All*).

Alternativamente, na raiz do repositório, execute:

```bash
dotnet test
```

Se necessário, execute o comando dentro da pasta que contém a solução ou o projeto de testes.

## 🏛️ Decisões Técnicas

### Armazenamento dos dados

Os dados dos professores são obtidos a partir do arquivo `professores.json` e mantidos em memória durante a execução da aplicação, evitando a necessidade de um banco de dados para o escopo do desafio.

### Separação das responsabilidades

A lógica de negócio é concentrada na camada de serviços, especialmente no `BuscadorProfessoresService`, enquanto as controllers ficam responsáveis pelo tratamento das requisições HTTP e pelo retorno das respostas da API.

### Regras de negócio

A implementação contempla as regras relacionadas à disponibilidade dos professores, incluindo a validação de horários, a sobreposição e o encostamento de intervalos, os horários fixos e a composição da lista de professores não elegíveis, conforme os requisitos do desafio.

### Validação e tratamento de erros

A API realiza validações dos dados recebidos, como duração, formato dos horários e preenchimento dos campos. Quando aplicável, retorna o status HTTP `400 Bad Request` acompanhado de mensagens descritivas.

## 📌 Observações

- Certifique-se de que o arquivo `professores.json` esteja disponível no caminho esperado pela aplicação.
- Verifique a configuração de CORS e a URL da API caso o frontend não consiga realizar as requisições.
- Execute os testes automatizados antes de considerar as alterações concluídas.

---

Desenvolvido como parte de um desafio técnico de desenvolvimento de software.
