describe("Customer Create Screen", () => {
  beforeEach(() => {
    cy.visit("/");
  });

  it("should load the customer create form", () => {
    cy.contains("Cadastro de Cliente").should("be.visible");

    cy.get('[data-testid="btn-submit"]').should("be.visible");
  });

  it("should auto-fill address when typing a CEP", () => {
    cy.intercept("GET", "**/postal-codes/11065-651", {
      statusCode: 200,
      body: {
        cep: "11065-651",
        street: "Avenida Barão de Penedo",
        neighborhood: "José Menino",
        city: "Santos",
        state: "SP",
      },
    }).as("getCep");

    cy.get('[data-testid="customer-postalCode"]').clear().type("11065-651").blur();

    cy.wait("@getCep");

    cy.get('[data-testid="customer-street"]').should(
      "have.value",
      "Avenida Barão de Penedo"
    );

    cy.get('[data-testid="customer-neighborhood"]').should("have.value", "José Menino");

    cy.get('[data-testid="customer-city"]').should("have.value", "Santos");

    cy.get('[data-testid="customer-state"]').should("have.value", "SP");
  });

  it("should show inline loading while searching CEP", () => {
    cy.intercept("GET", "**/postal-codes/11065-651", (req) => {
      req.on("response", (res) => {
        res.setDelay(800);
      });

      req.reply({
        statusCode: 200,
        body: {
          cep: "11065-651",
          street: "Avenida Barão de Penedo",
          neighborhood: "José Menino",
          city: "Santos",
          state: "SP",
        },
      });
    }).as("getCepSlow");

    cy.get('[data-testid="customer-postalCode"]').clear().type("11065-651").blur();

    cy.get('[data-testid="postalcode-loading"]').should("be.visible");

    cy.wait("@getCepSlow");

    cy.get('[data-testid="postalcode-loading"]').should("not.exist");
  });

  it("should create a customer and show success modal", () => {
    cy.intercept("POST", "**/customers", {
      statusCode: 201,
      body: { id: "11111111-1111-1111-1111-111111111111" },
    }).as("createCustomer");

    cy.intercept("GET", "**/postal-codes/11065-651", {
      statusCode: 200,
      body: {
        cep: "11065-651",
        street: "Avenida Barão de Penedo",
        neighborhood: "José Menino",
        city: "Santos",
        state: "SP",
      },
    }).as("getCep");

    cy.get('[data-testid="customer-type"]').select("Pessoa Física");
    cy.get('[data-testid="customer-name"]').type("Nelson");
    cy.get('[data-testid="customer-cpfCnpj"]').type("39940838859");
    cy.get('[data-testid="customer-email"]').type("nelson@email.com");
    cy.get('[data-testid="customer-phone"]').type("11999999999");

    cy.get('[data-testid="customer-date"]').type("1990-06-13");

    cy.get('[data-testid="customer-postalCode"]').type("11065-651").blur();
    cy.wait("@getCep");

    cy.get('[data-testid="btn-submit"]').click();

    cy.wait("@createCustomer");

    cy.get('[data-testid="modal-success"]').should("be.visible");
    cy.contains("Cliente cadastrado com sucesso").should("be.visible");
  });

  it("should allow creating another customer from success modal", () => {
    cy.intercept("POST", "**/customers", {
      statusCode: 201,
      body: { id: "22222222-2222-2222-2222-222222222222" },
    }).as("createCustomer");

    cy.intercept("GET", "**/postal-codes/11065-651", {
      statusCode: 200,
      body: {
        cep: "11065-651",
        street: "Avenida Barão de Penedo",
        neighborhood: "José Menino",
        city: "Santos",
        state: "SP",
      },
    }).as("getCep");

    cy.get('[data-testid="customer-type"]').select("Pessoa Física");
    cy.get('[data-testid="customer-name"]').type("Nelson");
    cy.get('[data-testid="customer-cpfCnpj"]').type("39940838859");
    cy.get('[data-testid="customer-email"]').type("nelson@email.com");
    cy.get('[data-testid="customer-date"]').type("1990-06-13");
    cy.get('[data-testid="customer-postalCode"]').type("11065-651").blur();
    cy.wait("@getCep");

    cy.get('[data-testid="btn-submit"]').click();
    cy.wait("@createCustomer");

    cy.get('[data-testid="modal-success"]').should("be.visible");

    cy.get('[data-testid="btn-create-another"]').click();

    // modal fechou
    cy.get('[data-testid="modal-success"]').should("not.exist");

    // form resetado
    cy.get('[data-testid="customer-name"]').should("have.value", "");
    cy.get('[data-testid="customer-email"]').should("have.value", "");
    cy.get('[data-testid="customer-cpfCnpj"]').should("have.value", "");
  });

  it("should show API validation errors", () => {
    cy.intercept("POST", "**/customers", {
      statusCode: 400,
      body: {
        message: "Validation failed",
        errors: [{ propertyName: "Email", errorMessage: "E-mail já cadastrado" }],
      },
    }).as("createCustomerFail");

    cy.get('[data-testid="customer-type"]').select("Pessoa Física");
    cy.get('[data-testid="customer-name"]').type("Nelson");
    cy.get('[data-testid="customer-cpfCnpj"]').type("39940838859");
    cy.get('[data-testid="customer-email"]').type("nelson@email.com");
    cy.get('[data-testid="customer-date"]').type("1990-06-13");

    cy.get('[data-testid="btn-submit"]').click();

    cy.wait("@createCustomerFail");

    cy.contains("E-mail já cadastrado").should("be.visible");
  });

  it("should show API conflict message (409) as submit error", () => {
    cy.intercept("POST", "**/customers", {
      statusCode: 409,
      body: { message: "E-mail já cadastrado" },
    }).as("createCustomerConflict");

    cy.get('[data-testid="customer-type"]').select("Pessoa Física");
    cy.get('[data-testid="customer-name"]').type("Nelson");
    cy.get('[data-testid="customer-cpfCnpj"]').type("39940838859");
    cy.get('[data-testid="customer-email"]').type("nelson@email.com");
    cy.get('[data-testid="customer-date"]').type("1990-06-13");

    cy.get('[data-testid="btn-submit"]').click();

    cy.wait("@createCustomerConflict");

    cy.get('[data-testid="submit-error"]').should("be.visible");
    cy.get('[data-testid="submit-error"]').should("contain.text", "E-mail já cadastrado");
  });

  it("should show client-side required validation (example)", () => {
    cy.get('[data-testid="btn-submit"]').click();

    cy.contains("Nome é obrigatório").should("be.visible");
    cy.contains("E-mail é obrigatório").should("be.visible");
    cy.contains("Data é obrigatória").should("be.visible");
  });
});