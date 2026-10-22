# Desafio Técnico: Encontre um Professor

**Backend .NET 8 | Frontend React + TypeScript**

Solução desenvolvida para um desafio técnico com o objetivo de localizar professores disponíveis em determinados horários, por meio de uma API REST e de uma interface web para interação com o usuário.

---

## 🛠️ Tecnologias Utilizadas

- **Backend:** .NET 8, C# e ASP.NET Core Web API.
- **Testes automatizados:** xUnit.
- **Frontend:** React, TypeScript e Vite.
- **Gerenciamento de dependências:** NuGet e NPM.

---

## 🚀 Como Executar o Projeto

### 1. Backend — API .NET 8

1. Abra a solução `DesafioProfessores.sln`, localizada na pasta `backend`, no Visual Studio.
2. Verifique se o arquivo `professores.json` está disponível no caminho esperado pela aplicação.
3. Defina o projeto da API como **Projeto de Inicialização** (*Startup Project*).
4. Execute a aplicação pelo botão de inicialização do Visual Studio.
5. Acesse a documentação Swagger pelo endereço configurado para a API.

### 2. Frontend — React + TypeScript

1. Abra a pasta `frontend` em um editor de sua preferência.
2. Abra o terminal integrado e execute os comandos:

```bash
npm install
npm run dev
```

3. Acesse o endereço exibido no terminal, geralmente `http://localhost:5173`.

> **Observação:** mantenha a API em execução para que o frontend possa consumir seus endpoints. Verifique também se a URL base configurada no frontend corresponde ao endereço da API.

---

## 🧪 Testes Automatizados

A aplicação possui testes automatizados com **xUnit**, voltados à validação das principais regras de negócio da busca de professores.

Para executar os testes pelo Visual Studio:

1. Acesse **Testes → Gerenciador de Testes** (*Test Explorer*).
2. Aguarde a descoberta dos testes.
3. Clique em **Executar Todos** (*Run All*).

Os resultados permitem identificar comportamentos inesperados e verificar a implementação das regras definidas para o desafio.

---

## 🏛️ Decisões Técnicas

### Armazenamento dos dados

Os dados dos professores são carregados a partir do arquivo `professores.json` e mantidos em memória durante a execução da aplicação, atendendo ao escopo do desafio sem a necessidade de um banco de dados.

### Separação de responsabilidades

A lógica de negócio está concentrada na camada de serviços, especialmente no `BuscadorProfessoresService`, mantendo as controllers responsáveis pelo tratamento das requisições HTTP e pela composição das respostas.

### Regras de negócio

A implementação contempla as principais regras relacionadas à disponibilidade dos professores, incluindo validações de horários, sobreposições, encostamentos de intervalos e identificação de professores elegíveis e não elegíveis, conforme os requisitos do desafio.

### Organização do repositório

O projeto utiliza uma estrutura de **monorepo**, reunindo backend e frontend em um único repositório, mas mantendo seus arquivos, dependências e responsabilidades organizados em diretórios separados.

---

## 💡 Evoluções Futuras

A implementação prioriza o atendimento aos requisitos funcionais do desafio dentro do tempo e do escopo disponíveis. Algumas melhorias podem ampliar a manutenibilidade, a testabilidade e a escalabilidade da solução.

1. **Arquitetura em camadas:** evoluir a organização do backend para separar explicitamente responsabilidades de domínio, aplicação, infraestrutura e API, conforme a complexidade do projeto justificar.
2. **Domain-Driven Design (DDD):** avaliar a aplicação de conceitos de DDD caso as regras de negócio e o domínio evoluam, evitando complexidade arquitetural desnecessária para o escopo atual.
3. **Evolução do frontend:** ampliar a componentização do React, padronizar os estilos e avaliar estratégias de gerenciamento de estado conforme as necessidades da interface.
4. **Testes de interface:** adicionar testes automatizados para os componentes e fluxos principais, utilizando ferramentas como Vitest e React Testing Library.

---

## 📌 Considerações Finais

O desenvolvimento foi conduzido com foco em resolver as principais dores identificadas no problema e atender às regras de negócio e aos requisitos funcionais estabelecidos, respeitando o tempo disponível e o escopo proposto para o desafio.

As principais necessidades e regras previstas foram contempladas, priorizando o funcionamento da solução, a validação dos horários e a identificação correta dos professores elegíveis e não elegíveis.

Ao longo do desenvolvimento, também foram identificadas oportunidades de melhoria na organização arquitetural e na estrutura do frontend. Essas evoluções podem ser incorporadas progressivamente, incluindo uma separação mais explícita das camadas do backend, maior componentização da interface e ampliação da cobertura de testes.

A proposta foi entregar uma solução funcional, objetiva e coerente com os requisitos do desafio, sem perder de vista a importância da manutenibilidade, da testabilidade e da evolução contínua do software.

---

Desenvolvido como parte de um desafio técnico de desenvolvimento de software.