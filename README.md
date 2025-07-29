# SGHSS - Sistema de Gestão de Horários da Saúde
Este sistema permite o agendamento de consultas médicas de forma estruturada e segura, garantindo a correta associação entre instituições, profissionais e pacientes.

## 📋 Compreendendo o Fluxo e as Dependências
Para que o SGHSS funcione corretamente e permita o agendamento de consultas, é essencial seguir uma ordem lógica de cadastro e associação entre as entidades do sistema.

### O fluxo está dividido em três etapas principais:

### 🏗️ 1. Configuração da Estrutura (Administrador)
Esta é a base do sistema. Antes de qualquer agendamento ser possível, um administrador precisa realizar os cadastros fundamentais, seguindo a ordem de fluxo e relacionamento:

---

#### 1 - Adm
Para que possa realizar os cadastros, primeiro é necessário criar uma conta ADM, que atualmente está sem autenticação, para fim de testes. Onde ele já vem com todas as permissões do sistema.
Endpoint: /api/v1/adm/cadastrar
<img width="1000" height="171" alt="image" src="https://github.com/user-attachments/assets/fe72fa01-fa6a-42a3-a202-e72227dde931" />

Enums necessários:
```csharp
public enum CargoFuncionario
{
    Recepcionista = 0,
    Gerente = 1,
    Coordenador = 2,
    Diretor = 3,
    Estagiario = 4,
    Atendente = 5,
    AdmLocal = 6,
    AdmGeral = 7
}
```

#### 2 - Instituições
Representam os locais (físicos ou virtuais) de atendimento, como hospitais, clínicas e laboratórios.
Endpoint: /api/v1/instituicao/cadastrar
<img width="976" height="166" alt="image" src="https://github.com/user-attachments/assets/6eedcc67-bcd8-4b75-9284-4532c78a752b" />

Enums necessários:
```csharp
public enum TipoInstituicaoEnum
{
    Clinica = 0,
    Hospital = 1,
    ProntoSocorro = 2,
    Laboratorio = 3,
    UBS = 4,
    PostoDeSaude = 5,
    UPA = 6,
    Maternidade = 7,
    Caps = 8,
    UnidadeMovel = 9,
    CentroOftalmologico = 10,
    CasaDeRepouso = 11
}
```


#### Especialidades e Serviço laboratório
São as áreas de atuação médica (Cardiologia, Dermatologia, etc.) ou de serviços de laboratório (Exame de sangue, hemograma, exame de urina, etc.) que categorizam os serviços e os profissionais.
Endpoint Especialidades: /api/v1/servico/especializacao/cadastrar
Endpoint Serviços Lavboratório: /api/v1/servico/servico-laboratorio/cadastrar
<img width="998" height="265" alt="image" src="https://github.com/user-attachments/assets/f7b08d21-7330-4b64-8e8e-232e4784a13c" />



#### Profissionais
Médicos, enfermeiros e técnicos que prestarão os atendimentos. Este cadastro é feito internamente pelo administrador.
Endpoint: /api/v1/profissional/cadastrar
<img width="972" height="178" alt="image" src="https://github.com/user-attachments/assets/4ba7ed35-3344-4827-8c52-3f5bfce7f782" />

Enums necessários:
```csharp
public enum CargoProfissionalSaude
{
    Medico = 0,
    Enfermeiro = 1,
    Tecnico = 2,
    Estagiario = 3,
    Auxiliar = 4,
    Biomédico = 5
}
```


#### Funcionários
Recepcionista, gerente, adm local. Este cadastro é feito internamente pelo administrador.
Endpoint: /api/v1/funcionario/cadastrar
<img width="976" height="176" alt="image" src="https://github.com/user-attachments/assets/f90db980-93cb-4cbb-97fc-99cc2c3a6fd2" />

Enums necessários:
```csharp
public enum CargoFuncionario
{
    Recepcionista = 0,
    Gerente = 1,
    Coordenador = 2,
    Diretor = 3,
    Estagiario = 4,
    Atendente = 5,
    AdmLocal = 6,
    AdmGeral = 7
}
```


📌 Nota: Nessa etapa, apenas os dados isolados são inseridos. Ainda não há vínculo entre eles.

#### 🔗 2. Associação e Oferta de Serviço (Administrador)
Agora, é hora de conectar as peças cadastradas:

#### Associação entre Serviço laboratório/Especialização e Instituição
Ex: A instituição Clinica e Saúde, possui a especialização pediatria.
Endpoint: /api/v1/instituicao/vincular-servico-laboratorio/{instituicaoId}/{servicoLaboratorioId}
Endpoint: /api/v1/instituicao/vincular-especializacao/{instituicaoId}/{servicoLaboratorioId}
<img width="947" height="81" alt="image" src="https://github.com/user-attachments/assets/d62b74a8-87ff-401a-a5f1-e1498aacfcec" />
<img width="961" height="96" alt="image" src="https://github.com/user-attachments/assets/b30fa9b7-d5c5-4a28-8a26-0741b7a43b9f" />

#### Associação entre Profissional e Instituição
Ex: A Dra. Maria atende na Clínica Vida e Saúde.
Endpoint: /api/v1/instituicao/profissional/{instituicaoId}/{profissionalId}
<img width="962" height="80" alt="image" src="https://github.com/user-attachments/assets/452a71f1-3985-4993-8b74-a992089cd5fc" />

#### Associação entre Profissional e Especialidade
Ex: A Dra. Maria possui a especialidade "Cardiologia".
Endpoint: /api/v1/profissional/vincular-servico-laboratorio/{profissionalId}/{servicoLaboratorioId}
Endpoint: /api/v1/profissional/vincular-especializacao/{profissionalId}/{especializacaoId}
<img width="952" height="87" alt="image" src="https://github.com/user-attachments/assets/e937a2d9-9915-499d-964d-0d969b625540" />
<img width="957" height="97" alt="image" src="https://github.com/user-attachments/assets/c69f7c23-b1a4-428b-a98d-c2940affe3ed" />

#### Agenda Profissional saúde
É necessário definir dias e horários de atendimento em cada instituição que o profissional atua.
Esta é a oferta de serviço que torna o profissional visível para agendamentos.
Endpoint: /api/v1/profissional/cadastrar-agenda
<img width="966" height="75" alt="image" src="https://github.com/user-attachments/assets/3320dbd2-a828-4608-bef1-98bb63652771" />

Enums necessários:
```csharp
public enum DiaSemanaEnum
{
    Domingo = 0,
    Segunda = 1,
    Terca = 2,
    Quarta = 3,
    Quinta = 4,
    Sexta = 5,
    Sabado = 6
}
```


#### Agenda Instituição
Agenda utilizada para o cadastro da disponibilidade institucional de atendimentos.
Endpoint: /api/v1/instituicao/cadastrar-agenda
<img width="981" height="71" alt="image" src="https://github.com/user-attachments/assets/fcdf3386-f83e-4d81-a7d1-e29c84b80a38" />

Enums necessários:
```csharp
public enum DiaSemanaEnum
{
    Domingo = 0,
    Segunda = 1,
    Terca = 2,
    Quarta = 3,
    Quinta = 4,
    Sexta = 5,
    Sabado = 6
}
```


#### 📘 Exemplo de Requisição – Agendas

json
{
  "instituicaoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "diaSemana": 0,
  "horaInicio": "08:00",
  "horaFim": "17:00"
}

🔸 Campos:

instituicaoId: GUID da instituição cadastrada.

diaSemana: número do dia da semana (0 = Domingo, 1 = Segunda, ..., 6 = Sábado). Veja DiaSemanaEnum.

horaInicio: horário de início do expediente (formato HH:mm).

horaFim: horário de término do expediente (formato HH:mm).

📌 Resumindo: Para que um profissional esteja disponível para agendamento, ele precisa:

Estar cadastrado.

Estar vinculado a uma instituição.

Estar vinculado a uma especialidade.

Ter uma agenda cadastrada para aquele local.



#### 👤 Ação do Paciente
Somente após as etapas anteriores estarem completas, o paciente pode interagir com o sistema:

Autocadastro
O paciente cria sua conta.

O paciente busca por Especialidades e quando clica em uma específica, por exemplo, "Cardiologia", o sistema:
<img width="961" height="81" alt="image" src="https://github.com/user-attachments/assets/de69878e-ceca-4a0d-98f3-3d0d84c47344" />
<img width="955" height="70" alt="image" src="https://github.com/user-attachments/assets/da63279a-5ffb-4888-a2cb-8f2d6790847c" />

Encontra todas as instituições que possuem profissionais disponiveis para aquela especialidade se for presencial, se for teleconsulta ou homecare, pula esta parte
<img width="952" height="67" alt="image" src="https://github.com/user-attachments/assets/b1d95458-275f-4ae7-8176-ddb6854c8240" />

Encontra todos os profissionais com essa especialidade, trazendo a grade com seus horários livres.
<img width="957" height="93" alt="image" src="https://github.com/user-attachments/assets/5fa88e80-0854-44df-914e-73c389e689a3" />
<img width="966" height="96" alt="image" src="https://github.com/user-attachments/assets/b98958c1-0907-44b8-b377-645b4a1e34a5" />

O agendamento é então realizado, ligando:

Paciente + Profissional + Instituição + Especialidade + Horário



#### Enums gerais
```csharp
public enum GeneroEnum
{
    Feminino = 0,
    Masculino = 1,
    NaoDefinido = 2
}
```
```csharp
public enum EstadosEnum
{
    AC = 0,  // Acre
    AL = 1,  // Alagoas
    AP = 2,  // Amapá
    AM = 3,  // Amazonas
    BA = 4,  // Bahia
    CE = 5,  // Ceará
    DF = 6,  // Distrito Federal
    ES = 7,  // Espírito Santo
    GO = 8,  // Goiás
    MA = 9,  // Maranhão
    MT = 10, // Mato Grosso
    MS = 11, // Mato Grosso do Sul
    MG = 12, // Minas Gerais
    PA = 13, // Pará
    PB = 14, // Paraíba
    PR = 15, // Paraná
    PE = 16, // Pernambuco
    PI = 17, // Piauí
    RJ = 18, // Rio de Janeiro
    RN = 19, // Rio Grande do Norte
    RS = 20, // Rio Grande do Sul
    RO = 21, // Rondônia
    RR = 22, // Roraima
    SC = 23, // Santa Catarina
    SP = 24, // São Paulo
    SE = 25, // Sergipe
    TO = 26  // Tocantins
}
```
```csharp
public enum StatusPagamentoEnum
{
    Pendente = 0,
    Aprovado = 1,
    Rejeitado = 2,
    EmProcessamento = 3,
    Cancelado = 4
}

public enum FormaDePagamentoEnum
{
    CreditoAVista = 0,
    CreditoParcelado = 1,
    Debito = 2,
    Pix = 3,
    Dinheiro = 4,
    Convenio = 5
}
```
```csharp
public enum TipoLeitoEnum
{
    Enfermaria = 0,
    UTIAdulto = 1,
    UTIPediatrica = 2,
    UTINeonatal = 3,
    UnidadeIntermediaria = 4,
    Observacao = 5,
    Isolamento = 6,
    Maternidade = 7,
    PosOperatorio = 8,
    Pediatria = 9,
    Neonatal = 10,
    Psiquiatrico = 11,
    LongaPermanencia = 12,
    SemIdentificacao = 13
}

public enum StatusLeitoEnum
{
    Liberado = 0,
    EmUso = 1,
    EmManutencao = 2,
    Inativo = 3
}

public enum TipoConsultaEnum
{
    HomeCare = 0,
    TeleConsulta = 1,
    Presencial = 2
}

public enum StatusConsultaEnum
{
    Pendente = 0,
    EmAtendimento = 1,
    Finalizada = 2,
    Cancelada = 3
}

public enum DiaSemanaEnum
{
    Domingo = 0,
    Segunda = 1,
    Terca = 2,
    Quarta = 3,
    Quinta = 4,
    Sexta = 5,
    Sabado = 6
}
```

✅ Checklist para Testar o Sistema:

- Cadastrar ADM
  
- Cadastrar Instituições

- Cadastrar Especialidades

- Cadastrar Profissionais

- Associar Profissionais às Instituições

- Associar Profissionais às Especialidades

- Cadastrar Agendas dos Profissionais e instituições

- Autocadastrar Paciente

- Realizar Busca e Agendamento

- Demais cadastros de leitos e suprimentos

#### 🔐 Autenticação com Token Bearer
A API do SGHSS utiliza autenticação baseada em JWT Token (Bearer Token). Siga os passos abaixo:

Login:
Faça o login com e-mail e senha para receber um token JWT.
<img width="963" height="158" alt="image" src="https://github.com/user-attachments/assets/3175ba54-f3f4-44e1-9988-d214fe22d778" />

Autorização:
Para acessar endpoints protegidos, envie o token no header da requisição:

Authorization: Bearer {seu_token_aqui}


