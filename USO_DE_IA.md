# Uso de Inteligência Artificial
# Uso de Inteligência Artificial

## 🛠️ Ferramentas Utilizadas
- **Assistentes de IA (ChatGPT / Gemini):** Utilizados como parceiros de pair programming para suporte no desenvolvimento, produtividade, estruturação inicial e validação de cenários complexos (edge cases).

## 🚀 Como foi utilizada (Produtividade e Desenvolvimento)
- **Aceleração do Esqueleto Inicial:** Auxílio na criação rápida das estruturas base dos modelos de dados em C# e DTOs com base no contrato do desafio.
- **Suporte aos Testes Unitários:** Apoio na estruturação lógica dos cenários iniciais para os testes em xUnit, agilizando a cobertura das regras de negócio.
- **Refinamento de Código:** Auxílio na organização de trechos repetitivos e na aplicação de boas práticas de separação de responsabilidades (Service Layer).

## ⚠️ Exemplo de Erro da IA e Correção Crítica
- **Erro da IA:** Inicialmente, a IA sugeriu uma lógica de validação de sobreposição de horários muito restritiva, que bloqueava a marcação de uma nova aula no exato horário em que a aula anterior terminava (ex: considerando inválida uma aula que começava às 19:00 quando havia outra terminando às 19:00).
- **Diagnóstico e Correção:** Como o requisito do desafio permitia expressamente que as aulas **encotassem** (`fim` de uma igual ao `inicio` da outra), percebi a falha na lógica gerada pela ferramenta. Corrigi manualmente a expressão booleana de verificação de conflitos para respeitar o limite exato do encostamento, garantindo que o comportamento estivesse 100% aderente às regras exigidas.