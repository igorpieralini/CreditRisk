
# CreditRisk

<sub>Sistema de análise e gerenciamento de risco de crédito · Arquitetura de Microsserviços · .NET 10</sub>

<br/>

Sistema backend desenvolvido em **C# e ASP.NET Core**, organizado em cinco microsserviços responsáveis por análise de crédito, gestão de garantias, controle de exposição financeira, decisões de risco e monitoramento de operações.

A aplicação utiliza **PostgreSQL**, **Entity Framework Core** e **RabbitMQ**, com APIs REST, separação de responsabilidades e testes automatizados com xUnit.

<br/>

<img width="100%" src="https://capsule-render.vercel.app/api?type=rect&color=000000&height=1" alt=""/>

<br/>

<div align="center">

### <samp>// microsserviços</samp>

<br/>

**CreditAnalysis**

<samp>Análise financeira e avaliação de operações de crédito</samp>

<br/><br/>

**Guarantee**

<samp>Cadastro, avaliação e gerenciamento de garantias</samp>

<br/><br/>

**Exposure**

<samp>Controle de limites, exposições e movimentações financeiras</samp>

<br/><br/>

**RiskDecision**

<samp>Aplicação de políticas e classificação de decisões de crédito</samp>

<br/><br/>

**Monitoring**

<samp>Monitoramento de operações, eventos e alertas de risco</samp>

<br/>

</div>

<br/>

<img width="100%" src="https://capsule-render.vercel.app/api?type=rect&color=000000&height=1" alt=""/>

<br/>

<div align="center">

### <samp>// funcionalidades</samp>

<br/>

<samp>Analisar crédito &bull; Avaliar garantias &bull; Consultar exposições</samp>

<br/><br/>

<samp>Gerenciar limites &bull; Registrar movimentações &bull; Aplicar políticas</samp>

<br/><br/>

<samp>Aprovar &bull; Rejeitar &bull; Encaminhar para revisão manual</samp>

<br/><br/>

<samp>Monitorar operações &bull; Gerar alertas &bull; Registrar eventos</samp>

</div>

<br/>

<img width="100%" src="https://capsule-render.vercel.app/api?type=rect&color=000000&height=1" alt=""/>

<br/>

<div align="center">

### <samp>// tecnologias</samp>

<br/>

<table align="center">
  <tr>
    <td align="center" width="140">
      <br/>
      <img src="https://skillicons.dev/icons?i=cs" width="40" alt="C#"/>
      <br/><br/>
      <samp>C#</samp>
      <br/><br/>
    </td>
    <td align="center" width="140">
      <br/>
      <img src="https://skillicons.dev/icons?i=dotnet" width="40" alt=".NET"/>
      <br/><br/>
      <samp>.NET 10</samp>
      <br/><br/>
    </td>
    <td align="center" width="140">
      <br/>
      <img src="https://skillicons.dev/icons?i=postgres" width="40" alt="PostgreSQL"/>
      <br/><br/>
      <samp>PostgreSQL</samp>
      <br/><br/>
    </td>
    <td align="center" width="140">
      <br/>
      <img src="https://skillicons.dev/icons?i=rabbitmq" width="40" alt="RabbitMQ"/>
      <br/><br/>
      <samp>RabbitMQ</samp>
      <br/><br/>
    </td>
    <td align="center" width="140">
      <br/>
      <img src="https://skillicons.dev/icons?i=rider" width="40" alt="Rider"/>
      <br/><br/>
      <samp>Rider</samp>
      <br/><br/>
    </td>
  </tr>
</table>

<br/>

<samp>ASP.NET Core &bull; Entity Framework Core &bull; MassTransit</samp>

<br/><br/>

<samp>xUnit &bull; Moq &bull; Swagger &bull; APIs REST</samp>

</div>

<br/>

<img width="100%" src="https://capsule-render.vercel.app/api?type=rect&color=000000&height=1" alt=""/>

<br/>

<div align="center">

### <samp>// arquitetura</samp>

</div>

```text
CreditRisk/
├── CreditRisk.CreditAnalysis/
│   └── Análise e cálculos de crédito
│
├── CreditRisk.Guarantee/
│   └── Garantias e avaliações
│
├── CreditRisk.Exposure/
│   └── Limites e exposição financeira
│
├── CreditRisk.RiskDecision/
│   └── Políticas e decisões de risco
│
├── CreditRisk.Monitoring/
│   └── Monitoramento e alertas
│
└── CreditRisk.Tests/
    ├── CreditAnalysis/
    ├── Guarantee/
    ├── Exposure/
    ├── RiskDecision/
    └── Monitoring/
```

<br/>

<img width="100%" src="https://capsule-render.vercel.app/api?type=rect&color=000000&height=1" alt=""/>

<br/>

<div align="center">

### <samp>// comunicação e persistência</samp>

<br/>

<samp>REST APIs &bull; RabbitMQ &bull; MassTransit</samp>

<br/><br/>

<samp>PostgreSQL &bull; Supabase &bull; Entity Framework Core</samp>

<br/>

</div>

Cada microsserviço possui responsabilidades próprias. Os serviços de persistência utilizam PostgreSQL, enquanto o RabbitMQ permite o processamento assíncrono de mensagens.

A comunicação via mensageria foi validada no serviço de monitoramento. A publicação automática de eventos entre todos os serviços ainda está em desenvolvimento.

<br/>

<img width="100%" src="https://capsule-render.vercel.app/api?type=rect&color=000000&height=1" alt=""/>

<br/>

<div align="center">

### <samp>// testes automatizados</samp>

<br/>

**175 testes aprovados · 0 falhas**

<br/>

<samp>xUnit &bull; Testes de regras &bull; Cálculos &bull; Serviços</samp>

</div>

<br/>

Para executar os testes:

```cmd
dotnet test CreditRisk.Tests/CreditRisk.Tests.csproj
```

A suíte inclui testes de comportamento e verificações estruturais. A cobertura integral das regras de negócio ainda não foi estabelecida.

<br/>

<img width="100%" src="https://capsule-render.vercel.app/api?type=rect&color=000000&height=1" alt=""/>

<br/>

<div align="center">

### <samp>// execução</samp>

</div>

**Requisitos**

- .NET SDK 10
- PostgreSQL
- RabbitMQ
- Configuração local das credenciais de conexão

**Portas dos serviços**

| Microsserviço | Porta |
|---|---|
| CreditAnalysis | 5207 |
| Guarantee | 5100 |
| Exposure | 5242 |
| RiskDecision | 5272 |
| Monitoring | 5181 |

Execute cada microsserviço em seu próprio terminal, configurando previamente os serviços externos e as credenciais locais.

As credenciais do banco de dados não são distribuídas no repositório.

<br/>

<img width="100%" src="https://capsule-render.vercel.app/api?type=rect&color=000000&height=1" alt=""/>

<br/>

<div align="center">

### <samp>// conceitos aplicados</samp>

<br/>

<samp>Microsserviços &bull; SOLID &bull; Injeção de Dependência</samp>

<br/><br/>

<samp>Separação de Camadas &bull; Regras de Negócio &bull; Persistência</samp>

<br/><br/>

<samp>Mensageria &bull; Testes Unitários &bull; APIs REST</samp>

</div>
