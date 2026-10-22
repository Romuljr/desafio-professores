import React, { useState } from 'react';
import type { BuscaRequest, BuscaResponse } from './types/professores';
import { buscarProfessores } from './services/api';
import './App.css';

const DIAS_SEMANA = [
  { id: 'segunda', label: 'Segunda-feira' },
  { id: 'terca', label: 'Terça-feira' },
  { id: 'quarta', label: 'Quarta-feira' },
  { id: 'quinta', label: 'Quinta-feira' },
  { id: 'sexta', label: 'Sexta-feira' },
  { id: 'sabado', label: 'Sábado' },
  { id: 'domingo', label: 'Domingo' },
];

export function App() {
  // Estados do Formulário
  const [idioma, setIdioma] = useState('Espanhol');
  const [dias, setDias] = useState<string[]>(['segunda']);
  const [inicioFaixa, setInicioFaixa] = useState('18:00');
  const [fimFaixa, setFimFaixa] = useState('20:00');
  const [duracaoMinutos, setDuracaoMinutos] = useState<number>(60);

  // Estados de Controle da Tela (Exigidos pelo desafio)
  const [loading, setLoading] = useState(false);
  const [erro, setErro] = useState<string | null>(null);
  const [resultado, setResultado] = useState<BuscaResponse | null>(null);

  // Manipular seleção de múltiplos dias com checkbox
  const handleDiaToggle = (diaId: string) => {
    setDias((prev) =>
      prev.includes(diaId) ? prev.filter((d) => d !== diaId) : [...prev, diaId]
    );
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setErro(null);
    setResultado(null);

    const payload: BuscaRequest = {
      idioma,
      dias,
      inicioFaixa,
      fimFaixa,
      duracaoMinutos: Number(duracaoMinutos),
    };

    try {
      const data = await buscarProfessores(payload);
      setResultado(data);
    } catch (err: any) {
      // Captura tanto erro 400 da API quanto falha de rede (API desligada)
      setErro(err.message || 'Falha de comunicação com a API.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container">
      <header>
        <h1>📚 Encontre um Professor</h1>
        <p>Escola de Idiomas - Sistema de Busca</p>
      </header>

      {/* Formulário */}
      <form onSubmit={handleSubmit} className="form-card">
        <div className="form-row">
          <div className="form-group">
            <label>Idioma:</label>
            <select value={idioma} onChange={(e) => setIdioma(e.target.value)}>
              <option value="Inglês">Inglês</option>
              <option value="Espanhol">Espanhol</option>
              <option value="Francês">Francês</option>
            </select>
          </div>

          <div className="form-group">
            <label>Duração da Aula:</label>
            <select
              value={duracaoMinutos}
              onChange={(e) => setDuracaoMinutos(Number(e.target.value))}
            >
              <option value={30}>30 minutos</option>
              <option value={60}>60 minutos (1h)</option>
              <option value={90}>90 minutos (1h30)</option>
            </select>
          </div>
        </div>

        <div className="form-row">
          <div className="form-group">
            <label>Início da Faixa:</label>
            <input
              type="time"
              value={inicioFaixa}
              onChange={(e) => setInicioFaixa(e.target.value)}
            />
          </div>

          <div className="form-group">
            <label>Fim da Faixa:</label>
            <input
              type="time"
              value={fimFaixa}
              onChange={(e) => setFimFaixa(e.target.value)}
            />
          </div>
        </div>

        <div className="form-group">
          <label>Dias da Semana:</label>
          <div className="checkbox-grid">
            {DIAS_SEMANA.map((d) => (
              <label key={d.id} className="checkbox-label">
                <input
                  type="checkbox"
                  checked={dias.includes(d.id)}
                  onChange={() => handleDiaToggle(d.id)}
                />
                {d.label}
              </label>
            ))}
          </div>
        </div>

        <button type="submit" className="btn-submit" disabled={loading}>
          {loading ? 'Buscando professores...' : 'Buscar Professores'}
        </button>
      </form>

      {/* Estado: Erro (Validação 400 ou Falha de Comunicação) */}
      {erro && <div className="alert alert-error">⚠️ {erro}</div>}

      {/* Estado: Carregando */}
      {loading && <div className="loading">Carregando resultados...</div>}

      {/* Resultados da Busca */}
      {resultado && (
        <div className="results-section">
          <h2>Professores Disponíveis</h2>

          {/* Estado: Nenhum professor encontrado */}
          {resultado.professores.length === 0 ? (
            <div className="alert alert-warning">
              Nenhum professor encontrado para os critérios selecionados.
            </div>
          ) : (
            <div className="professores-grid">
              {resultado.professores.map((prof) => (
                <div key={prof.id} className="professor-card">
                  <h3>{prof.nome}</h3>
                  <span className="badge">
                    {prof.aulasNaSemana} {prof.aulasNaSemana === 1 ? 'aula' : 'aulas'} na semana
                  </span>
                  
                  <div className="horarios-box">
                    <strong>Horários disponíveis:</strong>
                    <div className="tags-list">
                      {prof.horarios.map((h, index) => (
                        <span key={index} className="tag">{h}</span>
                      ))}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}

          {/* Diferencial: Professores Não Elegíveis e Motivos */}
          {resultado.naoElegiveis.length > 0 && (
            <div className="nao-elegiveis-box">
              <h3>Professores Indisponíveis (Motivos)</h3>
              <ul>
                {resultado.naoElegiveis.map((nao) => (
                  <li key={nao.id}>
                    <strong>{nao.nome}:</strong> {nao.motivo}
                  </li>
                ))}
              </ul>
            </div>
          )}
        </div>
      )}
    </div>
  );
}

export default App;