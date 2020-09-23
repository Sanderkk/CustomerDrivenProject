/* Test routing and navigation by clicking on the different navigation elemnts and 
   verify that the new url is correct
*/
describe("Test routing", () => {
  it("Visit home page", () => {
    cy.visit("/"); // Open home page
    cy.url().should("eq", Cypress.config().baseUrl); // Verify that home page url matches baseUrl defined in cypress.json
  });

  it("Visit admin page", () => {
    cy.contains("Admin Page").click(); // Find "Admin Page" text and click it
    cy.url().should("include", "/admin"); // Verify that the new url includes "/admin"
  });

  it("Visit customers page", () => {
    cy.contains("Customers").click();
    cy.url().should("include", "/customers");
  });

  it("Visit dashboards page", () => {
    console.log(cy.url());
    cy.contains("Dashboards").click();
    cy.url().should("include", "/dashboards");
  });

  it("Visit home page by pressing logo", () => {
    cy.get(".navbar_logo").click(); // Find element with clasName "navbar_logo" and click it
    cy.url().should("eq", Cypress.config().baseUrl);
  });
});
