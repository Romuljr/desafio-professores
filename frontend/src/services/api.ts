import type { BuscaRequest, BuscaResponse } from '../types/professores';

// ATENÇÃO: Substitua pela porta exata que aparece quando você roda a sua API C#
const API_URL = 'http://localhost:5000/api/professores/busca';

export async function buscarProfessores(dados: BuscaRequest): Promise<BuscaResponse> {
  const response = await fetch(API_URL, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(dados),
  });

  // Se a API retornar erro (ex: 400 Bad Request)
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.mensagem || 'Falha de comunicação com a API.');
  }

  return response.json();
}