## Cinema!

### Requisitos:

	- .NET Framework 4.7.2
	- Instância padrão "mssqllocaldb" do SQL Server Express LocalDB
	- Node.js
	
	
### Iniciando a Aplicação

	1. Na pasta Client executar os comandos:
      npm install
      npm start
	
	2. Abrir no Visual Studio a solução da pasta Server, restaurar os pacotes Nuget e iniciar aplicação pelo projeto WebAPI.
	
	3. Na tela de login utilizar as credenciais pré cadastradas:
      username: admin
      senha: admin
      
      ou
      
      username: customer
      senha: customer
	

### Arquitetura e Organização

Para o desenvolvimento do sistema foi utilizada a arquitetura de camadas baseada no modelo Domain Driven Design da figura abaixo. 

![Arquitetura de Camadas](https://user-images.githubusercontent.com/42355371/74002848-3ba2f200-494f-11ea-9488-c3a22e4f53bd.jpg)

A camada de apresentação foi desenvolvida utilizando Angular 8 e o restante das camadas está na solução do Visual Studio, na qual cada projeto representa uma camada. As responsabilidades de cada camada são:

	- Apresentação: interação com o usuário;
	- Serviços Distribuidos: disponibiliza endpoints para serem utilizados pela camada de apresentação;
	- Aplicação: gerencia os recursos da solução;
	- Domínio: contém os objetos e as regras de negócio;
	- Infraestrutura: serviços externos e camada de acesso à dados.

O sistema está organizado por Features. Por exemplo, para o gerenciamento das Salas do cinema foi identificada a feature Lounge, a qual foi decomposta em todas as camadas da solução. Na camada de serviços distribuídos existe o Controller para disponibilização dos endpoints. Na camada de aplicação o LoungeService gerencia os fluxos da funcionalidade. Na mesma camada ainda estão os Commands, Queries e ViewModels, que são DTOs utilizados na comunicação com o front-end. Na camada de domínio estão os objetos e os contratos de funcionamento da Feature. Na camada de acesso à dados está o repositório para gerenciar o armazenamento. No front-end existe também uma pasta Features, contendo os componentes e serviços de cada funcionalidade.

![image](https://user-images.githubusercontent.com/42355371/74003137-5cb81280-4950-11ea-9570-4094209d04b8.png)
