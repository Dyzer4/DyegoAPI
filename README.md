# TIVIT.CIPA.API

Candidate features: 
- Update Candidate 
- New Candidate
- Deactive/active Candidate
- List active Candidates by election (only actives)
- Search by filters (name, corporateid, department, siteid) (only actives)
- Search Candidate by id

Voter features:
- Search Voter by id
- List active voters by Election (only actives)
- Deactive/active Voter

User features:
- Search user by id
- Deactive/active User

Site features: 
- Search Site by id
- Deactive/active Site
- List all Sites / only actives

Election features:
- List all Elections
- Search Elction by id
- Deactive/active Election
- Update Election (DON NOT TRY THAT YET, WAIT FIX!!!!!)

## Estrutura de pastas
```
TIVIT.CIPA.Api/
│
├── Attributes/
│   └── [.cs files] - Atributos customizados
│
├── Controllers/
│   └── [.cs files] - Controllers REST
│
├── Extensions/
│   └── [.cs files] - Métodos de extensão
│
├── Properties/
│   └── [.json files] - Configurações de inicialização
│
├── Security/
│   └── [.cs files] - Handlers de segurança
│
├── Templates/
│   └── Email/
│       └── [.html files] - Templates de email
│
└── [.csproj files] - Arquivo de projeto
└── [.tmp files] - Arquivos de backup


TIVIT.CIPA.Api.Domain/
│
├── Business/
│   └── [.cs files] - Lógica de negócio
│
├── Extensions/
│   └── [.cs files] - Extensões de classe
│
├── Helpers/
│   └── [.cs files] - Classes auxiliares
│
├── Model/
│   ├── [.cs files] - Entidades de domínio
│   └── Requests/
│       └── [.cs files] - DTOs de requisição
│
├── Providers/
│   └── [.cs files] - Provedores de serviços
│
├── Repositories/
│   ├── Config/
│   │   └── [.cs files] - Configurações de entidades
│   ├── Context/
│   │   └── [.cs files] - DbContext
│   └── [.cs files] - Interfaces de repository
│
├── Resources/
│   └── [.resx files] - Recursos de tradução/mensagens
│
├── Services/
│   └── [.cs files] - Serviços de domínio
│
├── Validators/
│   └── [.cs files] - Validadores de negócio
│
└── [.csproj files] - Arquivo de projeto
```
